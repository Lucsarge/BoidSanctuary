﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class BoidScript : MonoBehaviour
{
    private static Vector3[] fibDirections = new Vector3[]{
        new Vector3(0f, 0f, -1f),
        new Vector3(-0.01158065f, 0.01061171f, -0.9998766f),
        new Vector3(0.002737628f, -0.03129123f, -0.9995065f),
        new Vector3(0.0286766f, 0.03737205f, -0.9988899f),
        new Vector3(-0.06183658f, -0.01090346f, -0.9980267f),
        new Vector3(0.06617174f, -0.04215602f, -0.9969173f),
        new Vector3(-0.02435698f, 0.09090165f, -0.995562f),
        new Vector3(-0.05066969f, -0.0973355f, -0.993961f),
        new Vector3(0.1177747f, 0.04286648f, -0.9921147f),
        new Vector3(-0.1301758f, 0.05392054f, -0.9900237f),
        new Vector3(0.06611212f, -0.1417777f, -0.9876884f),
        new Vector3(0.05169996f, 0.1639717f, -0.9851093f),
        new Vector3(-0.1622771f, -0.09369048f, -0.9822872f),
        new Vector3(0.1979804f, -0.04389135f, -0.9792228f),
        new Vector3(-0.1251217f, 0.1786925f, -0.9759167f),
        new Vector3(-0.03047036f, -0.2314482f, -0.9723699f),
        new Vector3(0.1905075f, 0.1598548f, -0.9685832f),
        new Vector3(-0.2636219f, 0.01151043f, -0.9645574f),
        new Vector3(0.1972766f, -0.1972764f, -0.9602937f),
        new Vector3(-0.01282549f, 0.2937605f, -0.955793f),
        new Vector3(-0.1986321f, -0.2367209f, -0.9510565f),
        new Vector3(0.3211463f, 0.04227946f, -0.9460853f),
        new Vector3(-0.2774781f, 0.1942917f, -0.9408808f),
        new Vector3(0.07650585f, -0.3450961f, -0.9354441f),
        new Vector3(0.1840629f, 0.3188048f, -0.9297765f),
        new Vector3(-0.3649716f, -0.1150752f, -0.9238796f),
        new Vector3(0.359938f, -0.1678425f, -0.9177547f),
        new Vector3(-0.15748f, 0.3801896f, -0.9114033f),
        new Vector3(-0.1456255f, -0.4001015f, -0.9048271f),
        new Vector3(0.3902313f, 0.2031402f, -0.8980276f),
        new Vector3(-0.4385215f, 0.1175f, -0.8910065f),
        new Vector3(0.2514191f, -0.3946476f, -0.8837656f),
        new Vector3(0.08365569f, 0.4744347f, -0.8763067f),
        new Vector3(-0.3930743f, -0.3016154f, -0.8686315f),
        new Vector3(0.5071042f, -0.04436762f, -0.860742f),
        new Vector3(-0.3529959f, 0.3852254f, -0.8526402f),
        new Vector3(4.578135E-07f, -0.5358268f, -0.8443279f),
        new Vector3(0.3709148f, 0.4047818f, -0.8358074f),
        new Vector3(-0.5599446f, -0.0489874f, -0.8270805f),
        new Vector3(0.4561809f, -0.3500429f, -0.8181497f),
        new Vector3(-0.1020689f, 0.5788553f, -0.809017f),
        new Vector3(-0.3226055f, -0.5063893f, -0.7996847f),
        new Vector3(0.592023f, 0.1586311f, -0.7901551f),
        new Vector3(-0.5545961f, 0.2887067f, -0.7804304f),
        new Vector3(0.2180135f, -0.598982f, -0.7705132f),
        new Vector3(0.2485324f, 0.600012f, -0.760406f),
        new Vector3(-0.5993522f, -0.2794821f, -0.750111f),
        new Vector3(0.641863f, -0.2023802f, -0.7396311f),
        new Vector3(-0.3422712f, 0.5928365f, -0.7289687f),
        new Vector3(-0.1506217f, -0.6794171f, -0.7181264f),
        new Vector3(0.5792279f, 0.40558f, -0.7071068f),
        new Vector3(-0.7119825f, 0.09373524f, -0.6959128f),
        new Vector3(0.4685703f, -0.5584238f, -0.6845471f),
        new Vector3(0.03226571f, 0.7389269f, -0.6730126f),
        new Vector3(-0.5304079f, -0.5304093f, -0.6613119f),
        new Vector3(0.7596822f, 0.03316815f, -0.649448f),
        new Vector3(-0.5902463f, 0.4952775f, -0.6374241f),
        new Vector3(0.1018637f, -0.7737541f, -0.6252427f),
        new Vector3(0.4532178f, 0.6472546f, -0.6129071f),
        new Vector3(-0.7807302f, -0.1730779f, -0.6004202f),
        new Vector3(0.7006319f, -0.4045039f, -0.5877853f),
        new Vector3(-0.2460261f, 0.780282f, -0.5750053f),
        new Vector3(-0.349537f, -0.7495906f, -0.5620834f),
        new Vector3(0.7721848f, 0.3198508f, -0.5490229f),
        new Vector3(-0.7934086f, 0.2887773f, -0.5358269f),
        new Vector3(0.393704f, -0.7563018f, -0.5224986f),
        new Vector3(0.2227793f, 0.8314121f, -0.5090415f),
        new Vector3(-0.7325988f, -0.4667116f, -0.4954587f),
        new Vector3(0.8629925f, -0.152175f, -0.4817537f),
        new Vector3(-0.5379964f, 0.701143f, -0.4679298f),
        new Vector3(-0.07765185f, -0.8876164f, -0.4539905f),
        new Vector3(0.6620933f, 0.6067009f, -0.4399392f),
        new Vector3(-0.9048271f, -1.546179E-06f, -0.4257793f),
        new Vector3(0.671957f, -0.6157351f, -0.4115144f),
        new Vector3(-0.0799861f, 0.9142625f, -0.3971479f),
        new Vector3(-0.5624247f, -0.7329611f, -0.3826834f),
        new Vector3(0.9156519f, 0.1614494f, -0.3681245f),
        new Vector3(-0.7889422f, 0.502619f, -0.3534748f),
        new Vector3(0.2435103f, -0.908823f, -0.338738f),
        new Vector3(0.4368492f, 0.8391902f, -0.3239175f),
        new Vector3(-0.8936996f, -0.3252837f, -0.3090171f),
        new Vector3(0.8830383f, -0.3657644f, -0.2940404f),
        new Vector3(-0.4058378f, 0.8703215f, -0.2789912f),
        new Vector3(-0.2900493f, -0.9199143f, -0.2638731f),
        new Vector3(0.8388191f, 0.484289f, -0.2486899f),
        new Vector3(-0.9493199f, 0.2104639f, -0.2334454f),
        new Vector3(0.5597577f, -0.7994278f, -0.2181433f),
        new Vector3(0.1278221f, 0.9708444f, -0.2027873f),
        new Vector3(-0.7524722f, -0.6314062f, -0.1873813f),
        new Vector3(0.9841719f, -0.04296612f, -0.1719291f),
        new Vector3(-0.6984026f, 0.6983997f, -0.1564345f),
        new Vector3(0.04318469f, -0.9890813f, -0.1409013f),
        new Vector3(0.63772f, 0.7600032f, -0.1253332f),
        new Vector3(-0.9854579f, -0.1297351f, -0.1097343f),
        new Vector3(0.815514f, -0.5710346f, -0.09410831f),
        new Vector3(-0.2157663f, 0.9732878f, -0.0784592f),
        new Vector3(-0.4990202f, -0.8643125f, -0.06279062f),
        new Vector3(0.9526565f, 0.3003774f, -0.04710655f),
        new Vector3(-0.9058623f, 0.422406f, -0.03141085f),
        new Vector3(0.3826384f, -0.9237646f, -0.0157074f),
        new Vector3(0.3420195f, 0.9396929f, -7.54979E-08f),
        new Vector3(-0.8869019f, -0.4616908f, 0.01570725f),
        new Vector3(0.9654485f, -0.2586939f, 0.0314107f),
        new Vector3(-0.5366995f, 0.8424575f, 0.04710639f),
        new Vector3(-0.1733114f, -0.9828634f, 0.06279047f),
        new Vector3(0.7909123f, 0.6068788f, 0.07845905f),
        new Vector3(-0.9917727f, 0.08677822f, 0.09410828f),
        new Vector3(0.6715022f, -0.7328323f, 0.1097343f),
        new Vector3(-2.543005E-06f, 0.9921147f, 0.1253332f),
        new Vector3(-0.6688608f, -0.7299124f, 0.1409012f),
        new Vector3(0.98393f, 0.08608194f, 0.1564344f),
        new Vector3(-0.7815474f, 0.5996866f, 0.1719291f),
        new Vector3(0.1705684f, -0.9673648f, 0.1873812f),
        new Vector3(0.5261282f, 0.8258731f, 0.2027872f),
        new Vector3(-0.9426652f, -0.2525788f, 0.2181431f),
        new Vector3(0.8625054f, -0.4489852f, 0.2334453f),
        new Vector3(-0.3312651f, 0.910174f, 0.2486898f),
        new Vector3(-0.3691177f, -0.8911359f, 0.263873f),
        new Vector3(0.8703274f, 0.4058253f, 0.278991f),
        new Vector3(-0.9115558f, 0.287413f, 0.2940403f),
        new Vector3(0.475539f, -0.823633f, 0.3090169f),
        new Vector3(0.204774f, 0.9236586f, 0.3239174f),
        new Vector3(-0.7707192f, -0.5396745f, 0.3387379f),
        new Vector3(0.9274403f, -0.1221067f, 0.3534748f),
        new Vector3(-0.5976533f, 0.7122464f, 0.3681245f),
        new Vector3(-0.04030884f, -0.9229998f, 0.3826834f),
        new Vector3(0.6489486f, 0.6489525f, 0.3971479f),
        new Vector3(-0.9105364f, -0.03974216f, 0.4115143f),
        new Vector3(0.6931376f, -0.5816119f, 0.4257792f),
        new Vector3(-0.1172007f, 0.8903469f, 0.4399391f),
        new Vector3(-0.5110631f, -0.729868f, 0.4539904f),
        new Vector3(0.862815f, 0.1912904f, 0.4679297f),
        new Vector3(-0.7589008f, 0.4381587f, 0.4817536f),
        new Vector3(0.261208f, -0.8284269f, 0.4954586f),
        new Vector3(0.3637734f, 0.7800935f, 0.5090413f),
        new Vector3(-0.7877358f, -0.3262938f, 0.5224985f),
        new Vector3(0.7934048f, -0.2887881f, 0.5358267f),
        new Vector3(-0.3859328f, 0.7413703f, 0.5490227f),
        new Vector3(-0.2140778f, -0.7988949f, 0.5620833f),
        new Vector3(0.6900221f, 0.4395891f, 0.5750052f),
        new Vector3(-0.7967277f, 0.1404763f, 0.5877852f),
        new Vector3(0.4868129f, -0.6344358f, 0.6004202f),
        new Vector3(0.06886123f, 0.7871488f, 0.612907f),
        new Vector3(-0.575399f, -0.5272453f, 0.6252427f),
        new Vector3(0.7705132f, 2.633324E-06f, 0.637424f),
        new Vector3(-0.5606231f, 0.5137304f, 0.649448f),
        new Vector3(0.06537655f, -0.7472567f, 0.6613119f),
        new Vector3(0.4502688f, 0.5867812f, 0.6730125f),
        new Vector3(-0.7178944f, -0.1265817f, 0.6845471f),
        new Vector3(0.6056656f, -0.3858427f, 0.6959128f),
        new Vector3(-0.1830082f, 0.6830139f, 0.7071068f),
        new Vector3(-0.3213325f, -0.6172844f, 0.7181263f),
        new Vector3(0.6432661f, 0.2341225f, 0.7289686f),
        new Vector3(-0.6217834f, 0.2575485f, 0.7396311f),
        new Vector3(0.2794745f, -0.5993558f, 0.7501111f),
        new Vector3(0.1952927f, 0.6193898f, 0.7604058f),
        new Vector3(-0.5520308f, -0.3187028f, 0.7705131f),
        new Vector3(0.6104217f, -0.1353292f, 0.7804303f),
        new Vector3(-0.3515544f, 0.5020605f, 0.7901549f),
        new Vector3(-0.07837439f, -0.5952832f, 0.7996845f),
        new Vector3(0.450267f, 0.3778244f, 0.8090169f),
        new Vector3(-0.5744579f, 0.02508698f, 0.8181496f),
        new Vector3(0.3974546f, -0.3974515f, 0.8270805f),
        new Vector3(-0.02394087f, 0.5485007f, 0.8358073f),
        new Vector3(-0.3444227f, -0.4104674f, 0.8443279f),
        new Vector3(0.5180297f, 0.06819125f, 0.8526401f),
        new Vector3(-0.4169816f, 0.2919754f, 0.860742f),
        new Vector3(0.1072421f, -0.4837133f, 0.8686315f),
        new Vector3(0.2408795f, 0.4172095f, 0.8763067f),
        new Vector3(-0.4462716f, -0.1407125f, 0.8837656f),
        new Vector3(0.4114534f, -0.1918686f, 0.8910065f),
        new Vector3(-0.1683591f, 0.4064502f, 0.8980275f),
        new Vector3(-0.1456303f, -0.4000998f, 0.904827f),
        new Vector3(0.3650176f, 0.1900165f, 0.9114032f),
        new Vector3(-0.3836138f, 0.1027957f, 0.9177546f),
        new Vector3(0.2056148f, -0.3227525f, 0.9238795f),
        new Vector3(0.06392019f, 0.3625326f, 0.9297765f),
        new Vector3(-0.2804317f, -0.2151802f, 0.935444f),
        new Vector3(0.3374492f, -0.02952039f, 0.9408808f),
        new Vector3(-0.2188332f, 0.238819f, 0.9460853f),
        new Vector3(1.32013E-06f, -0.309017f, 0.9510565f),
        new Vector3(0.1986535f, 0.2167868f, 0.955793f),
        new Vector3(-0.2779295f, -0.02431594f, 0.9602937f),
        new Vector3(0.209342f, -0.1606391f, 0.9645574f),
        new Vector3(-0.04318396f, 0.2449118f, 0.9685832f),
        new Vector3(-0.1254279f, -0.1968872f, 0.9723699f),
        new Vector3(0.2107105f, 0.05645841f, 0.9759167f),
        new Vector3(-0.1798753f, 0.09363534f, 0.9792228f),
        new Vector3(0.06408658f, -0.1760814f, 0.9822872f),
        new Vector3(0.06579369f, 0.158842f, 0.9851093f),
        new Vector3(-0.1417788f, -0.06611038f, 0.9876883f),
        new Vector3(0.1343801f, -0.04236972f, 0.9900236f),
        new Vector3(-0.062665f, 0.1085429f, 0.9921147f),
        new Vector3(-0.02375113f, -0.1071333f, 0.9939609f),
        new Vector3(0.07708856f, 0.05397929f, 0.995562f),
        new Vector3(-0.07778799f, 0.01024142f, 0.9969173f),
        new Vector3(0.04036147f, -0.04810014f, 0.9980267f),
        new Vector3(0.00205518f, 0.04706177f, 0.9988899f),
        new Vector3(-0.02221077f, -0.02221098f, 0.9995065f),
        new Vector3(0.01569253f, 0.0006849585f, 0.9998766f),
    };

    // Necessary mesh components and collider
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Mesh boidMesh;

    public float ViewDist = 5.0f;
    Color hitColor = Color.green;
    Color projectionColor = Color.white;

    public void InstantiateMesh(Mesh meshInstance)
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        boidMesh = meshInstance;
        meshFilter.mesh = boidMesh;
    }

    public void Move(float moveSpeed)
    {
        // Inch forward
        transform.Translate(this.transform.forward * moveSpeed * Time.fixedDeltaTime * moveSpeed, Space.World);
        //RegulateBoundaries();
    }

    private void RegulateBoundaries()
    {
        Vector3 currPos = this.transform.position;
        //Vector3 newPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        Vector3 newPos = this.transform.position;

        if (currPos.x > 25.0f) { newPos.x -= 50.0f; }
        else if (currPos.x < -25.0f) { newPos.x += 50.0f; }

        if (currPos.y > 50.0f) { newPos.y -= 50.0f; }
        else if (currPos.y < 0) { newPos.y += 50.0f; }

        if (currPos.z > 25.0f) { newPos.z -= 50.0f; }
        else if (currPos.z < -25.0f) { newPos.z += 50.0f; }

        this.transform.position = newPos;
    }

    // Manuevers the boid in the correct direction based on the good path found in check for obstacles
    private void Turn(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(this.transform.position + direction, Vector3.up);
        //print($"Boid look rotation: {lookRotation}");

        this.transform.LookAt(this.transform.position + direction);
    }

    public bool IsForwardIsClear()
    {
        Vector3 f = transform.forward;

        RaycastHit hitInfo;
        bool isClear = !Physics.SphereCast(this.transform.position, 1.0f, this.transform.forward, out hitInfo, ViewDist, 1 << 7);
        Debug.DrawRay(this.gameObject.transform.position, f * ViewDist, isClear ? Color.green : Color.red);

        return isClear;
    }

    // temporary debug variable for watching the behavior of the boid looking for a new direction
    public bool isChecking = false;
    // Casts rays to check the surroundings of the boid, while taking in the local forward vector
    public void ObtainNewDir()
    {
        isChecking = true;
        print("Checking");

        Vector3 newDir = -this.transform.forward; // default the new direction to behind the boid
        print("Drawing \"backward\" ray");
        print(this.transform.forward);
        print(newDir);
        Debug.DrawRay(this.transform.position, newDir, Color.yellow);

        Vector3 f = transform.forward;

        // indicates the boid has started searching for a new path
        // StartCoroutine(ColorSwitch(Color.red, .45f));
        float debugRayTime = 0.5f;
        foreach (Vector3 fibDirection in fibDirections)
        {
            if (Physics.Raycast(this.transform.position, -fibDirection, ViewDist + 2.0f))
            {
                hitColor = Color.red;
                Debug.DrawRay(this.transform.position, -fibDirection * (ViewDist + 2.0f), hitColor, debugRayTime);
            }
            else
            {
                // success, the raycast didn't hit anything so the path is clear
                hitColor = Color.green;
                Debug.DrawRay(this.transform.position, -fibDirection * (ViewDist + 2.0f), hitColor, 10.0f);
                newDir = -fibDirection;
                break;
            }
            //yield return new WaitForSeconds(debugRayTime);
        }
        Turn(newDir);
        isChecking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = projectionColor;
        Gizmos.DrawWireMesh(boidMesh, this.transform.position, this.transform.rotation);
    }

    // changes the color of the boid debug look ray to make some visual indication for a process that will likely be too short to see in real time
    // duration should be at least 0.0f
    private IEnumerator ColorSwitch(Color newColor, float duration = 1.0f)
    {
        projectionColor = newColor;
        yield return new WaitForSeconds(duration);
        projectionColor = Color.white; // returns to default color
    }

    [SerializeField]
    private bool isViewRaysShowing = false;
    void OnValidate()
    {
        if (isViewRaysShowing){
            DisplayRays();
        }
    }

    private void DisplayRays(){
        for(int i = 0; i < BoidViewRays.ViewRays.Length; i++){
            Debug.DrawLine(this.transform.position, BoidViewRays.ViewRays[i], (i > 800) ? Color.red : Color.green, 5.0f);
        }
    }
}
