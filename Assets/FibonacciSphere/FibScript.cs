﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FibScript : MonoBehaviour
{
    public GameObject sphereObject;
    [HideInInspector]
    public int N;
    private string fibSeq;

    //  Phyllotaxis

    [HideInInspector]
    public float Angle = 137.51f;
    [HideInInspector]
    public float C = .05f;
    [HideInInspector]
    public int Count;
    [HideInInspector]
    public int Mark;
    [HideInInspector]
    public float Radius;

    public void GenerateFibSeq()
    {
        RemoveChildren();
        fibSeq = "";
        int n1 = 0;
        int n2 = 1;
        int nextN;

        float[] x = new float[N];
        float[] y = new float[N];
        string xString = "", yString = "";
        float phi = (1 + Mathf.Sqrt(5)) / 2;

        fibSeq += "1";

        for(int i = 0; i < N; i++)
        {
            x[i] = (i / phi) % .1f;
            y[i] = (float)i / N;

            xString += x[i].ToString() + ", ";
            yString += y[i].ToString() + ", ";

            nextN = n1 + n2;
            n1 = n2;
            n2 = nextN;
            //Instantiate(sphereObject, new Vector3(this.transform.position.x, nextN, this.transform.position.z), this.transform.rotation, this.transform);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3((2*Mathf.PI*x[i]), Mathf.Acos(1-2*y[i]), this.transform.position.z);
            float scaleVal = .01f;
            sphere.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            sphere.transform.parent = this.transform;
            fibSeq += "," + nextN.ToString();
        }
        print(fibSeq);
        print(xString);
        print(yString);
    }

    //  Generates the Phyllotaxis Pattern based on an angle in 2D
    public void GeneratePhyllotaxis()
    {
        RemoveChildren();
        float x, y, theta, r;

        string finalAns = "";
        
        for(int n = 0; n < Count; n++)
        {
            theta = n * Angle;
            r = C * Mathf.Sqrt(n);
            x = r * Mathf.Cos(theta);
            y = r * Mathf.Sin(theta);
            finalAns += x + "," + y + "  ";

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(x, y, this.transform.position.z);
            float scaleVal = .075f;
            sphere.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            sphere.transform.parent = this.transform;
        }
        finalAns.TrimEnd(new char[] { ' ' });
        print(finalAns);
    }

    //  Generates the Phyllotaxis Pattern based on an angle on a sphere
    public void GenerateSpherePhyllotaxis()
    {
        RemoveChildren();
        float x, y, z, theta, r;
        float a = (180f / (float)Count);

        Vector3[] directions = new Vector3[Count];

        // setup for writing the directions to a file
        string altDirectionsStr = "";

        GameObject sphere;
        for (int n = 0; n < Count; n++)
        {
            theta = n * Angle * Mathf.Deg2Rad;
            float angle = (n * a) * Mathf.Deg2Rad;
            float adjacent = Radius * Mathf.Cos(angle);
            r = Mathf.Sin(angle) * Radius;
            x = r * Mathf.Cos(theta);
            y = r * Mathf.Sin(theta);
            z = (angle < 90) ? -adjacent : adjacent;

            sphere = Instantiate(sphereObject);
            sphere.transform.position = new Vector3(x, y, z);
            float scaleVal = .075f;
            sphere.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            sphere.transform.parent = this.transform;
            sphere.transform.name = n.ToString();
            sphere.AddComponent<SphereDataScript>().SetValues(theta, r, angle, x, y, z);

            Vector3 altDirection = sphere.transform.position - this.transform.position;
            print($"Vector to sphere {n}: {altDirection}");
            directions[n] = altDirection;

            // write to directions document
            altDirectionsStr += $"new Vector3({altDirection.x}f, {altDirection.y}f, {altDirection.z}f),\n";
        }

        // if the directions were found and the directions.txt document is available write them to the file
        if (File.Exists("Assets/FibonacciSphere/directions.txt")){
            Debug.Log("directions.txt does exist, writing to file");
            var sr = File.CreateText("Assets/FibonacciSphere/directions.txt");
            sr.WriteLine(altDirectionsStr);
            //sr.Write("Just a test");
            sr.Close();
        }
        else{
            Debug.Log("directions.txt doesn't exist");
        }

        //StartCoroutine(ShowDirections(directions));
    }

#region Debug Show Lines
    float directionDisplayTime = 0.25f;

    private IEnumerator ShowDirections(Vector3[] directions){
        foreach (Vector3 direction in directions){
            print($"tick: {direction}");
            Debug.DrawRay(this.transform.position, direction, Color.blue, directionDisplayTime);
            yield return new WaitForSeconds(directionDisplayTime);
        }
    }
#endregion

    public void TestSphere()
    {
        RemoveChildren();
        float x, y, z;

        GameObject sphere;
        for(float i = 0.0f; i < 4; i = i + 0.1f)
        {
            float theta = i * Angle * Mathf.Deg2Rad;
            x = Radius * Mathf.Cos(i);
            y = Radius * Mathf.Sin(i);
            z = Radius * Mathf.Cos(i);

            sphere = Instantiate(sphereObject);
            sphere.transform.position = new Vector3(x, y, z);
            float scaleVal = .075f;
            sphere.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            sphere.transform.parent = this.transform;
        }
    }

    private void RemoveChildren()
    {
        int numChildren = transform.childCount;
        for(int i = 0; i < numChildren; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
