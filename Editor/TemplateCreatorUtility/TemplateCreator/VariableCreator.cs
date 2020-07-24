using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor.Utility
{
    public class VariableCreator : TemplateCreator
    {


        public override void Init(string templatePath, string runtimeFolder, string editorFolder)
        {
            CreatorName = "Variable Creator";
            SetSize(5);

            //Variable templates
            var varsFolder = "Vars";
            items[0] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "Variable", $"{runtimeFolder}/{varsFolder}", "", "cs"),
                Required = true
            };
            items[1] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "VariablePropertyDrawer", $"{editorFolder}/{varsFolder}", "", "cs"),
                DependenceIndex = 0,
                Dependence = true
            };

            items[2] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "EventVariable", $"{editorFolder}/{varsFolder}", "", "cs"),
                DependenceIndex = 0,
                Dependence = true
            };

            //Reference templates
            var referenceFolder = "References";
            items[3] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "ReferenceVariable", $"{runtimeFolder}/{referenceFolder}", "", "cs")

            };

            items[4] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "ReferenceVariablePropertyDrawer", $"{editorFolder}/{referenceFolder}", "", "cs"),
                DependenceIndex = 3,
                Dependence = true
            };
            UpdateNames("");
        }

    }
}