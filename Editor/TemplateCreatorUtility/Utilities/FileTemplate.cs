using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor.Utility
{
    public class FileTemplate
    {
        public string templatePath;
        public string templateName;
        public string fileName;
        public string relativeFolderPath;
        public string fileFormat;

        public FileTemplate(string templatePath, string templateName, string relativeFolderPath, string fileName, string fileFormat)
        {
            this.templateName = templateName;
            this.templatePath = $"{templatePath}/{templateName}.template";
            this.fileName = fileName;
            this.relativeFolderPath = relativeFolderPath;
            this.fileFormat = fileFormat;
        }

        /// <summary>
        /// Sets file name
        /// </summary>
        /// <param name="name">New name for file</param>
        /// <param name="addTemplateName">Add template name to file name as the affix</param>
        public void SetName(string name, bool addTemplateName)
        {
            fileName = addTemplateName ? $"{name}{templateName}" : name;
        }

        /// <returns>return relative path to the file with file format</returns>
        public string GetPathToFile()
        {
            return $"{relativeFolderPath}/{fileName}.{fileFormat}";
        }

        /// <returns>return file name with file format</returns>
        public string GetFileNameWithFormat()
        {
            return $"{fileName}.{fileFormat}";
        }

    }
}