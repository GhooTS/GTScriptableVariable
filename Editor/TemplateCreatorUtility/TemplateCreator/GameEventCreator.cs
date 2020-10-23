using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor.Utility
{
    public class GameEventCreator : TemplateCreator
    {

        public override void Init(string templatePath, string runtimeFolder, string editorFolder)
        {
            SetSize(9);
            CreatorName = "Game Event Creator";

            var unityEventsFolder = "Events/UnityEvents";
            //UnityEvent template
            items[0] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "Event", $"{runtimeFolder}/{unityEventsFolder}", "", "cs"),
                Required = true
            };

            //GameEvent templates
            var gameEventFolder = "Events/GameEvents";
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
            var listenerFolder = "Events/Listeners";
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

            var triggerFolder = "Events/EventTriggers/Triggers";
            items[5] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "EventTrigger", $"{runtimeFolder}/{triggerFolder}", "", "cs"),
                Dependence = true,
                DependenceIndex = 1
            };
            items[6] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "EventTriggerEditor", $"{editorFolder}/{triggerFolder}", "", "cs"),
                Dependence = true,
                DependenceIndex = 5
            };

            var collisionTriggerFolder = "Events/EventTriggers/CollisionTriggers";
            items[7] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "CollisionEventTrigger", $"{runtimeFolder}/{collisionTriggerFolder}", "", "cs"),
                Dependence = true,
                DependenceIndex = 1
            };
            items[8] = new TemplateCreatorItem
            {
                Template = new FileTemplate($"{templatePath}/", "CollisionEventTriggerEditor", $"{editorFolder}/{collisionTriggerFolder}", "", "cs"),
                Dependence = true,
                DependenceIndex = 7
            };




            UpdateNames("");
        }


    }
}