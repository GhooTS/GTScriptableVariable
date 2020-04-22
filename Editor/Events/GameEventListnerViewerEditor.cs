using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEventListnerViewer))]
public class GameEventListnerViewerEditor : Editor
{
    private readonly List<GameEventListener> listeners = new List<GameEventListener>();
    private readonly List<bool> foldout = new List<bool>();
    private readonly List<Editor> listenersEditor = new List<Editor>();
    private GameEventListnerViewer viewer;
    private GameObject gameObject;
    private bool listenerVisible = false;

    private void OnEnable()
    {
        viewer = target as GameEventListnerViewer;
        gameObject = viewer.gameObject;
        listenerVisible = serializedObject.FindProperty("listenersVisable").boolValue;

        foreach (var item in gameObject.GetComponents<GameEventListener>())
        {
            AddListner(item);
        }
    }

    private void OnDisable()
    {
        //Make all listners visable only if there is non GameEventLisnterViewer attach to the game object
        if (gameObject != null && gameObject.TryGetComponent(out GameEventListnerViewer _) == false)
        {
            ChangeListnerVisableState(true);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        

        DrawOptions();
        DrawListeners();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawListeners()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            foldout[i] = EditorGUILayout.BeginFoldoutHeaderGroup(foldout[i], listeners[i].name, null,
                (position) =>
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Remove"), false, OnRemoveSelectedElement, i);
                    menu.DropDown(position);

                });

            if (foldout[i])
            {
                listenersEditor[i].OnInspectorGUI();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

    private void DrawOptions()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Listner"))
        {
            AddListner(gameObject.AddComponent<GameEventListener>());
        }

        if (GUILayout.Button(listenerVisible ? "Hide Listners" : "Show Listners"))
        {
            ChangeListnerVisableState(!listenerVisible);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void ChangeListnerVisableState(bool visablityState)
    {
        if (visablityState == listenerVisible)
        {
            return;
        }

        foreach (var listner in listeners)
        {
            listner.hideFlags = visablityState ? HideFlags.None : HideFlags.HideInInspector;
        }

        serializedObject.FindProperty("listnersVisable").boolValue = visablityState;
        listenerVisible = visablityState;
    }

    private void AddListner(GameEventListener eventListner)
    {
        listeners.Add(eventListner);
        foldout.Add(false);
        listenersEditor.Add(Editor.CreateEditor(eventListner));
        eventListner.hideFlags = listenerVisible ? HideFlags.None : HideFlags.HideInInspector;
    }

    private void OnRemoveSelectedElement(object elementIndex)
    {
        RemoveElement((int)elementIndex);
    }

    private void RemoveElement(int index)
    {
        DestroyImmediate(listeners[index]);
        listeners.RemoveAt(index);
        foldout.RemoveAt(index);
        listenersEditor.RemoveAt(index);
    }


    
}
