using GTVariable;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


public enum SidebarItemType
{
    None,
    Channel,
    Event,
    Variable
}

public class SidebarItem : TreeViewItem 
{
    public SerializedProperty TargetProperty { get; }
    public SerializedProperty Array { get; }
    public SerializedProperty Channel { get; }
    public Dictionary<string,int> NamesCounter { get; } 
    public List<string> AllNames { get { return NamesCounter.Keys.ToList(); } }

    public SidebarItemType Type { get; } = SidebarItemType.None;
    public bool CanMultiSelected { get; } = false;
    public int IndexInArray { get; }
    public string Error { get; private set; } = string.Empty;

    public Object currentObject
    {
        get
        {
            if (TargetProperty.propertyType == SerializedPropertyType.ObjectReference)
            {
                return TargetProperty == null ? null : TargetProperty.objectReferenceValue;
            }
            else
            {
                return null;
            }

        }
        set
        {
            if (TargetProperty.propertyType == SerializedPropertyType.ObjectReference)
            {
                TargetProperty.serializedObject.Update();
                TargetProperty.objectReferenceValue = value;
                TargetProperty.serializedObject.ApplyModifiedProperties();
            }
        }
    }

    public override string displayName
    {
        get
        {
            if (TargetProperty.propertyType == SerializedPropertyType.ObjectReference)
            {
                return currentObject == null ? "<None>" : currentObject.name;
            }
            else
            {
                return IndexInArray == -1 ? "Default": TargetProperty.FindPropertyRelative("name").stringValue;
            }
        }
        set 
        {
            TargetProperty.serializedObject.Update();
            if (TargetProperty.propertyType == SerializedPropertyType.ObjectReference)
            {
                currentObject.name = GetUnquiedName(value, displayName);
                if (currentObject != null)
                {
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(currentObject), currentObject.name);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
            else
            {
                TargetProperty.FindPropertyRelative("name").stringValue = GetUnquiedName(value, displayName);
            }
            TargetProperty.serializedObject.ApplyModifiedProperties();
        }
    }

    private string GetUnquiedName(string value,string currentName)
    {
        if(NamesCounter.TryGetValue(currentName,out int counter))
        {
            if (counter == 1)
            {
                NamesCounter.Remove(currentName);
            }
            else
            {
                NamesCounter[currentName]--;
            }
        }

        if(NamesCounter.TryGetValue(value,out counter))
        {
            if (counter == 0) return value;
        }

        var newName = ObjectNames.GetUniqueName(NamesCounter.Keys.ToArray(), value);

        if(NamesCounter.ContainsKey(newName) == false)
        {
            NamesCounter.Add(newName, 1);
        }
        else
        {
            NamesCounter[newName]++;
        }

        return newName;
    }

    public SidebarItem(int id, int depth, SerializedProperty targetProperty, SerializedProperty array, SidebarItemType type,Dictionary<string,int> namesCounter,SerializedProperty channel = null, int indexInArray = -1)
        : base(id, depth)
    {
        TargetProperty = targetProperty;
        Array = array;
        Type = type;
        IndexInArray = indexInArray;
        Channel = channel;
        CanMultiSelected = type == SidebarItemType.Event || type == SidebarItemType.Variable;
        NamesCounter = namesCounter == null ? new Dictionary<string, int>() : namesCounter;

        var displayName = this.displayName;

        if (NamesCounter.ContainsKey(displayName) == false)
        {
            NamesCounter.Add(displayName, 1);
        }
        else
        {
            NamesCounter[displayName]++;
        }
    }

    public void Validated()
    {
        Error = string.Empty;
    }
}