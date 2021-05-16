using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using System.Linq;
using System;

using Object = UnityEngine.Object;

namespace GTVariable.Editor
{
    public class SideBarTreeView : TreeView
    {
        public Object SelectedObject { get; private set; }
        public SerializedProperty SelectedProperty { get; private set; }
        public string CurrentChannelName { get; private set; }
        public string SelectedName { get; private set; }
        private SerializedObject serializedObject;
        private SearchField searchField;
        private ComponenetCreator creator = new ComponenetCreator();
        private bool creatingNewAsset;


        public SideBarTreeView(SerializedObject serializedObject, TreeViewState treeViewState)
        : base(treeViewState)
        {
            this.serializedObject = serializedObject;
            rowHeight = 20f;
            showBorder = true;
            Reload();
            SelectionChanged(GetSelection());
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = -1, depth = -1, displayName = "Root" };

            var rows = new List<TreeViewItem>();

            var defaultChannelProperty = GetDefaultChannel();
            var channelsProperty = GetChannelsProperty();

            var index = 0;

            rows.Add(new SidebarItem(index, 0, defaultChannelProperty, null, SidebarItemType.Channel, null, defaultChannelProperty));
            index++;
            index = AddVariablesAndGameEventFromChannel(rows, defaultChannelProperty, index);

            var channelsNameCounter = new Dictionary<string, int>();

            for (int i = 0; i < channelsProperty.arraySize; i++)
            {
                var channelProperty = channelsProperty.GetArrayElementAtIndex(i);
                rows.Add(new SidebarItem(index, 0, channelProperty, channelsProperty, SidebarItemType.Channel, channelsNameCounter, channelProperty, i));
                index++;
                index = AddVariablesAndGameEventFromChannel(rows, channelProperty, index);
            }


            SetupParentsAndChildrenFromDepths(root, rows);

            UpdateErrors(root);

            return root;
        }


        private void UpdateErrors(TreeViewItem root)
        {
            if (root.hasChildren == false) return;

            foreach (SidebarItem item in root.children)
            {
                UpdateErrorsRecursive(item);
            }
        }

        private void UpdateErrorsRecursive(SidebarItem parent)
        {
            parent.Validated();
            if (parent.hasChildren == false) return;


            foreach (SidebarItem item in parent.children)
            {
                UpdateErrorsRecursive(item);
            }
        }


        private static int AddVariablesAndGameEventFromChannel(List<TreeViewItem> rows, SerializedProperty channelProperty, int index)
        {
            var variables = channelProperty.FindPropertyRelative(VariableDataBaseManagerWindow.variablePropertyName);
            var gameEvents = channelProperty.FindPropertyRelative(VariableDataBaseManagerWindow.gameEventsPropertyName);

            var variableNames = new Dictionary<string, int>();
            var gameEventNames = new Dictionary<string, int>();

            for (int x = 0; x < variables.arraySize; x++)
            {
                rows.Add(new SidebarItem(index, 1, variables.GetArrayElementAtIndex(x), variables, SidebarItemType.Variable, variableNames, channelProperty, x));
                index++;
            }

            for (int x = 0; x < gameEvents.arraySize; x++)
            {
                rows.Add(new SidebarItem(index, 1, gameEvents.GetArrayElementAtIndex(x), gameEvents, SidebarItemType.Event, gameEventNames, channelProperty, x));
                index++;
            }

            return index;
        }




        protected override void RenameEnded(RenameEndedArgs args)
        {
            var row = FindRows(new int[] { args.itemID }).FirstOrDefault();

            if (row != null && args.acceptedRename && string.IsNullOrEmpty(args.newName) == false)
            {
                row.displayName = args.newName;
            }
        }

        protected override bool CanRename(TreeViewItem item)
        {
            return base.CanRename(item);
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            var selectedItem = FindRows(selectedIds).FirstOrDefault() as SidebarItem;

            SelectedObject = selectedItem == null ? null : selectedItem.currentObject;
            SelectedProperty = selectedItem == null ? null : selectedItem.TargetProperty;
            CurrentChannelName = selectedItem == null || selectedItem.Type == SidebarItemType.Channel ? string.Empty : selectedItem.Channel.FindPropertyRelative("name").stringValue;
            SelectedName = selectedItem == null ? string.Empty : selectedItem.displayName;
        }
        #region ContextAction

        //protected override void ContextClickedItem(int id)
        //{
        //    var row = FindRows(new List<int>() { id }).FirstOrDefault() as SidebarItem;

        //    if (row != null)
        //    {
        //        AddChannelOptions(menu, row);
        //        AddItemOptions(menu, row);
        //        menu.ShowAsContext();
        //        Repaint();
        //    }
        //}

        private void ShowContextMenu(SidebarItem row)
        {
            if (row != null)
            {
                var menu = new GenericMenu();

                AddChannelOptions(menu, row);
                AddItemOptions(menu, row);
                menu.ShowAsContext();
                Repaint();
            }
        }

        private void AddItemOptions(GenericMenu menu, SidebarItem row)
        {
            if (row.Type == SidebarItemType.Event || row.Type == SidebarItemType.Variable)
            {
                var defaultChannel = serializedObject.FindProperty(VariableDataBaseManagerWindow.defaultChannelPropertyName);

                if (defaultChannel.displayName != row.Channel.displayName) menu.AddItem(new GUIContent("Move/Default"), false, () =>
                {
                    MoveContentToChannel(row.Array, GetCorrectArray(defaultChannel, row.Type), row.TargetProperty);
                    Reload();
                    SelectionChanged(GetSelection());
                });

                var channels = serializedObject.FindProperty(VariableDataBaseManagerWindow.channelsPropertyName);

                for (int i = 0; i < channels.arraySize; i++)
                {
                    var channelProperty = channels.GetArrayElementAtIndex(i);

                    if (channelProperty.displayName != row.Channel.displayName)
                    {
                        menu.AddItem(new GUIContent($"Move/{channelProperty.FindPropertyRelative("name").stringValue}"), false, () =>
                        {
                            MoveContentToChannel(row.Array, GetCorrectArray(channelProperty, row.Type), row.TargetProperty);
                            Reload();
                            SelectionChanged(GetSelection());
                        });
                    }
                }

                menu.AddItem(new GUIContent($"Delete"), false, () =>
                {
                    if (EditorUtility.DisplayDialog($"Delete Asset ({row.displayName})"
                                                       , $"Would you like to delete asset '{row.displayName}'\nYou cannot undo this action"
                                                       , "Delete"
                                                       , "Cancel"))
                    {
                        DeleteItem(row);
                        Reload();
                        SelectionChanged(GetSelection());
                    }
                });
            }
        }

        private void AddChannelOptions(GenericMenu menu, SidebarItem row)
        {
            bool isDefaultChannel = row.IndexInArray == -1;

            if (row.Type == SidebarItemType.Channel)
            {
                creator.UpdateComponentsTypesList<VariableBase>();
                creator.AddToMenu(menu, "Add Variable/", (x) =>
                {
                    var type = x as Type;
                    var property = AddItem(GetChannelVariables(row.TargetProperty), type, "Variables");
                    Reload();
                    EndRename();
                    BeginRename(FindItem(property));
                });
                creator.UpdateComponentsTypesList<GameEventBase>();
                creator.AddToMenu(menu, "Add Event/", (x) =>
                {
                    var type = x as Type;
                    var property = AddItem(GetChannelGameEvents(row.TargetProperty), type, "GameEvents");
                    Reload();
                    EndRename();
                    BeginRename(FindItem(property));
                });

                if (isDefaultChannel == false)
                {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent($"Delete/Delete With Content"), false, () =>
                    {
                        if (EditorUtility.DisplayDialog($"Delete Assets({row.displayName})"
                                                       ,$"Would you like to delete all asset included in '{row.Channel.FindPropertyRelative("name").stringValue}'\nYou cannot undo this action"
                                                       ,"Delete"
                                                       ,"Cancel")) 
                        {
                            DeleteChannel(row, null);
                            Reload();
                            SelectionChanged(GetSelection());
                        }
                    });
                    menu.AddItem(new GUIContent($"Delete/Move Content/Defualt"), false, () =>
                    {
                        DeleteChannel(row, GetDefaultChannel());
                        Reload();
                        SelectionChanged(GetSelection());
                    });

                    var channels = row.Array;

                    for (int i = 0; i < channels.arraySize; i++)
                    {
                        var channelProperty = channels.GetArrayElementAtIndex(i);

                        if (i != row.IndexInArray)
                        {
                            menu.AddItem(new GUIContent($"Delete/Move Content/{channelProperty.FindPropertyRelative("name").stringValue}"), false, () =>
                            {
                                DeleteChannel(row, channelProperty);
                                Reload();
                                SelectionChanged(GetSelection());
                            });
                        }
                    }
                }
            }

        }

        #endregion

        protected override void DoubleClickedItem(int id)
        {
            var item = FindRows(GetSelection()).FirstOrDefault();
            // Otherwise, perform a rename by default.
            if (item != null) 
            {
                BeginRename(item);
            }
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        #region GUI
        public override void OnGUI(Rect rect)
        {
            var headerRect = rect;
            headerRect.height = 25f;
            headerRect.width -= 1;
            rect.yMin += headerRect.height;
            DrawHeader(headerRect);
            base.OnGUI(rect);
        }

        private void DrawHeader(Rect rect)
        {
            if (searchField == null)
            {
                searchField = new SearchField();
            }

            GUI.Box(rect, GUIContent.none, GTGUIStyles.sideBarHeader);
            var searchFieldRect = rect;
            searchFieldRect.xMin += 10;
            searchFieldRect.y += (searchFieldRect.height - EditorGUIUtility.singleLineHeight) / 2f;
            searchFieldRect.xMax -= 25f;
            searchString = searchField.OnGUI(searchFieldRect, searchString);

            var addButtonRect = rect;
            addButtonRect.xMin += rect.width - 20;

            if(GUI.Button(addButtonRect, GTGUIStyles.PlusIcon,GTGUIStyles.plusButtonStyle))
            {
                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("Add Channel"), false, () => 
                {
                    var channelPropert = serializedObject.FindProperty(VariableDataBaseManagerWindow.channelsPropertyName);
                    serializedObject.Update();
                    channelPropert.InsertArrayElementAtIndex(channelPropert.arraySize);
                    var addedChannel = channelPropert.GetArrayElementAtIndex(channelPropert.arraySize - 1);
                    addedChannel.FindPropertyRelative("name").stringValue = "New Channel";
                    GetChannelVariables(addedChannel).ClearArray();
                    GetChannelGameEvents(addedChannel).ClearArray();
                    serializedObject.ApplyModifiedProperties();
                    Reload();
                    var item = FindItem(addedChannel, 1);
                    EndRename();
                    BeginRename(item);
                    FrameItem(item.id);
                });

                menu.ShowAsContext();
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var rect = args.rowRect;
            rect.yMin += 1.25f;
            rect.yMax -= 1.25f;

            rect.xMin += GetContentIndent(args.item);


            GUIContent content = new GUIContent(args.label, (args.item as SidebarItem).Error);

            rect.width -= 25f;

            EditorGUI.LabelField(rect, content, GTGUIStyles.row);

            var contextButtonRect = rect;
            contextButtonRect.xMin += rect.width;
            contextButtonRect.width = 25f;

            if(GUI.Button(contextButtonRect, GTGUIStyles.PlusIcon, GTGUIStyles.plusButtonStyle))
            {
                ShowContextMenu(args.item as SidebarItem);
            }

            var textureRect = rect;
            textureRect.width = 7.5f;

            var row = args.item as SidebarItem;

            GUI.DrawTexture(textureRect, GTGUIStyles.GetSideBarItemTexture(row.Type));
        }
        #endregion

        #region Action

        private void MoveContentToChannel(SerializedProperty from,SerializedProperty to,SerializedProperty target)
        {
            serializedObject.Update();

            GTGUIUtilites.MoveToArray(from, to, target);

            serializedObject.ApplyModifiedProperties();
        }

        private void MoveContentToChannel(SerializedProperty from,SerializedProperty to)
        {
            serializedObject.Update();

            GTGUIUtilites.AppendToArray(GetChannelVariables(from), GetChannelVariables(to));
            GTGUIUtilites.AppendToArray(GetChannelGameEvents(from), GetChannelGameEvents(to));

            serializedObject.ApplyModifiedProperties();

        }

        private SerializedProperty AddItem(SerializedProperty array, Type type, string folderName)
        {
            var newObject = ScriptableObject.CreateInstance(type);

            newObject.name = "New" + type.Name;

            string assetPath = GetAssetPath(folderName, newObject);

            AssetDatabase.CreateAsset(newObject, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            serializedObject.Update();
            array.InsertArrayElementAtIndex(array.arraySize);
            array.GetArrayElementAtIndex(array.arraySize - 1).objectReferenceValue = newObject;
            serializedObject.ApplyModifiedProperties();

            return array.GetArrayElementAtIndex(array.arraySize - 1);
        }

        private void DeleteItem(SidebarItem row)
        {
            switch (row.Type)
            {
                case SidebarItemType.Event:
                case SidebarItemType.Variable:
                    RemoveItem(row.Array, row.TargetProperty);
                    break;
            }
        }


        private void DeleteChannel(SidebarItem channel, SerializedProperty moveToChannel)
        {
            if (moveToChannel == null)
            {
                var variablesProperty = channel.TargetProperty.FindPropertyRelative(VariableDataBaseManagerWindow.variablePropertyName);
                var gameEventsProperty = channel.TargetProperty.FindPropertyRelative(VariableDataBaseManagerWindow.gameEventsPropertyName);

                for (int i = 0; i < variablesProperty.arraySize; i++)
                {
                    DeleteAsset(variablesProperty.GetArrayElementAtIndex(i).objectReferenceValue, false);
                }

                for (int i = 0; i < gameEventsProperty.arraySize; i++)
                {
                    DeleteAsset(gameEventsProperty.GetArrayElementAtIndex(i).objectReferenceValue, false);
                }

                serializedObject.Update();
                serializedObject.FindProperty(VariableDataBaseManagerWindow.channelsPropertyName).DeleteArrayElementAtIndex(channel.IndexInArray);
                serializedObject.ApplyModifiedProperties();


                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                MoveContentToChannel(channel.TargetProperty, moveToChannel);
                serializedObject.Update();
                channel.Array.DeleteArrayElementAtIndex(channel.IndexInArray);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void RemoveItem(SerializedProperty array, SerializedProperty element, bool deleteAsset = true, bool saveDatabase = true)
        {
            var objRef = element.objectReferenceValue;
            var index = GTGUIUtilites.FindIndexInProperty(element.objectReferenceValue, array);

            serializedObject.Update();
            array.DeleteArrayElementAtIndex(index);
            if (objRef != null) array.DeleteArrayElementAtIndex(index);
            serializedObject.ApplyModifiedPropertiesWithoutUndo();


            if (deleteAsset) DeleteAsset(objRef, saveDatabase);
        }

        private void DeleteAsset(Object objRef,bool saveDatabase = true)
        {
            var pathToAsset = AssetDatabase.GetAssetPath(objRef);

            AssetDatabase.DeleteAsset(pathToAsset);
            if (saveDatabase)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        #endregion

        #region Utilites

        private SidebarItem FindItem(SerializedProperty property,int maxDepth = 0)
        {
            if (rootItem.hasChildren == false) return null;

            maxDepth--;

            foreach (SidebarItem item in rootItem.children)
            {
                if (item.TargetProperty.propertyPath == property.propertyPath) return item;

                if (maxDepth != 0)
                {
                    var foundItem = FindItemRecursive(item, property, maxDepth);
                    if (foundItem != null) return foundItem;
                }
            }

            return null;
        }

        private SidebarItem FindItemRecursive(SidebarItem target,SerializedProperty property, int maxDepth = 0)
        {
            if (target.hasChildren == false) return null;

            maxDepth--;

            foreach (SidebarItem item in target.children)
            {
                if (item.TargetProperty.propertyPath == property.propertyPath) return item;
                if (maxDepth != 0)
                {
                    var foundItem = FindItemRecursive(item, property, maxDepth);
                    if (foundItem != null) return foundItem;
                }
            }

            return null;
        }

        private string GetAssetPath(string folderName, ScriptableObject newObject)
        {
            var assetPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);

            assetPath = assetPath.Substring(0, assetPath.LastIndexOf('/'));

            if (string.IsNullOrEmpty(folderName) == false && AssetDatabase.IsValidFolder($"{assetPath}/{folderName}") == false)
            {
                AssetDatabase.CreateFolder(assetPath, folderName);
            }

            assetPath += string.IsNullOrEmpty(folderName) ? $"/{newObject.name}.asset" : $"/{folderName}/{newObject.name}.asset";

            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
            return assetPath;
        }

        private SerializedProperty GetDefaultChannel()
        {
            return serializedObject.FindProperty(VariableDataBaseManagerWindow.defaultChannelPropertyName);
        }

        private SerializedProperty GetChannelsProperty()
        {
            return serializedObject.FindProperty(VariableDataBaseManagerWindow.channelsPropertyName);
        }

        private SerializedProperty GetChannelVariables(SerializedProperty channel) 
        {
            return channel.FindPropertyRelative(VariableDataBaseManagerWindow.variablePropertyName);
        }

        private SerializedProperty GetChannelGameEvents(SerializedProperty channel)
        {
            return channel.FindPropertyRelative(VariableDataBaseManagerWindow.gameEventsPropertyName);
        }

        private SerializedProperty GetCorrectArray(SerializedProperty channel,SidebarItemType type)
        {
            switch (type)
            {
                case SidebarItemType.Event:
                    return GetChannelGameEvents(channel);
                case SidebarItemType.Variable:
                    return GetChannelVariables(channel);
            }

            return null;
        }

        #endregion
    }
}
