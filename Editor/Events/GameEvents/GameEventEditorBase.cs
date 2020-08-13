using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


namespace GTVariable.Editor
{

    public abstract class GameEventEditorBase : UnityEditor.Editor
    {
        private bool initizale = false;
        protected int currentSelected = -1;
        protected string selectedName;
        Vector2 responseScroll, subscirbersScroll;
        GUIStyle errorStyle;


        public void Init()
        {
            if (initizale) return;

            errorStyle = new GUIStyle(EditorStyles.label)
            {
                imagePosition = ImagePosition.ImageOnly
            };
            initizale = true;
        }

        public void DrawOptions()
        {
            EditorGUILayout.BeginHorizontal();

            if (EditorApplication.isPlaying && GUILayout.Button("Raise"))
            {
                RaiseEvent();
            }

            if (GUILayout.Button("Find in scene"))
            {
                UpdateListenersList();
            }

            EditorGUILayout.EndHorizontal();
        }

        public void DrawSubscribers<T>(List<T> gameEventListeners, List<bool> listenerValid)
            where T : Listener
        {
            GUILayout.Label("Subscirbers:", EditorStyles.boldLabel);
            subscirbersScroll = EditorGUILayout.BeginScrollView(subscirbersScroll, GUILayout.MaxHeight(Screen.height - 300));
            for (int i = 0; i < gameEventListeners.Count; i++)
            {
                bool selected;
                if (listenerValid.Count <= i || listenerValid[i])
                {
                    selected = DrawSubscriber(gameEventListeners[i]);
                }
                else
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUILayout.LabelField(EditorGUIUtility.IconContent("console.erroricon.sml"), errorStyle, GUILayout.MaxHeight(20), GUILayout.MaxWidth(30));
                    selected = DrawSubscriber(gameEventListeners[i]);
                    EditorGUILayout.EndHorizontal();
                }
                if (selected)
                {
                    currentSelected = i;
                    selectedName = gameEventListeners[i].listenerName;
                }
            }
            EditorGUILayout.EndScrollView();

        }

        public bool DrawSubscriber<T>(T listener)
            where T : Listener
        {
            var name = listener.listenerName;
            if (string.IsNullOrEmpty(name)) name = "[No name specify]";
            EditorGUILayout.BeginHorizontal();
            var enabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.ObjectField(listener.gameObject, listener.gameObject.GetType(), true);
            GUI.enabled = enabled;
            var selected = GUILayout.Button(name);

            EditorGUILayout.EndHorizontal();

            return selected;
        }

        public void DrawResponse(UnityEngine.Events.UnityEventBase response)
        {


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Target",EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField("Method name",EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.EndHorizontal();

            var eventCount = response.GetPersistentEventCount();

            responseScroll = EditorGUILayout.BeginScrollView(responseScroll, GUILayout.MaxHeight(200));
            for (int i = 0; i < eventCount; i++)
            {
                var validationState = ListenerUtility.ValidedResponse(response,i);

                EditorGUILayout.BeginHorizontal();

                if (validationState == ListenerValidionState.Valid)
                {
                    var enabled = GUI.enabled;
                    GUI.enabled = false;
                    EditorGUILayout.ObjectField(response.GetPersistentTarget(i), response.GetPersistentTarget(i).GetType(), true);
                    GUI.enabled = enabled;
                    EditorGUILayout.LabelField(response.GetPersistentMethodName(i));
                }
                else
                {
                    EditorGUI.HelpBox(GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth - 50, EditorGUIUtility.singleLineHeight + 5)
                                                              ,ValidationMessage.GetMessage(validationState)
                                                              ,MessageType.Error);
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        

        public void DrawSelected<ListenerType,EventType>(ListenerType listener,EventType respones)
            where ListenerType : Listener
            where EventType : UnityEngine.Events.UnityEventBase
        {
            EditorGUILayout.BeginHorizontal();
            var enabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.ObjectField(listener.gameObject, listener.gameObject.GetType(), true);
            GUI.enabled = enabled;
            EditorGUILayout.LabelField(selectedName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("Response", EditorStyles.boldLabel);
            DrawResponse(respones);
        }

        public abstract void RaiseEvent();
        public abstract void UpdateListenersList();
    }
}