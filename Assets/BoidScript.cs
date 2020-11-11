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

    Color projectionColor = Color.white;

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
        transform.Translate(this.transform.forward * moveSpeed * Time.deltaTime, Space.World);
    }

    //  Manuevers the boid in the correct direction based on the good path found in check for obstacles
    void Turn(Vector3 direction)
    {
        //  Works but I need to increase the number of rays to (hopefully) create a smoother rotate
        Quaternion rotateTo = Quaternion.FromToRotation(this.transform.forward, direction);
        print(rotateTo);
        transform.localRotation *= Quaternion.Lerp(this.transform.rotation, rotateTo, Time.deltaTime);
    }

    //  Casts rays to check the surroundings of the boid, while taking in the local forward vector
    void CheckForObstacles()
    {
        Vector3 f = transform.forward;
        endOfBoid = transform.position + f;
        viewRays = new Ray[]{
            //new Ray(endOfBoid, f),                          //  Forward
            new Ray(endOfBoid, f - transform.right),        //  Forward Left
            new Ray(endOfBoid, f + transform.up),           //  Forward Up
            new Ray(endOfBoid, f + transform.right),        //  Forward Right
            new Ray(endOfBoid, f - transform.up),           //  Forward Down
            new Ray(endOfBoid, -transform.right),           //  Left
            new Ray(endOfBoid, transform.up),               //  Up
            new Ray(endOfBoid, transform.right),            //  Right
            new Ray(endOfBoid, -transform.up)               //  Down
        };

        if(Physics.SphereCast(new Ray(endOfBoid, f), 1f, 7))  //  Checks that the forward direction is safe up to 10 meters
        {
            StartCoroutine(ColorSwitch());
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

    private void OnDrawGizmos()
    {
        Gizmos.color = projectionColor;
        Gizmos.DrawWireMesh(CreateBoidMesh(), this.transform.localPosition + this.transform.forward*10, this.transform.rotation);
    }

    //  Creates and returns a boid mesh
    //  Currently makes a pyramid like object
    Mesh CreateBoidMesh()
    {
        boidMesh = new Mesh { name = "Boid Mesh" };

        Vector3[] vert = new Vector3[]
        {
            new Vector3(-.5f, 0, -.5f),
            new Vector3(0, .25f, -.5f),
            new Vector3(.5f, 0, -.5f),
            new Vector3(0, -.25f, -.5f),
            new Vector3(0, 0, .5f)
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

    IEnumerator ColorSwitch()
    {
        projectionColor = Color.red;
        yield return new WaitForSeconds(.45f);
        projectionColor = Color.white;
    }
}
