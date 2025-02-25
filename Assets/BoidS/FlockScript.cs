using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlockScript : MonoBehaviour
{
    [SerializeField]
    bool simInProgress = false;
    
    [SerializeField]
    List<BoidScript> boidScripts;

    private Mesh boidMesh;
    [SerializeField]
    private GameObject boidObject;

    // Boid Properties
    [SerializeField]
    [Range(0.0f, 5.0f)]
    float boidMoveSpeed = 3.0f;

    [SerializeField]
    private float boidSeparationRadius = 10.0f;

    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float separationStrength = 0.5f;

    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float alignmentStrength = 0.5f;

    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float coherenceStrength = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // create boid mesh
        boidMesh = CreateBoidMesh();

        //GatherFlock();
        CreateFlock(100);
    }

    private void GatherFlock(){
        boidScripts = new List<BoidScript>();
        for (int i = 0; i < gameObject.transform.childCount; i++){
            var child = gameObject.transform.GetChild(i);
            if (child.CompareTag("Boid")){
                print("accquire boid");
                BoidScript boidInstance = child.gameObject.GetComponent<BoidScript>();
                boidInstance.InstantiateMesh(boidMesh);
                boidScripts.Add(boidInstance);
            }
            else{
                print("not a boid");
            }
        }
    }

    private void CreateFlock(int numOfBoids){
        if (numOfBoids <= 0){
            return;
        }

        boidScripts = new List<BoidScript>();
        for (int i = 1; i <= numOfBoids; i++){
            GameObject boid = Instantiate(boidObject, this.transform.position, UnityEngine.Random.rotation);
            BoidScript boidScriptInstance = boid.GetComponent<BoidScript>();
            boidScriptInstance.InstantiateMesh(boidMesh);
            boidScripts.Add(boidScriptInstance);
            print($"Creating boid: {i}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // handle input for managing flock
        if (Input.GetKeyDown(KeyCode.Space)){
            simInProgress = !simInProgress;
        }

        if (simInProgress){
            // allow boid flock to move
            foreach (var child in boidScripts){
                if (!child.isChecking){ // ignore it while it's doing it's checking, remove this later or change it to a setting that can be turned on or off
                    // if (!child.IsForwardIsClear()){
                    //     child.ObtainNewDir();
                    // }

                    #region Steering Behaviors
                    // Align calculate rotation
                    Vector3 avgVecFromBoid = new Vector3();
                    Vector3 avgVelocity = new Vector3();
                    Vector3 coherePos = new Vector3();

                    int numOfFlockmates = 0; // total number of boids that appear within the boids view raidus
                    int numOfCrowd = 0; // total number of flockmates that are close enough to need separation
                    // get nearby boids
                    foreach (BoidScript flockmate in boidScripts) {
                        if (flockmate == child) { continue; } // skip the current boid that you're working on

                        float dist = Vector3.Distance(flockmate.transform.position, child.transform.position);
                        if (dist < flockmate.ViewDist) {
                            numOfFlockmates++;

                            if (dist < boidSeparationRadius) {
                                numOfCrowd++;
                                avgVecFromBoid += (child.transform.position - flockmate.transform.position);
                            }
                            avgVelocity += flockmate.transform.forward;
                            coherePos += flockmate.transform.position;
                        }
                    }

                    // Apply steering behaviors
                    if (numOfFlockmates != 0) {
                        Vector3 steeringVector = new Vector3(); // vector that will capture the sum of all the steering behavior adjustments

                        // Separation
                        if (numOfCrowd > 0) {
                            Vector3 separationVec;
                            avgVecFromBoid /= numOfFlockmates; // avg vector from boid to flockmates
                            separationVec = avgVecFromBoid.normalized * separationStrength;
                            steeringVector += separationVec;
                        }

                        // Alignment
                        Vector3 alignmentVec;
                        avgVelocity /= numOfFlockmates;
                        alignmentVec = (avgVelocity.normalized - child.transform.forward) * alignmentStrength;
                        steeringVector += alignmentVec;

                        // Cohesion
                        Vector3 coherenceVec;
                        coherePos /= numOfFlockmates; // avg position of flockmates
                        Vector3 endOfVelocity = child.transform.position + child.transform.forward; // get the point from the boids position to the end of its velocity
                        coherenceVec = (coherePos - endOfVelocity) * coherenceStrength;
                        steeringVector += coherenceVec;

                        // set the new velocity to the normalized sum of each steering behavior vectors multiplied by delta time
                        child.transform.forward = (child.transform.forward + (steeringVector * Time.deltaTime)).normalized;
                    }
                    #endregion

                    child.Move(boidMoveSpeed);
                }
            }

            //simInProgress = false; // currently this is stopping the simulation progress after a single step for testing
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
