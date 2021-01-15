using System.Collections;
using System.Collections.Generic;
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

        string finalAns = "";

        GameObject sphere;
        for (int n = 0; n < Count; n++)
        {
            //if(n % Mark == 0)
            //{
            theta = n * Angle;
            r = C * Mathf.Sqrt(n);
            x = r * Mathf.Cos(theta);
            y = r * Mathf.Sin(theta);
            float angle = (n * a) * Mathf.Deg2Rad;
            if(angle < 90)
            {
                float adjacent = Radius * Mathf.Cos(angle);
                float opposite = Radius * Mathf.Cos(angle);
                adjacent *= -1;
                z = adjacent;
            }
            else
            {
                float adjacent = Radius * Mathf.Cos(angle);
                float opposite = Radius * Mathf.Cos(angle);
                z = adjacent;
            }
            finalAns += x + "," + y + "," + z + "  ";

            sphere = Instantiate(sphereObject);
            sphere.transform.position = new Vector3(x, y, z);
            float scaleVal = .075f;
            sphere.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            sphere.transform.parent = this.transform;
            sphere.transform.name = n.ToString();
            //}
            //else
            //{
            //    theta = n * Angle;
            //    r = C * Mathf.Sqrt(n);
            //    x = r * Mathf.Cos(theta);
            //    y = r * Mathf.Sin(theta);
            //    z = (float)n / Count;
            //    finalAns += x + "," + y + "," + z + "  ";

            //    sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //    sphere.transform.position = new Vector3(x, y, z);
            //    float scaleVal = .075f;
            //    sphere.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            //    sphere.transform.parent = this.transform;
            //}

        }
        finalAns.TrimEnd(new char[] { ' ' });
        print(finalAns);
    }

    private void RemoveChildren()
    {
        //Transform[] childTransforms = transform.GetComponentsInChildren<Transform>();
        int numChildren = transform.childCount;
        for(int i = 0; i < numChildren; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
