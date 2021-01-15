using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(FibScript))]
public class FibScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FibScript myTarget = (FibScript)target;

        myTarget.N = EditorGUILayout.IntSlider("N value", myTarget.N, 0, 200);

        if (GUILayout.Button("Gen Fibonacci"))
        {
            myTarget.GenerateFibSeq();
        }

        //Phyllotaxis Gen
        myTarget.Angle = EditorGUILayout.Slider("Angle", myTarget.Angle, 0.0f, 360.0f);
        myTarget.C = EditorGUILayout.Slider("C", myTarget.C, 0.0f, 1.0f);
        myTarget.Count = EditorGUILayout.IntSlider("Count", myTarget.Count, 1, 500);
        myTarget.Mark = EditorGUILayout.IntField("Mark every: ", myTarget.Mark);
        myTarget.Radius = EditorGUILayout.Slider("Radius", myTarget.Radius, 1f, 20f);
        if (GUILayout.Button("Gen Phyllotaxis"))
        {
            myTarget.GeneratePhyllotaxis();
        }
        if (GUILayout.Button("Gen Sphere"))
        {
            myTarget.GenerateSpherePhyllotaxis();
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
