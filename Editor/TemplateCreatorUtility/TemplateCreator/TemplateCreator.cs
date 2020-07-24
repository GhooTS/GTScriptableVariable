using UnityEditor;
using UnityEngine;

namespace GTVariable.Editor.Utility
{
    public abstract class TemplateCreator
    {
        protected TemplateCreatorItem[] items;
        public int Length { get { return items.Length; } }
        public bool AnyExist { get; private set; }
        public bool AllExist { get; private set; }

        public string CreatorName { get; protected set; }
        public string PreviewTemplateContent { get; protected set; } = "";
        protected Vector2 previewScrollVector;

        public abstract void Init(string templatePath, string runtimeFolder, string editorFolder);

        protected void SetSize(int size)
        {
            items = new TemplateCreatorItem[size];
        }


        public virtual void DrawCreator(string rootFolderPath, string rootFolderRelativePath, TextPattern[] textPatterns)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create"))
            {
                CraeteAll(rootFolderPath, rootFolderRelativePath, textPatterns);
            }

            if (GUILayout.Button("Delete"))
            {
                DeleteAll(rootFolderRelativePath);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            for (int i = 0; i < items.Length; i++)
            {
                DrawModifyOption(i, rootFolderRelativePath, textPatterns);
            }
            EditorGUILayout.EndVertical();
            previewScrollVector = EditorGUILayout.BeginScrollView(previewScrollVector);
            var enabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.TextArea(PreviewTemplateContent);
            GUI.enabled = enabled;
            EditorGUILayout.EndScrollView();
        }


        protected void DrawModifyOption(int index, string rootFolderRelativePath, TextPattern[] textPatterns)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(items[index].FileExist ? "Delete" : "Create", GUILayout.MaxWidth(60));
            var dependeceIndex = items[index].DependenceIndex;
            var enabled = GUI.enabled;
            var toggleEnabled = items[index].Dependence == false || items[dependeceIndex].Modify;
            GUI.enabled = toggleEnabled; //Disable toggle if

            var modify = items[index].Dependence && items[dependeceIndex].FileExist && items[index].FileExist //Check if both this and class it's dependes on exist 
                        || items[index].Required // Always check if this class is required
                        || items[index].Modify // Check if user set modify to true
                        || items[index].FileExist; // Check if file exist

            //Set modify to false if the file its dependce on doesn't exist and is uncheck
            if (modify && items[index].Dependence && items[dependeceIndex].FileExist == false && items[dependeceIndex].Modify == false)
            {
                modify = false;
            }

            items[index].Modify = EditorGUILayout.ToggleLeft(items[index].Template.fileName, modify);
            GUI.enabled = enabled;

            //Update preview content
            if (GUILayout.Button("Preview", GUILayout.MaxWidth(70), GUILayout.MaxWidth(70)))
            {
                PreviewTemplateContent = FileFromTemplate.GetFileContentPreview(items[index].Template.templatePath, textPatterns);
            }


            //Display option to remove file, for unrequired class files
            if (items[index].Required == false && items[index].FileExist && GUILayout.Button("Delete", GUILayout.MaxWidth(70), GUILayout.MaxWidth(70)))
            {
                DeleteWithDependecies(rootFolderRelativePath, index);
            }

            EditorGUILayout.EndHorizontal();
        }


        public void Create(string rootFolderPath, string rootFolderRelativePath, TextPattern[] textPatterns, int from, int count)
        {
            for (int i = from; i < from + count; i++)
            {
                var item = items[i];
                if (item.Modify == false || item.FileExist) continue;

                FileFromTemplate.Create(item.Template, rootFolderPath, textPatterns);
                AssetDatabase.ImportAsset($"{rootFolderRelativePath}/{item.Template.GetPathToFile()}");
            }
        }



        public void CraeteAll(string rootFolderPath, string rootFolderRelativePath, TextPattern[] textPatterns)
        {
            Create(rootFolderPath, rootFolderRelativePath, textPatterns, 0, Length);
        }

        public void Delete(string rootFolderRelativePath, int from, int count)
        {
            if (AnyExist == false) return;

            for (int i = from; i < from + count; i++)
            {
                Delete(rootFolderRelativePath, items[i]);
            }

            AssetDatabase.Refresh();
        }

        public void Delete(string rootFolderRelativePath, TemplateCreatorItem item)
        {
            if (item.Modify && item.FileExist)
            {
                AssetDatabase.DeleteAsset($"{rootFolderRelativePath}/{item.Template.GetPathToFile()}");
            }
        }

        public void DeleteWithDependecies(string rootFolderRelativePath, int index)
        {
            Delete(rootFolderRelativePath, items[index]);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Dependence && items[i].DependenceIndex == index && items[i].FileExist)
                {
                    DeleteWithDependecies(rootFolderRelativePath, i);
                }
            }
        }

        public void DeleteAll(string rootFolderRelativePath)
        {
            Delete(rootFolderRelativePath, 0, Length);
        }


        public void UpdateExist(string rootFolder)
        {
            AnyExist = false;
            AllExist = true;
            for (int i = 0; i < items.Length; i++)
            {
                items[i].FileExist = System.IO.File.Exists($"{rootFolder}/{items[i].Template.GetPathToFile()}");

                if (items[i].FileExist) AnyExist = true;
                if (items[i].FileExist == false) AllExist = false;
            }
        }

        public string GetFileName(int index)
        {
            return items[index].Template.fileName;
        }

        public bool Exist(int index)
        {
            return items[index].FileExist;
        }

        public void UpdateNames(string name, bool useTemplateNameAsAffix = true)
        {
            foreach (var item in items)
            {
                item.Template.SetName(name, useTemplateNameAsAffix);
            }
        }

    }
}