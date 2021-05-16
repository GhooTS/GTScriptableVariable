using UnityEditor;
using UnityEngine;
using GTVariable.Editor.Utility;
using System;
using System.Linq;

using Object = UnityEngine.Object;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;

namespace GTVariable.Editor
{

    public class VariableDataBaseManagerWindow : UnityEditor.EditorWindow
    {
        private bool saveToTarget = false;
        private VariableDatabase database;
        private SerializedObject serializedDatabase;
        private UnityEditor.Editor cachedEditor;
        private SideBarTreeView treeView;
        [SerializeField] private TreeViewState state;
        [SerializeField] private float sideBarSize = 0.3f;

        private Vector2 scroll;


        public static readonly string defaultChannelPropertyName = "defaultChannel";
        public static readonly string channelsPropertyName = "channels";
        public static readonly string variablePropertyName = "variables";
        public static readonly string gameEventsPropertyName = "gameEvents";





        

        [MenuItem("Tools/GT/Variable Database Manager")]
        public static void Init()
        {
            GetWindow<VariableDataBaseManagerWindow>("Variable Database Manager");
        }

        private void OnEnable()
        {
            position = WindowPrefs.LoadPosition(name);
            var databaseGUID = EditorPrefs.GetString($"{name}_DatabaseGUID");
            database = AssetDatabase.LoadAssetAtPath<VariableDatabase>(AssetDatabase.GUIDToAssetPath(databaseGUID));
            state = new TreeViewState();
            EditorJsonUtility.FromJsonOverwrite(EditorPrefs.GetString($"{name}_treeState"), state);
            OnDatabaseChanaged();
            Undo.undoRedoPerformed += OnUndo;
        }

        private void OnDisable()
        {
            WindowPrefs.SavePosition(name, position);
            EditorPrefs.SetString($"{name}_DatabaseGUID", AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(database)));
            EditorPrefs.SetString($"{name}_treeState", EditorJsonUtility.ToJson(state)); 
            Undo.undoRedoPerformed -= OnUndo;
        }


        private void OnUndo()
        {
            serializedDatabase.Update();
            Repaint();
            treeView.Reload();
        }

        private void OnDatabaseChanaged()
        {
            serializedDatabase = database == null ? null : new SerializedObject(database);
            if (database != null)
            {
                if (state == null)
                {
                    state = new TreeViewState();
                }
                treeView = new SideBarTreeView(serializedDatabase, state);
            }
        }



        private void OnGUI()
        {
            using(var horizontal = new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                EditorGUI.BeginChangeCheck();
                if (GUILayout.Button(database == null ? "[Selecte Database]" : database.name, EditorStyles.toolbarDropDown,GUILayout.MaxWidth(150f)))
                {
                    var menu = new GenericMenu();

                    var GUIDs = AssetDatabase.FindAssets($"t:{typeof(VariableDatabase)}");

                    foreach (var guid in GUIDs)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        var tmp = AssetDatabase.LoadAssetAtPath<VariableDatabase>(path);
                        if (tmp != null)
                        {
                            menu.AddItem(new GUIContent(tmp.name), false, () => { database = tmp; OnDatabaseChanaged(); });
                        }
                    }

                    if(GUIDs.Length != 0) menu.AddSeparator("");

                    menu.AddItem(new GUIContent("Create Database"), false, () => 
                    {
                        var path = EditorUtility.SaveFilePanel("Create Variable Database", Application.dataPath, "VariableDatabase", "asset");

                        if (string.IsNullOrEmpty(path)) return;

                        var relativePath = "Assets" + path.Substring(Application.dataPath.Length);

                        var instance = CreateInstance<VariableDatabase>();
                        AssetDatabase.CreateAsset(instance, relativePath);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        database = instance;
                        OnDatabaseChanaged();
                    });
                    menu.ShowAsContext();
                }

                EditorGUILayout.LabelField(GUIContent.none);
                EditorGUI.BeginDisabledGroup(database == null);
                if(GUILayout.Button("Add Not Included Asset", EditorStyles.toolbarButton, GUILayout.MaxWidth(160)))
                {
                    AddNotIncludedAssets();
                }
                if (GUILayout.Button("Focus",EditorStyles.toolbarButton,GUILayout.MaxWidth(110)))
                {
                    EditorGUIUtility.PingObject(database);
                }
                if (GUILayout.Button("Delete", EditorStyles.toolbarButton, GUILayout.MaxWidth(110)))
                {
                    var path = AssetDatabase.GetAssetPath(database);
                    if (EditorUtility.DisplayDialog("Delete Selected Database?", $"{path}\nYou cannot undo this action.", "Delete", "Cancel"))
                    {
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        database = null;
                        OnDatabaseChanaged();
                    }
                }
                EditorGUI.EndDisabledGroup();
            }


            if (database == null)
            {
                EditorGUILayout.HelpBox("Select or create database", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space();

            var columStyle = new GUIStyle()
            {
                margin = new RectOffset(5, 5, 3, 3)
            };

            
            if (treeView != null)
            {
                treeView.OnGUI(GetSideBarRect());
            }
            DrawSelectedObjectEditor();
        }

        private readonly float topBarSpacing = 45f;
        private readonly float sideBarMinWidght = 250f;

        private Rect GetSideBarRect()
        {
            var sideBarRect = GUIUtility.ScreenToGUIRect(position);
            sideBarRect.width = Mathf.Max(sideBarMinWidght, sideBarRect.width * sideBarSize);
            sideBarRect.yMin += topBarSpacing;
            sideBarRect.xMin += 10;
            return sideBarRect;
        }
        private Rect GetContentRect()
        {
            var contentRect = GUIUtility.ScreenToGUIRect(position);
            contentRect.x += Mathf.Max(sideBarMinWidght, contentRect.width * sideBarSize);
            contentRect.width = Mathf.Max(0, Mathf.Min(contentRect.width - sideBarMinWidght, contentRect.width * (1f - sideBarSize)));
            contentRect.yMin += topBarSpacing;
            contentRect.xMax -= 10;
            contentRect.xMin += 2.5f;
            contentRect.width = Mathf.Max(0, contentRect.width);
            return contentRect;
        }


        private void DrawSelectedObjectEditor()
        {
            if (serializedDatabase.UpdateIfRequiredOrScript())
            {
                treeView.Reload();
            }

            Object currentSelected = treeView.SelectedObject;
            SerializedProperty currentProperty = treeView.SelectedProperty;


            if (currentSelected != null || currentProperty != null)
            {
                var contentRect = GetContentRect();
                var headerRect = contentRect;
                headerRect.height = 25f;
                GUILayout.BeginArea(headerRect, GTGUIStyles.sideBarHeader);

                var labelText = string.IsNullOrEmpty(treeView.CurrentChannelName) ? "" : $"<color=grey>[{treeView.CurrentChannelName}]</color> ";
                labelText += treeView.SelectedName;

                EditorGUILayout.LabelField(labelText, GTGUIStyles.labelCenter);
                GUILayout.EndArea();

                contentRect.yMin += headerRect.height;

                GUILayout.BeginArea(contentRect,GTGUIStyles.sideBarContent);
                scroll = EditorGUILayout.BeginScrollView(scroll);
                if (currentSelected != null)
                {
                    if (cachedEditor == null)
                    {
                        cachedEditor = UnityEditor.Editor.CreateEditor(currentSelected);
                    }
                    UnityEditor.Editor.CreateCachedEditor(currentSelected, null, ref cachedEditor);
                    cachedEditor.OnInspectorGUI();
                }
                else
                {
                    EditorGUILayout.PropertyField(currentProperty);
                }
                EditorGUILayout.EndScrollView();
                GUILayout.EndArea();
            }
        }


        private void AddNotIncludedAssets()
        {
            var path = AssetDatabase.GetAssetPath(database);
            path = path.Substring(0, path.LastIndexOf('/'));

            var GUIDs = AssetDatabase.FindAssets($"t:{nameof(VariableBase)} t:{nameof(GameEventBase)}");

            var index = 0;

            var defaultChannelEvents = serializedDatabase.FindProperty(defaultChannelPropertyName).FindPropertyRelative(gameEventsPropertyName);
            var defaultChannelVariables = serializedDatabase.FindProperty(defaultChannelPropertyName).FindPropertyRelative(variablePropertyName);

            int addedEvents = 0;
            int addedVaraibles = 0;

            var channelsVariables = new List<SerializedProperty>();
            var channelsGameEvents = new List<SerializedProperty>();

            channelsVariables.Add(defaultChannelVariables);
            channelsGameEvents.Add(defaultChannelEvents);

            var channels = serializedDatabase.FindProperty(channelsPropertyName);

            for (int i = 0; i < channels.arraySize; i++)
            {
                channelsVariables.Add(channels.GetArrayElementAtIndex(i).FindPropertyRelative(variablePropertyName));
                channelsGameEvents.Add(channels.GetArrayElementAtIndex(i).FindPropertyRelative(gameEventsPropertyName));
            }

            serializedDatabase.Update();

            foreach (var GUID in GUIDs)
            {
                index++;
                EditorUtility.DisplayProgressBar("Searching Assets", $"Checking asset {index} from {GUIDs.Length}", index / GUIDs.Length);

                var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(GUID));

                if (asset is GameEventBase)
                {
                    if (GTGUIUtilites.Containe(asset,channelsGameEvents) == false)
                    {
                        defaultChannelEvents.InsertArrayElementAtIndex(defaultChannelEvents.arraySize);
                        defaultChannelEvents.GetArrayElementAtIndex(defaultChannelEvents.arraySize - 1).objectReferenceValue = asset;
                        addedEvents++;
                    }
                }
                else if(asset is VariableBase)
                {
                    if (GTGUIUtilites.Containe(asset, channelsVariables) == false)
                    {
                        defaultChannelVariables.InsertArrayElementAtIndex(defaultChannelVariables.arraySize);
                        defaultChannelVariables.GetArrayElementAtIndex(defaultChannelVariables.arraySize - 1).objectReferenceValue = asset;
                        addedVaraibles++;
                    }
                }
            }

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Add Not Include Assets Operation", $"Added {addedEvents} Game Events and {addedVaraibles} Variables to Default Channel", "Ok");

            serializedDatabase.ApplyModifiedProperties();

            if(addedEvents + addedVaraibles != 0)
            {
                treeView.Reload();
            }
        }


        //protected void HandleDropOperation()
        //{
        //    if (database == null) return;

        //    var dropArea = position;
        //    dropArea.position = Vector2.zero;
        //    // Cache References:
        //    Event currentEvent = Event.current;
        //    EventType currentEventType = currentEvent.type;

        //    // The DragExited event does not have the same mouse position data as the other events,
        //    // so it must be checked now:
        //    if (currentEventType == EventType.DragExited) DragAndDrop.PrepareStartDrag();// Clear generic data when user pressed escape. (Unfortunately, DragExited is also called when the mouse leaves the drag area)

        //    if (!dropArea.Contains(currentEvent.mousePosition)) return;

        //    switch (currentEventType)
        //    {
        //        case EventType.DragUpdated:
        //            if (dropArea.Contains(currentEvent.mousePosition))
        //            {
        //                DragAndDrop.visualMode = DragAndDropVisualMode.Move;
        //                foreach (var objRef in DragAndDrop.objectReferences)
        //                {
        //                    if (ValidType(objRef.GetType()) == false)
        //                    {
        //                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
        //                    }
        //                }

        //            }
        //            break;
        //        case EventType.DragPerform:
        //            DragAndDrop.AcceptDrag();
        //            if (EditorUtility.DisplayDialog("Drop Down Operation", "This action will move assets under this database", "Confirm", "Cancel"))
        //            {
        //                AddDropElementToDatabase();
        //            }

        //            currentEvent.Use();
        //            break;
        //        case EventType.MouseUp:
        //            // Clean up, in case MouseDrag never occurred:
        //            DragAndDrop.PrepareStartDrag();
        //            break;
        //    }
        //}

        //private void AddDropElementToDatabase()
        //{
        //    serializedDatabase.Update();
        //    foreach (var objRef in DragAndDrop.objectReferences)
        //    {
        //        var index = GetTargetPropertyIndex(objRef.GetType());

        //        if (index == -1) continue;

        //        var array = serializedDatabase.FindProperty(propertyNames[index]);

        //        if (GTGUIUtilites.FindIndexInProperty(objRef, array) != -1) continue;

        //        var elementIndex = array.arraySize;
        //        array.InsertArrayElementAtIndex(array.arraySize);
        //        array.GetArrayElementAtIndex(elementIndex).objectReferenceValue = objRef;
        //        var path = AssetDatabase.GetAssetPath(objRef);

        //        var mainAsset = AssetDatabase.LoadMainAssetAtPath(path);


        //        if (mainAsset != database && mainAsset is VariableDatabase)
        //        {
        //            var otherDatabase = new SerializedObject(mainAsset);
        //            otherDatabase.Update();
        //            var otherArray = otherDatabase.FindProperty(propertyNames[index]);
        //            var oldIndex = GTGUIUtilites.FindIndexInProperty(objRef, otherArray);
        //            otherArray.DeleteArrayElementAtIndex(oldIndex);
        //            otherArray.DeleteArrayElementAtIndex(oldIndex);
        //            otherDatabase.ApplyModifiedProperties();
        //        }

        //        AssetDatabase.RemoveObjectFromAsset(objRef);

        //        if (AssetDatabase.LoadMainAssetAtPath(path) == null) AssetDatabase.DeleteAsset(path);

        //        AssetDatabase.AddObjectToAsset(objRef, database);
        //    }

        //    serializedDatabase.ApplyModifiedProperties();
        //    AssetDatabase.SaveAssets();
        //    AssetDatabase.Refresh();
        //}

        //private bool ValidType(Type dropElementType)
        //{
        //    foreach (var type in elementTypes)
        //    {
        //        if (type.IsAssignableFrom(dropElementType))
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //private int GetTargetPropertyIndex(Type dropElementType)
        //{
        //    return ArrayUtility.FindIndex(elementTypes, type => type.IsAssignableFrom(dropElementType));
        //}
    }
}