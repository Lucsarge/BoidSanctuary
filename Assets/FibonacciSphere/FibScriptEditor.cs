using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FibScript))]
public class FibScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FibScript myTarget = (FibScript)target;
        myTarget.experience = EditorGUILayout.IntField("Exp: ", myTarget.experience);
        EditorGUILayout.LabelField("Level", myTarget.Level.ToString());

        myTarget.N = EditorGUILayout.IntSlider("N value", myTarget.N, 0, 200);

        if (GUILayout.Button("Gen Fibonacci"))
        {
            myTarget.GenerateFibSeq();
        }
    }
}


public class MenuItems
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void NewMenuOption()
    {
        
    }
}
