using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GTVariable.Editor
{
    public abstract class GameEventEditorBase<T> : UnityEditor.Editor
        where T : GameEventBase
    {
        protected readonly List<Listener> listeners = new List<Listener>();
        protected readonly List<bool> validionStates = new List<bool>();
        protected readonly List<bool> foldout = new List<bool>();
        protected Vector2 subscribersScroll;
        protected T gameEvent;
        protected int page = 0;
        protected int showPerPage = 5;
        protected bool responseVisable = false;

        private UnityEventBase response;

        private const string showPerPageKey = "GameEventEditor_SHOWPERPAGE";
        private const string responseVisableKey = "GameEventEditor_RESPONSEVISABLE";
        private Delegate[] invocationList = new Delegate[0];


        private void OnEnable()
        {
            gameEvent = target as T;
            UpdateListenersList();
            showPerPage = EditorPrefs.GetInt(showPerPageKey);
            responseVisable = EditorPrefs.GetBool(responseVisableKey);
        }


        private void OnDisable()
        {
            EditorPrefs.SetInt(showPerPageKey, showPerPage);
            EditorPrefs.SetBool(responseVisableKey, responseVisable);
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawOptions();
            DrawSubscribers();
        }

        private void OnSceneGUI()
        {
            Debug.Log("Scene GUI");
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

            if (Application.isPlaying) DrawParameter();

            EditorGUILayout.EndHorizontal();
            invocationList = gameEvent.OnEventRaised?.GetInvocationList();
            var invokationListCount = invocationList == null ? 0 : invocationList.Length;
            showPerPage = EditorGUILayout.IntSlider("Show Per Page", showPerPage, 1, 50);
            page = EditorGUILayout.IntSlider("Page", page, 0, Mathf.Max(0, (listeners.Count + invokationListCount - 2) / showPerPage));
            responseVisable = EditorGUILayout.Toggle("Always show response", responseVisable);
        }

        public void DrawSubscribers()
        {
            GUILayout.Label("Subscirbers:", EditorStyles.boldLabel);
            subscribersScroll = EditorGUILayout.BeginScrollView(subscribersScroll);
            for (int i = 0; i < showPerPage; i++)
            {
                int index = showPerPage * page + i;

                if (index >= listeners.Count) break;

                if (listeners[index] == null)
                {
                    UpdateListenersList();
                    break;
                }

                DrawSubscriber(index);
            }

            if(invocationList != null)
            {
                for (int i = 0; i < showPerPage; i++)
                {
                    int index = showPerPage * page + i + listeners.Count;

                    if (index < 0 || index >= invocationList.Length) break;

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Box("", EditorStyles.helpBox, GUILayout.Width(5f), GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing));
                    EditorGUILayout.BeginVertical();
                    if (invocationList[index].Target is UnityEngine.Object)
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.ObjectField("Target",invocationList[index].Target as UnityEngine.Object, invocationList[index].Target.GetType(), true);
                        EditorGUI.EndDisabledGroup();
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Target", invocationList[index].Target.ToString());
                    }
                    EditorGUILayout.LabelField("Method", invocationList[index].Method.Name);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                }
            }

            EditorGUILayout.EndScrollView();

        }

        public bool DrawSubscriber(int i)
        {
            var name = listeners[i].listenerName;
            if (string.IsNullOrEmpty(name)) name = "[No name specify]";


            GUILayout.Box(new GUIContent(name,listeners[i].listenerDescription), GUILayout.ExpandWidth(true));
            var enabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Listener", listeners[i], listeners[i].GetType(), true);
            GUI.enabled = enabled;

            response = listeners[i].GetResponse();

            foldout[i] = EditorGUILayout.BeginFoldoutHeaderGroup(foldout[i], GameEventGUI.GetResposeFoldoutHeader(response, validionStates[i]));

            if (foldout[i] || responseVisable)
            {
                GameEventGUI.DrawResponse(response);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(10f);

            return false;
        }

        private void CheckForResponseProblems()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                var valid = ListenerUtility.IsListenerValid(listeners[i]);
                if (i >= validionStates.Count)
                {
                    validionStates.Add(valid);
                }
                else
                {
                    validionStates[i] = valid;
                }
            }
        }

        public abstract void RaiseEvent();
        public virtual void DrawParameter()
        {

        }

        public void UpdateListenersList()
        {
            listeners.Clear();
            GameEventUtility.GetAssosiatedListenersInScene(gameEvent, listeners);
            CheckForResponseProblems();
            for (int i = foldout.Count; i < listeners.Count; i++)
            {
                foldout.Add(false);
            }
        }
    }
}