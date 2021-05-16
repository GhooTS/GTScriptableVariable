using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace GTVariable.Editor
{ 
    public class ChannelToTexture
    {
        public string ChannelName;
        public Texture2D texture;
    }

    public class ChannelToTextureProvider : ScriptableObject
    {
        public List<ChannelToTexture> channelsTexture = new List<ChannelToTexture>();
    }

    public static class GameEventGUI
    {

        public static void DrawResponse(UnityEventBase response)
        {
            var eventCount = response.GetPersistentEventCount();

            for (int i = 0; i < eventCount; i++)
            {
                var validationState = ListenerUtility.ValidedResponse(response, i);

                if (validationState == ListenerValidionState.Valid)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Box("", EditorStyles.helpBox,GUILayout.Width(5f),GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing));
                    EditorGUILayout.BeginVertical();
                    var enabled = GUI.enabled;
                    GUI.enabled = false;
                    EditorGUILayout.ObjectField("Target",response.GetPersistentTarget(i), response.GetPersistentTarget(i).GetType(), true);
                    GUI.enabled = enabled;
                    EditorGUILayout.LabelField("Method name",response.GetPersistentMethodName(i));
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.HelpBox(ValidationMessage.GetMessage(validationState), MessageType.Error);
                }
            }
        }

        public static string GetResposeFoldoutHeader(UnityEventBase respose,bool isValide)
        {
            int responseCount = respose.GetPersistentEventCount();
            return $"{(isValide ? "" : $"<color={(EditorGUIUtility.isProSkin ? "#F77" : "#E44")}>")}Response ({responseCount}){(isValide ? "" : "</color>")}";
        }

    } 
}