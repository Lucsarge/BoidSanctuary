using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDataScript : MonoBehaviour
{
    public float theta;
    public float r;
    public float angle;
    public float x;
    public float y;
    public float z;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues(float theta, float r, float angle, float x, float y, float z)
    {
        this.theta = (theta * Mathf.Rad2Deg)%360;
        this.r = r;
        this.angle = angle;
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
