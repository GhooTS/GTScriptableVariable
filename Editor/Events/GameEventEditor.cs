using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    private List<GameEventListener> gameEventListners = new List<GameEventListener>();
    private readonly List<bool> foldout = new List<bool>();
    private GameEvent gameEvent;

    private void OnEnable()
    {
        gameEvent = target as GameEvent;
        gameEventListners = EditorApplication.isPlaying ? gameEvent.EventListners : FindGameEventListnerInScene();
    }

    public override void OnInspectorGUI()
    {
        DrawOptions();
        DrawSubscribers();
    }

    private void DrawOptions()
    {
        EditorGUILayout.BeginHorizontal();

        bool enable = GUI.enabled;

        GUI.enabled = EditorApplication.isPlaying;
        if (GUILayout.Button("Fire"))
        {
            gameEvent.Raise();
        }

        GUI.enabled = enable;

        if (EditorApplication.isPlaying)
        {
            gameEventListners = gameEvent.EventListners;
        }
        else
        {
            if (GUILayout.Button("Find in scene"))
            {
                gameEventListners = FindGameEventListnerInScene();
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private List<GameEventListener> FindGameEventListnerInScene()
    {
        return FindObjectsOfType<GameEventListener>()
                .Where(p => p.gameEvents.Where(c => c == (target as GameEvent)).ToArray().Length > 0).ToList();
    }


    private void DrawSubscribers()
    {
        GUILayout.Label("Subscirbers:", EditorStyles.boldLabel);

        for (int i = 0; i < gameEventListners.Count; i++)
        {
            if(foldout.Count <= i)
            {
                foldout.Add(false);
            }

            DrawSubscriber(i);
        }

        //Remove redundace foldout
        if(foldout.Count != gameEventListners.Count)
        {
            foldout.RemoveRange(gameEventListners.Count, foldout.Count - gameEventListners.Count);
        }
    }

    private void DrawSubscriber(int id)
    {
        foldout[id] = EditorGUILayout.BeginFoldoutHeaderGroup(foldout[id], gameEventListners[id].name,null,
            (position) => {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Focus"), false, () => {
                    Selection.activeObject = gameEventListners[id];
                });
                menu.DropDown(position);
            });
        if (foldout[id])
        {
            EditorGUILayout.LabelField("Response:");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Target", EditorStyles.boldLabel, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
            EditorGUILayout.LabelField($"Method", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();
            for (int i = 0; i < gameEventListners[id].Response.GetPersistentEventCount(); i++)
            {
                EditorGUILayout.BeginHorizontal();
                var eventTarget = gameEventListners[id].Response.GetPersistentTarget(i);
                if (eventTarget == null)
                {
                    var bgColor = GUI.color;
                    GUI.contentColor = Color.red;
                    EditorGUILayout.LabelField("NULL");
                    GUI.contentColor = bgColor;
                }
                else
                {
                    GUILayout.Label($"{eventTarget.name}", GUILayout.MaxWidth(100));
                    var method = gameEventListners[id].Response.GetPersistentMethodName(i);
                    GUILayout.Label($"{(method == "" ? "NULL" : method)}");
                }
                if (GUILayout.Button("Focus target", GUILayout.MaxWidth(50)))
                {
                    Selection.activeObject = gameEventListners[id];
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}
