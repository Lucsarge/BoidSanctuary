using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class FibScript : MonoBehaviour
{
    public GameObject sphereObject;

    #region Fibonnaci Sequence Properties
    [HideInInspector]
    public int N;
    private string fibSeq;
    #endregion

    #region Phyllotaxis Properties
    // The angle that is applied for each iteration of the sequence
    [HideInInspector]
    public float Angle = 137.51f;

    // 
    [HideInInspector]
    public float C = .05f;

    // Multiplier that should be applied to each of the next points
    // The ideal turn fraction should be 0.618 which is roughly the golden ration PHI
    [HideInInspector]
    public float TurnFraction = 0.618f;

    // The power to which the distance scalar value is being raised to in order to correct for the bunching up of dots on the phyllotaxis
    // 0.5 allow for even spacing
    // approaching 1.0 causes the objects to be bunched up in center where the distribution begins
    // approaching 0.0 causes the objects to be receeded from the center and bunch up towards the outside where the distribution ends
    [HideInInspector]
    public float SpacingPow = 0.5f;

    // Number of dots to populate the phyllotaxis with
    [HideInInspector]
    public int Count;

    // Controls the radius for the spheres, does not impact the plane phyllotaxis
    [HideInInspector]
    public float Radius;
    #endregion

    public void GenerateFibSeq()
    {
        RemoveChildren(); // clear the scene for re-drawing
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

    // Generates the Phyllotaxis Pattern based on an angle in 2D
    public void GenerateDisk()
    {
        RemoveChildren(); // clear the scene for re-drawing

        float x, y; // Ultimately the x and y positioning for the points in the disk
        float angle; // Degree offset for determining the position of the next point
        float dist; // Distance between each point

        string finalAns = "";

        float twoPI = 2 * Mathf.PI;

        for(int n = 0; n < Count; n++)
        {
            dist = Mathf.Pow(n / (Count - 1.0f), SpacingPow); // calculate the distance from the center that the dot should be placed
            angle = twoPI * TurnFraction * n; // calculate the angle that the next dot should be placed
            x = dist * Mathf.Cos(angle); // find x coordinate
            y = dist * Mathf.Sin(angle); // find y coordinate

            // writing down for debug purposes
            finalAns += x + "," + y + "  ";

            // create the object to visually display the coordinates for this dot in the scene
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(x, y, this.transform.position.z);
            float scaleVal = .035f;
            sphere.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            sphere.transform.parent = this.transform;
        }
        finalAns.TrimEnd(new char[] { ' ' });
        print(finalAns);
    }

    // Generates the Phyllotaxis Pattern based on an angle on a sphere
    public void GenerateSphere()
    {
        RemoveChildren(); // clear the scene for re-drawing

        float t;
        float inclination;
        float azimuth;

        float twoPI = 2 * Mathf.PI;

        float x, y, z; // Ultimately the x, y, and z positioning for the points on the sphere

        // setup for writing the directions to a file
        string altDirectionsStr = "";

        for(int n = 0; n < Count; n++)
        {
            //dist = Mathf.Pow(n / (Count - 1.0f), SpacingPow); // calculate the distance from the center that the dot should be placed
            //angle = twoPI * TurnFraction * n; // calculate the angle that the next dot should be placed

            // TODO: allows for divide by zero error, not sure if I need to fix
            t = n / (Count - 1.0f); // scalar that controls how far along the sequence is from the start of the sphere to the end, setting to a fixed value from 0 - 1 demonstrates its purpose best
            inclination = Mathf.Acos(1 - 2 * t); // Acos(1 - t) provides values ascending values between 0 and 2, Acos(2x) condenses those range of those values in half, both steps give you ascending values from 0 - 1
            azimuth = twoPI * TurnFraction * n; // provides incremental steps for the turning value similar to angle in the phyllotaxis disk

            // these coordinates are being found as spherical coordinates as oppossed to polar
            x = Mathf.Sin(inclination) * Mathf.Cos(azimuth); // find x coordinate
            y = Mathf.Sin(inclination) * Mathf.Sin(azimuth); // find y coordinate
            z = Mathf.Cos(inclination); // find z coordinate

            // write to directions string
            altDirectionsStr += $"new Vector3({x}f, {y}f, {z}f),\n";

            // create the object to visually display the coordinates for this dot in the scene
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(x, y, z);
            float scaleVal = .035f;
            sphere.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            sphere.transform.parent = this.transform;
        }

        // if the directions were found and the directions.txt document is available write them to the file
        if (File.Exists("Assets/FibonacciSphere/directions.txt")){
            Debug.Log("directions.txt does exist, writing to file");
            var sr = File.CreateText("Assets/FibonacciSphere/directions.txt");
            sr.WriteLine(altDirectionsStr);
            sr.Close();
        }
        else{
            Debug.Log("directions.txt doesn't exist");
        }

        // OLD ATTEMPT
        // TODO: remove old code once a comparison is done for a better understanding of the correct implementation
        // RemoveChildren(); // clear the scene for re-drawing
        // float x, y, z, theta, r;
        // float a = (180f / (float)Count);

        // Vector3[] directions = new Vector3[Count];

        // // setup for writing the directions to a file
        // string altDirectionsStr = "";

        // GameObject sphere;
        // for (int n = 0; n < Count; n++)
        // {
        //     theta = n * Angle * Mathf.Deg2Rad;
        //     float angle = (n * a) * Mathf.Deg2Rad;
        //     float adjacent = Radius * Mathf.Cos(angle);
        //     r = Mathf.Sin(angle) * Radius;
        //     x = r * Mathf.Cos(theta);
        //     y = r * Mathf.Sin(theta);
        //     z = (angle < 90) ? -adjacent : adjacent;

        //     sphere = Instantiate(sphereObject);
        //     sphere.transform.position = new Vector3(x, y, z);
        //     float scaleVal = .075f;
        //     sphere.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
        //     sphere.transform.parent = this.transform;
        //     sphere.transform.name = n.ToString();
        //     sphere.AddComponent<SphereDataScript>().SetValues(theta, r, angle, x, y, z);

        //     Vector3 altDirection = sphere.transform.position - this.transform.position;
        //     print($"Vector to sphere {n}: {altDirection}");
        //     directions[n] = altDirection;

        //     // write to directions document
        //     altDirectionsStr += $"new Vector3({altDirection.x}f, {altDirection.y}f, {altDirection.z}f),\n";
        // }

        // // if the directions were found and the directions.txt document is available write them to the file
        // if (File.Exists("Assets/FibonacciSphere/directions.txt")){
        //     Debug.Log("directions.txt does exist, writing to file");
        //     var sr = File.CreateText("Assets/FibonacciSphere/directions.txt");
        //     sr.WriteLine(altDirectionsStr);
        //     //sr.Write("Just a test");
        //     sr.Close();
        // }
        // else{
        //     Debug.Log("directions.txt doesn't exist");
        // }

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
        RemoveChildren(); // clear the scene for re-drawing
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
