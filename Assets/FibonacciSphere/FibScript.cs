using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FibScript : MonoBehaviour
{
    public int experience;
    //public GameObject sphereObject = Resources.Load();
    public int N;

    string fibSeq;

    public int Level
    {
        get { return experience / 750; }
    }

    public void DoNothing()
    {
        print("Nothing");
    }

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
