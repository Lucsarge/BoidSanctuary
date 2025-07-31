using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FlockScript : MonoBehaviour
{
    [SerializeField]
    private bool isSimInProgress = false;

    [SerializeField]
    private List<BoidScript> boidScripts;

    [SerializeField]
    private Mesh boidMesh;

    [SerializeField]
    private GameObject boidPrefab;

    [SerializeField]
    private bool isSteeringEnabled = false;

    [SerializeField]
    private bool isAvoidanceEnabled = false;

    [SerializeField]
    private LayerMask obstacleLayerMask = 1 << 7;
    private float obstacleAvoidanceRadius = 1.0f;

    // Boid Properties
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float boidMoveSpeed = 3.0f;

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

    public const float SIMULATION_TIME = 0.0166f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Time.fixedDeltaTime = SIMULATION_TIME;

        // obtain the boid prefab
        if (boidPrefab == null)
        {
            boidPrefab = Resources.Load<GameObject>("Boid");
        }

        // obtain the boid mesh
        if (boidMesh == null)
        {
            boidMesh = Resources.Load<Mesh>("BoidMesh");
        }

        GatherFlock();
        CreateFlock(50);
    }

    private void GatherFlock()
    {
        boidScripts = new List<BoidScript>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var child = gameObject.transform.GetChild(i);
            if (child.CompareTag("Boid"))
            {
                print("accquire boid");
                BoidScript boidInstance = child.gameObject.GetComponent<BoidScript>();
                boidInstance.InstantiateMesh(boidMesh);
                boidScripts.Add(boidInstance);
            }
            else
            {
                print("not a boid");
            }
        }
    }

    private void CreateFlock(int numOfBoids)
    {
        if (numOfBoids <= 0)
        {
            return;
        }

        boidScripts = new List<BoidScript>();
        for (int i = 1; i <= numOfBoids; i++)
        {
            GameObject boid = Instantiate(boidPrefab, this.transform.position, UnityEngine.Random.rotation);
            BoidScript boidScriptInstance = boid.GetComponent<BoidScript>();
            boidScriptInstance.InstantiateMesh(boidMesh);
            boidScripts.Add(boidScriptInstance);
            print($"Creating boid: {i}");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ScreenCapture.CaptureScreenshot("Boids Screenshot.png");
            print("Screenshot saved");
        }

        // handle input for managing flock
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSimInProgress = !isSimInProgress;
        }
    }

    private void FixedUpdate()
    {
        if (isSimInProgress)
        {
            Simulate();
        }
    }

    // Simulate with a time step in seconds
    public void Simulate(float timeStep = 0)
    {
        if (timeStep == 0)
        {
            timeStep = Time.fixedDeltaTime;
        }
        // allow boid flock to move
        foreach (var child in boidScripts)
        {
            if (!child.isChecking)
            { // ignore it while it's doing it's checking, remove this later or change it to a setting that can be turned on or off
              // if (!child.IsForwardIsClear()){
              //     child.ObtainNewDir();
              // }

                if (isSteeringEnabled)
                {
                    #region Steering Behaviors
                    // Align calculate rotation
                    Vector3 avgVecFromBoid = new Vector3();
                    Vector3 avgVelocity = new Vector3();
                    Vector3 coherePos = new Vector3();

                    int numOfFlockmates = 0; // total number of boids that appear within the boids view raidus
                    int numOfCrowd = 0; // total number of flockmates that are close enough to need separation
                                        // get nearby boids
                    foreach (BoidScript flockmate in boidScripts)
                    {
                        if (flockmate == child) { continue; } // skip the current boid that you're working on

                        float dist = Vector3.Distance(flockmate.transform.position, child.transform.position);
                        if (dist < flockmate.ViewDist)
                        {
                            numOfFlockmates++;

                            if (dist < boidSeparationRadius)
                            {
                                numOfCrowd++;
                                avgVecFromBoid += (child.transform.position - flockmate.transform.position);
                            }
                            avgVelocity += flockmate.transform.forward;
                            coherePos += flockmate.transform.position;
                        }
                    }

                    // Apply steering behaviors
                    if (numOfFlockmates != 0)
                    {
                        Vector3 steeringVector = new Vector3(); // vector that will capture the sum of all the steering behavior adjustments

                        // Separation
                        if (numOfCrowd > 0)
                        {
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
                        child.transform.forward = (child.transform.forward + (steeringVector * timeStep)).normalized;
                    }
                    #endregion
                }

                if (isAvoidanceEnabled)
                {
                    if (!child.IsForwardIsClear())
                    {
                        float furthestClearDist = 0.0f;
                        Vector3 newDirection = child.transform.forward;
                        for (int i = 0; i < BoidViewRays.ViewRays.Length; i = i + 100)
                        {
                            RaycastHit hitInfo;
                            Vector3 testingDirection = child.transform.TransformDirection(BoidViewRays.ViewRays[i]);
                            //Debug.DrawRay(child.transform.position, testingDirection * child.ViewDist, Color.green, 0.3f);
                            if (Physics.SphereCast(child.transform.position, obstacleAvoidanceRadius, testingDirection, out hitInfo, child.ViewDist, obstacleLayerMask))
                            {
                                if (hitInfo.distance > furthestClearDist)
                                {
                                    newDirection = testingDirection;
                                    furthestClearDist = hitInfo.distance;
                                }
                            }
                            else
                            {
                                newDirection = testingDirection;
                                //Debug.DrawRay(child.transform.position, testingDirection * child.ViewDist, Color.red, 1.0f);
                                print("Found clear path");
                                print(testingDirection);
                                break;
                            }
                        }
                        Vector3 newLookAtDirection = Vector3.Lerp(child.transform.forward, newDirection, .25f).normalized;
                        child.transform.LookAt(child.transform.position + newLookAtDirection);
                        //Quaternion newRot = Quaternion.Euler(newDirection);
                        //print($"New Rot: {newRot}");
                        //child.transform.rotation = Quaternion.Lerp(child.transform.rotation, newRot, 1f);
                    }
                }

                child.Move(boidMoveSpeed);
            }
        }
    }
}
