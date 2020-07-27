using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor.Utility
{
    public class GameEventCreator : TemplateCreator
    {

        public override void Init(string templatePath, string runtimeFolder, string editorFolder)
        {
            SetSize(5);
            CreatorName = "Game Event Creator";
            //UnityEvent template
            items[0] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "Event", $"{runtimeFolder}/UnityEvent", "", "cs"),
                Required = true
            };

            //GameEvent templates
            var gameEventFolder = "GameEvent";
            items[1] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "GameEvent", $"{runtimeFolder}/{gameEventFolder}", "", "cs"),
                Required = true
            };
            items[2] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "GameEventEditor", $"{editorFolder}/{gameEventFolder}", "", "cs"),
                DependenceIndex = 1,
                Dependence = true
            };

            //Listener templates
            var listenerFolder = "Listener";
            items[3] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "Listener", $"{runtimeFolder}/{listenerFolder}", "", "cs"),
                Required = true
            };
            items[4] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "ListenerEditor", $"{editorFolder}/{listenerFolder}", "", "cs"),
                DependenceIndex = 3,
                Dependence = true
            };

            UpdateNames("");
        }


    }
}