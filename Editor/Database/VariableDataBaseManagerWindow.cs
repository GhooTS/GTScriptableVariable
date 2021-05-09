using UnityEditor;
using UnityEngine;
using GTVariable.Editor.Utility;
using System;
using System.Linq;

using Object = UnityEngine.Object;
using UnityEditor.IMGUI.Controls;

namespace GTVariable.Editor
{

    public class VariableDataBaseManagerWindow : UnityEditor.EditorWindow
    {
        private VariableDatabase database;
        private SerializedObject serializedDatabase;
        private string[] topBarTabs = new string[] { "Game Events", "Variables" };
        private string[] propertyNames = new string[] { "persistentGameEvents", "persistentVariables" };
        private Type[] elementTypes = new Type[] { typeof(GameEventBase), typeof(VariableBase) };
        private int selectedTopBarTab = 0;
        private UnityEditor.Editor cachedEditor;
        private SideBar sideBar = new SideBar()
        {
            CanDuplicated = false
        };


        

        [MenuItem("Tools/GT/Variable Database Manager")]
        public static void Init()
        {
            GetWindow<VariableDataBaseManagerWindow>("Variable Database Manager");
        }

        private void OnEnable()
        {
            position = WindowPrefs.LoadPosition(name);
            OnDatabaseChanaged();
            sideBar.OnElementDelete += OnElementDelete;
            Undo.undoRedoPerformed += Repaint;
        }

        private void OnDisable()
        {
            WindowPrefs.SavePosition(name, position);
            sideBar.OnElementDelete -= OnElementDelete;
            Undo.undoRedoPerformed -= Repaint;
        }


        private void OnDatabaseChanaged()
        {
            serializedDatabase = database == null ? null : new SerializedObject(database);
            if (database != null)
            {
                sideBar.Array = GetCurrentProperty();
                sideBar.ElementType = GetCurrentType();
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
                EditorGUI.BeginChangeCheck();
                selectedTopBarTab = EditorGUILayout.Popup(selectedTopBarTab, topBarTabs,EditorStyles.toolbarDropDown, GUILayout.MaxWidth(150f));
                if (EditorGUI.EndChangeCheck())
                {
                    OnDatabaseChanaged();
                }

                EditorGUILayout.LabelField(GUIContent.none);
                EditorGUI.BeginDisabledGroup(database == null);
                if (GUILayout.Button("Focus",EditorStyles.toolbarButton,GUILayout.MaxWidth(110)))
                {
                    EditorGUIUtility.PingObject(database);
                }
                if (GUILayout.Button("Delete", EditorStyles.toolbarButton, GUILayout.MaxWidth(110)))
                {
                    var path = AssetDatabase.GetAssetPath(database);
                    if (EditorUtility.DisplayDialog("Delete Selected Asset?", $"{path}\nYou cannot undo this action.", "Delete", "Cancel"))
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

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(columStyle, GUILayout.MaxWidth(300));
            sideBar.OnGUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(columStyle);
            DrawSelectedObjectEditor();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            HandleDropOperation();
        }

        private void DrawSelectedObjectEditor()
        {
            
            var currentSelected = sideBar.CurrentSelected;


            if (currentSelected != null)
            {
                
                EditorGUILayout.BeginHorizontal(GTGUIStyles.sideBarHeader);
                EditorGUILayout.LabelField(currentSelected.name, EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginVertical(GTGUIStyles.sideBarContent);
                EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
                if (cachedEditor == null)
                {
                    cachedEditor = UnityEditor.Editor.CreateEditor(currentSelected);
                }
                UnityEditor.Editor.CreateCachedEditor(currentSelected, null, ref cachedEditor);
                EditorGUI.BeginChangeCheck();
                Undo.RecordObject(currentSelected, "Object Chanage");
                currentSelected.name = EditorGUILayout.DelayedTextField("Name:", currentSelected.name);
                if (EditorGUI.EndChangeCheck())
                {
                    AssetDatabase.Refresh();
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(currentSelected));
                }

                cachedEditor.OnInspectorGUI();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();
            }
        }

        private SerializedProperty GetCurrentProperty()
        {
            return serializedDatabase.FindProperty(propertyNames[selectedTopBarTab]);
        }

        private Type GetCurrentType()
        {
            return elementTypes[selectedTopBarTab];
        }

        private void OnElementDelete(Object element)
        {
            if (element != null)
            {
                AssetDatabase.RemoveObjectFromAsset(element);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }


        protected void HandleDropOperation()
        {
            if (database == null) return;

            var dropArea = position;
            dropArea.position = Vector2.zero;
            // Cache References:
            Event currentEvent = Event.current;
            EventType currentEventType = currentEvent.type;

            // The DragExited event does not have the same mouse position data as the other events,
            // so it must be checked now:
            if (currentEventType == EventType.DragExited) DragAndDrop.PrepareStartDrag();// Clear generic data when user pressed escape. (Unfortunately, DragExited is also called when the mouse leaves the drag area)

            if (!dropArea.Contains(currentEvent.mousePosition)) return;

            switch (currentEventType)
            {
                case EventType.DragUpdated:
                    if (dropArea.Contains(currentEvent.mousePosition))
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                        foreach (var objRef in DragAndDrop.objectReferences)
                        {
                            if (ValidType(objRef.GetType()) == false)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                            }
                        }
                        
                    }
                    break;
                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    if (EditorUtility.DisplayDialog("Drop Down Operation", "This action will move assets under this database", "Confirm", "Cancel"))
                    {
                        AddDropElementToDatabase();
                    }

                    currentEvent.Use();
                    break;
                case EventType.MouseUp:
                    // Clean up, in case MouseDrag never occurred:
                    DragAndDrop.PrepareStartDrag();
                    break;
            }
        }

        private void AddDropElementToDatabase()
        {
            serializedDatabase.Update();
            foreach (var objRef in DragAndDrop.objectReferences)
            {
                var index = GetTargetPropertyIndex(objRef.GetType());

                if (index == -1) continue;

                var array = serializedDatabase.FindProperty(propertyNames[index]);

                if (GTGUIUtilites.FindIndexInProperty(objRef, array) != -1) continue;

                var elementIndex = array.arraySize;
                array.InsertArrayElementAtIndex(array.arraySize);
                array.GetArrayElementAtIndex(elementIndex).objectReferenceValue = objRef;
                var path = AssetDatabase.GetAssetPath(objRef);

                var mainAsset = AssetDatabase.LoadMainAssetAtPath(path);


                if (mainAsset != database && mainAsset is VariableDatabase)
                {
                    var otherDatabase = new SerializedObject(mainAsset);
                    otherDatabase.Update();
                    var otherArray = otherDatabase.FindProperty(propertyNames[index]);
                    var oldIndex = GTGUIUtilites.FindIndexInProperty(objRef, otherArray);
                    otherArray.DeleteArrayElementAtIndex(oldIndex);
                    otherArray.DeleteArrayElementAtIndex(oldIndex);
                    otherDatabase.ApplyModifiedProperties();
                }

                AssetDatabase.RemoveObjectFromAsset(objRef);

                if (AssetDatabase.LoadMainAssetAtPath(path) == null) AssetDatabase.DeleteAsset(path);



                AssetDatabase.AddObjectToAsset(objRef, database);
            }
            serializedDatabase.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private bool ValidType(Type dropElementType)
        {
            foreach (var type in elementTypes)
            {
                if (type.IsAssignableFrom(dropElementType))
                {
                    return true;
                }
            }

            return false;
        }

        private int GetTargetPropertyIndex(Type dropElementType)
        {
            return ArrayUtility.FindIndex(elementTypes, type => type.IsAssignableFrom(dropElementType));
        }
    }
}