using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor.Utility
{
    public class TemplateCreatorWindow : EditorWindow
    {
        private const string varsRootFolderKey = "GTVars_customVarsRootFolder";

        //Paths to root folder
        private string rootFolderPath = "";
        private string rootFolderRelativePath;

        private string[] selectionGridOptions;
        private int selected = 0;

        private readonly TextPattern[] textPatterns = new TextPattern[]
        {
        new TextPattern("*className*",""),
        new TextPattern("*typeName*",""),
        new TextPattern("*nameSpace*","")
        };
        private string ClassName
        {
            get { return textPatterns[0].replacement; }
            set { textPatterns[0].replacement = value; }
        }
        private string TypeName
        {
            get { return textPatterns[1].replacement; }
            set { textPatterns[1].replacement = value; }
        }
        private string Namespace
        {
            get { return textPatterns[2].replacement; }
            set { textPatterns[2].replacement = value; }
        }

        private readonly TemplateCreator[] creators = new TemplateCreator[]
        {
        new GameEventCreator(),
        new VariableCreator()
        };

        [MenuItem("Window/ScriptableVars/Templates Creator")]
        public static void Init()
        {
            var window = GetWindow<TemplateCreatorWindow>("Templates Creator");
            WindowPrefs.SavePosition(window.titleContent.text, window.position);
        }

        private void OnEnable()
        {
            //Load window position 
            position = WindowPrefs.LoadPosition(titleContent.text);
            LoadRootFolderPath();
            InitCreators();

            UpdateRootRelativePath();
        }



        private void OnDisable()
        {
            WindowPrefs.SavePosition(titleContent.text, position);
        }


        private void OnGUI()
        {
            DrawOptions();
            EditorGUI.BeginChangeCheck();
            selected = GUILayout.SelectionGrid(selected, selectionGridOptions, 2);
            if (EditorGUI.EndChangeCheck())
            {
                creators[selected].UpdateNames(ClassName);
                creators[selected].UpdateExist(rootFolderPath);
            }
            creators[selected].DrawCreator(rootFolderPath, rootFolderRelativePath, textPatterns);
        }

        private void DrawOptions()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select", GUILayout.MaxWidth(80)))
            {
                rootFolderPath = EditorUtility.OpenFolderPanel("Root folder location", rootFolderPath, Application.dataPath);
                UpdateRootRelativePath();
                EditorPrefs.SetString(varsRootFolderKey, rootFolderPath);
            }
            EditorGUILayout.LabelField(rootFolderRelativePath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            ClassName = EditorGUILayout.TextField("Class Name", ClassName);
            if (EditorGUI.EndChangeCheck())
            {
                TypeName = ClassName;
                creators[selected].UpdateNames(ClassName);
                creators[selected].UpdateExist(rootFolderPath);
            }
            TypeName = EditorGUILayout.TextField("Type Name", TypeName);
            EditorGUILayout.EndHorizontal();
            Namespace = EditorGUILayout.TextField(new GUIContent("Namespace", "example 'using yourNameSpace;'"), Namespace);
        }

        private void UpdateRootRelativePath()
        {
            int startIndex;
            if (string.IsNullOrEmpty(rootFolderPath) == false && (startIndex = rootFolderPath.LastIndexOf("/Assets/")) != -1)
            {
                rootFolderRelativePath = rootFolderPath.Substring(startIndex + 1);
            }
        }

        private string GetTemplatePath()
        {
            var mainFolder = AssetDatabase.FindAssets("GTScriptableVariable");
            for (int i = 0; i < mainFolder.Length; i++)
            {
                mainFolder[i] = AssetDatabase.GUIDToAssetPath(mainFolder[i]);
            }
            var templatePath = AssetDatabase.FindAssets("Templates", mainFolder)[0];
            templatePath = AssetDatabase.GUIDToAssetPath(templatePath);
            return templatePath;
        }

        private void LoadRootFolderPath()
        {
            if (EditorPrefs.HasKey(varsRootFolderKey))
            {
                rootFolderPath = EditorPrefs.GetString(varsRootFolderKey);
            }
            else
            {
                rootFolderPath = $"{Application.dataPath}/ScriptableVariableCustom";
            }
        }

        private void InitCreators()
        {
            selectionGridOptions = new string[creators.Length];

            string templatePath = GetTemplatePath();
            foreach (var creator in creators)
            {
                creator.Init(templatePath, "Runtime", "Editor");
                creator.UpdateExist(rootFolderPath);
                creator.UpdateNames(ClassName);
            }

            for (int i = 0; i < selectionGridOptions.Length; i++)
            {
                selectionGridOptions[i] = creators[i].CreatorName;
            }

        }
    }
}