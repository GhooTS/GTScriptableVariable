﻿using UnityEditor;
using UnityEngine;

namespace GTVariable.Editor
{
    [CustomEditor(typeof(FloatEventTrigger))]
    public class FloatEventTriggerEditor : EventTriggerEditor<FloatGameEvent, FloatListener, FloatEvent, float>
    {

    }
}