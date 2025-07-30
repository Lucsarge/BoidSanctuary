using System;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(FlockScript))]
public class FlockInspector : Editor
{
    private const string MESH_SAVE_DIRECTORY = "Assets/Boids/Resources/BoidMesh.asset";

    public override void OnInspectorGUI()
    {
        FlockScript flockTarget = target as FlockScript;

        base.OnInspectorGUI();

        if (GUILayout.Button("Save Boid Mesh"))
        {
            // save mesh to the save
            Mesh boidMesh = CreateBoidMesh();
            AssetDatabase.CreateAsset(boidMesh, MESH_SAVE_DIRECTORY);
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Skip +1"))
        {
            flockTarget.Simulate(FlockScript.SIMULATION_TIME);
        }

        if (GUILayout.Button("Skip +200"))
        {
            for (int i = 0; i < 200; i++)
            {
                flockTarget.Simulate(FlockScript.SIMULATION_TIME);
            }
        }
    }

    #region Boid Mesh
    private static Vector3[] boidVerts = new Vector3[]
    {
        new Vector3(-.5f, 0, -.5f), // base left
        new Vector3(0, .25f, -.5f), // base top
        new Vector3(.5f, 0, -.5f), // base right
        new Vector3(0, -.25f, -.5f), // base bottom
        new Vector3(0, 0, .5f) // front
    };

    private static int[] boidTriangs = new int[]
    {
        0,1,2,
        2,3,0,
        0,4,1,
        2,1,4,
        2,4,3,
        0,3,4
    };

    // Creates and returns a boid mesh
    // Currently makes a pyramid like object
    private Mesh CreateBoidMesh()
    {
        Mesh mesh = new Mesh { name = "Boid Mesh" };

        mesh.Clear();
        mesh.vertices = boidVerts;
        mesh.triangles = boidTriangs;
        mesh.RecalculateNormals();

        return mesh;
    }
    #endregion
}
