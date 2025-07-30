using UnityEditor;
using UnityEngine;

public class FlockTool : EditorWindow
{
    #region Camera Controls
    [SerializeField]
    private Camera cameraRef;

    public enum CameraState
    {
        NORTH = 0,
        EAST,
        SOUTH,
        WEST,
        TOP,
        CENTER,
    }

    [SerializeField]
    [HideInInspector]
    private float cameraRotateValue = 0.0f;

    [SerializeField]
    [HideInInspector]
    private float cameraHeightValue = 0.0f;

    [SerializeField]
    [HideInInspector]
    private CameraState cameraState = CameraState.NORTH;

    [SerializeField]
    private Transform cameraTarget;

    [SerializeField]
    private bool didCameraPivotChange = false;
    #endregion

    #region Editor Window Functions
    [MenuItem("Tools/Boids/Flock Tool")]
    public static void ShowWindow()
    {
        FlockTool wnd = GetWindow<FlockTool>();
        wnd.titleContent = new GUIContent("Flock Tool");
        wnd.Show();
    }

    // TODO: Ensure that when openeing a new scene the state of the camera is setup

    private void OnEnable()
    {
        Debug.Log("Flock Tool enabled");
    }

    private void OnDisable()
    {
        Debug.Log("Flock Tool disabled");
    }

    void OnGUI()
    {
        // TODO: Camera Reference and Camera Target are not saved and get cleared whenever the applications is opened or scenes change
        cameraRef = EditorGUILayout.ObjectField("Camera", cameraRef, typeof(Camera), true) as Camera;
        cameraTarget = EditorGUILayout.ObjectField("Camera Target", cameraTarget, typeof(Transform), true) as Transform;

        CameraState previousCameraState = cameraState;
        cameraState = (CameraState)EditorGUILayout.EnumPopup("Camera State", cameraState);
        if (previousCameraState != cameraState)
        {
            UpdateCameraState();
        }

        if (cameraState == CameraState.CENTER)
        {
            // Camera Rotation Slider
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Camera Rotation:");
                float newCamRotateValue = EditorGUILayout.Slider(cameraRotateValue, 0, 360);
                if (cameraRotateValue != newCamRotateValue)
                {
                    didCameraPivotChange = true;
                    Undo.RecordObject(this, "camera pivot rotation");
                    cameraRotateValue = newCamRotateValue;
                }
            }

            // Camera Height Slider
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Camera Height:");
                float newCamHeightValue = EditorGUILayout.Slider(cameraHeightValue, 0, 10);
                if (cameraHeightValue != newCamHeightValue)
                {
                    didCameraPivotChange = true;
                    Undo.RecordObject(this, "camera pivot height");
                    cameraHeightValue = newCamHeightValue;
                }
            }

            if (didCameraPivotChange)
            {
                UpdateCameraState();
            }
        }

        if (GUILayout.Button("Update Cam State"))
        {
            // update the camera position
            UpdateCameraState();
        }
    }
    #endregion

    #region Camera Functions
    public void UpdateCameraState()
    {
        didCameraPivotChange = false;
        Debug.Log("Updating camera state");
        switch (cameraState)
        {
            case CameraState.NORTH:
                cameraRef.transform.position = cameraTarget.position + new Vector3(0, 0, 50);
                break;
            case CameraState.EAST:
                cameraRef.transform.position = cameraTarget.position + new Vector3(50, 0, 0);
                break;
            case CameraState.SOUTH:
                cameraRef.transform.position = cameraTarget.position + new Vector3(0, 0, -50);
                break;
            case CameraState.WEST:
                cameraRef.transform.position = cameraTarget.position + new Vector3(-50, 0, 0);
                break;
            case CameraState.TOP:
                cameraRef.transform.position = cameraTarget.position + new Vector3(0, 25, 0);
                break;
            case CameraState.CENTER:
                cameraRef.transform.position = cameraTarget.position + new Vector3(0, cameraHeightValue, -50);
                cameraRef.transform.RotateAround(cameraTarget.transform.position, new Vector3(0, 1, 0), -cameraRotateValue);
                break;
        }

        Vector3 relativePos = cameraTarget.position - cameraRef.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        cameraRef.transform.rotation = rotation;
    }
    #endregion
}
