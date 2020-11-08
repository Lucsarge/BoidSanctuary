using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidScript : MonoBehaviour
{
    //  Necessary mesh components and collider
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh boidMesh;

    //  Ray variables
    Ray[] viewRays = new Ray[8];
    RaycastHit[] hits = new RaycastHit[8];  //  Forward, Left, Right, Up, Down
    Color hitColor = Color.green;
    Vector3 endOfBoid;

    //  Movement Variables
    float moveSpeed = 5;


    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        boidMesh = CreateBoidMesh();
        meshFilter.mesh = boidMesh;
        meshCollider.sharedMesh = boidMesh;

        

    }

    // Update is called once per frame
    void Update()
    {
        CheckForObstacles();
        Move();
    }

    void Move()
    {
        //  Inch forward
        transform.Translate(this.transform.forward * moveSpeed * Time.deltaTime);
    }

    //  Manuevers the boid in the correct direction based on the good path found in check for obstacles
    void Turn(Vector3 direction)
    {
        direction = direction.normalized;
        this.transform.Rotate(direction.x, direction.y, 0);
        //this.transform.Rotate(0,15*Time.deltaTime,0);
    }

    //  Casts rays to check the surroundings of the boid, while taking in the local forward vector
    void CheckForObstacles()
    {
        Vector3 f = transform.forward;
        endOfBoid = transform.position + f;
        viewRays = new Ray[]{
            //new Ray(endOfBoid, f),                          //  Forward
            new Ray(endOfBoid, f - transform.right),        //  Forward Left
            new Ray(endOfBoid, f + transform.right),        //  Forward Right
            new Ray(endOfBoid, f + transform.up),           //  Forward Up
            new Ray(endOfBoid, f - transform.up),           //  Forward Down
            new Ray(endOfBoid, -transform.right),           //  Left
            new Ray(endOfBoid, transform.right),            //  Right
            new Ray(endOfBoid, transform.up),               //  Up
            new Ray(endOfBoid, -transform.up)               //  Down
        };

        if(Physics.Raycast(new Ray(endOfBoid, f), 10))  //  Checks that the forward direction is safe up to 10 meters
        {
            for (int i = 0; i < 8; i++)
            {
                //hitColor = Physics.Raycast(viewRays[i], out hits[i], 10f) ? Color.red : Color.green; Turn(viewRays[i].direction);
                if (Physics.Raycast(viewRays[i], out hits[i], 10))
                {
                    hitColor = Color.red;
                    Debug.DrawRay(endOfBoid, viewRays[i].direction * 10f, hitColor);
                }   
                else
                {
                    hitColor = Color.green;
                    Turn(viewRays[i].direction);
                    Debug.DrawRay(endOfBoid, viewRays[i].direction * 10f, hitColor);
                    i = 9;
                    break;
                }
            }
        }

        
        
    }

    void ObstacleAvoidance()
    {

    }


    //  Creates and returns a boid mesh
    //  Currently makes a pyramid like object
    Mesh CreateBoidMesh()
    {
        boidMesh = new Mesh { name = "Boid Mesh" };

        Vector3[] vert = new Vector3[]
        {
            new Vector3(-.5f, 0, 0),
            new Vector3(0, .25f, 0),
            new Vector3(.5f, 0, 0),
            new Vector3(0, -.25f, 0),
            new Vector3(0, 0, 1)
        };

        int[] triang = new int[]
        {
            0,1,2,
            2,3,0,
            0,4,1,
            2,1,4,
            2,4,3,
            0,3,4
        };

        boidMesh.Clear();
        boidMesh.vertices = vert;
        boidMesh.triangles = triang;
        boidMesh.RecalculateNormals();

        return boidMesh;
    }
}
