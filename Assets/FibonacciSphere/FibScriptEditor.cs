using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FibScript))]
public class FibScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Separator();

        FibScript myTarget = (FibScript)target;

        #region Fibonacci Properties
        EditorGUILayout.LabelField("Fibonacci Properties");
        myTarget.N = EditorGUILayout.IntSlider("N value", myTarget.N, 0, 200);
        EditorGUILayout.Separator();
        #endregion

        //Phyllotaxis Gen
        #region Phyllotaxis Properties
        EditorGUILayout.LabelField("Phyllotaxis Properties");
        myTarget.Angle = EditorGUILayout.Slider("Angle", myTarget.Angle, 0.0f, 360.0f);
        myTarget.C = EditorGUILayout.Slider("C", myTarget.C, 1.0f, 2.0f);
        myTarget.TurnFraction = EditorGUILayout.Slider("Turn Fraction", myTarget.TurnFraction, 0.0f, 2.0f);
        myTarget.SpacingPow = EditorGUILayout.Slider("Spacing Pow", myTarget.SpacingPow, 0.0f, 1.0f);
        myTarget.Count = EditorGUILayout.IntSlider("Count", myTarget.Count, 1, 1000);
        myTarget.Radius = EditorGUILayout.Slider("Radius", myTarget.Radius, 0f, 10f);
        if (GUILayout.Button("Gen Fibonacci"))
        {
            myTarget.GenerateFibSeq();
        }
        if (GUILayout.Button("Gen Phyllotaxis Plane"))
        {
            myTarget.GenerateDisk();
        }
        if (GUILayout.Button("Gen Phyllotaxis Sphere"))
        {
            myTarget.GenerateSphere();
        }
        if(GUILayout.Button("Test Sphere"))
        {
            myTarget.TestSphere();
        }
        #endregion
    }
}


public class MenuItems
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void NewMenuOption()
    {
        
    }
}
