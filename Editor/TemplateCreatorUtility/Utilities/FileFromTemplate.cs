using System.IO;
using System.Text;

namespace GTVariable.Editor.Utility
{
    public static class FileFromTemplate
    {
        /// <summary>
        /// Create file from template and repace text in the file base on the pattern
        /// </summary>
        /// <param name="templatePath">Absolute path to the template</param>
        /// <param name="folderPath">Absolute path to a folder in which file will be created</param>
        /// <param name="fileName">Name of the file with format</param>
        /// <param name="textPatterns">patterns that will be use to create file content from template content</param>
        public static void Create(string templatePath, string folderPath, string fileName, TextPattern[] textPatterns)
        {
            if (File.Exists($"{folderPath}/{fileName}")) return; // retun if file already exist

            //Create folder if not exist
            if (Directory.Exists(folderPath) == false) Directory.CreateDirectory(folderPath);


            using (StreamWriter sWriter = new StreamWriter($"{folderPath}/{fileName}"))
            {
                using (StreamReader sReader = new StreamReader(templatePath))
                {
                    StringBuilder content = new StringBuilder(sReader.ReadToEnd());
                    foreach (var replaceText in textPatterns)
                    {
                        content = content.Replace(replaceText.pattern, replaceText.replacement);
                    }

                    sWriter.Write(content);

                }
            }
        }

        /// <summary>
        /// Create file from template and repace text in the file base on the pattern
        /// </summary>
        /// <param name="rootFolderPath">Absolute path to a folder in which file will be created</param>
        /// <param name="textPatterns">patterns that will be use to create file content from template content</param>
        public static void Create(FileTemplate template, string rootFolderPath, TextPattern[] textPatterns)
        {
            var pathToFile = $"{rootFolderPath}/{template.GetPathToFile()}";
            if (File.Exists(pathToFile)) return; // retun if file already exist

            var pathToFolder = $"{rootFolderPath}/{template.relativeFolderPath}";
            //Create folder if not exist
            if (Directory.Exists(pathToFolder) == false) Directory.CreateDirectory(pathToFolder);


            using (StreamWriter sWriter = new StreamWriter(pathToFile))
            {
                using (StreamReader sReader = new StreamReader(template.templatePath))
                {
                    StringBuilder content = new StringBuilder(sReader.ReadToEnd());
                    foreach (var textPattern in textPatterns)
                    {
                        content = content.Replace(textPattern.pattern, textPattern.replacement);
                    }

                    sWriter.Write(content);

                }
            }
        }

        /// <summary>
        /// Return template content
        /// </summary>
        /// <param name="templatePath">Absolute path to template</param>
        public static string GetTemplateContent(string templatePath)
        {
            using (StreamReader sReader = new StreamReader(templatePath))
            {
                return sReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Return content preview for file
        /// </summary>
        /// <param name="templatePath">Absolute path to the template</param>
        /// <param name="textPatterns"></param>
        public static string GetFileContentPreview(string templatePath, TextPattern[] textPatterns)
        {
            using (StreamReader sReader = new StreamReader(templatePath))
            {
                StringBuilder builder = new StringBuilder(sReader.ReadToEnd());
                foreach (var textPattern in textPatterns)
                {
                    builder.Replace(textPattern.pattern, textPattern.replacement);
                }

                return builder.ToString();
            }
        }
    }
}