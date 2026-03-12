using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Camera to orbit around. If null, Camera.main is used.")]
    public Transform cameraTransform;

    [Header("Orbit Settings")]
    [Tooltip("Distance forward from the camera.")]
    public float forwardDistance = 1.5f;

    [Tooltip("Vertical offset from the camera.")]
    public float heightOffset = 0.0f;

    [Tooltip("Side offset (left/right) from the camera.")]
    public float sideOffset = 0.0f;

    [Header("Behaviour")]
    [Tooltip("Optional smoothing factor (0 = instant).")]
    public float smooth = 0.0f;

    [Tooltip("Maximum pitch range (clamped).")]
    public float pitchClamp = 0f;

    private float initialPitch;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null)
            {
                Debug.LogWarning("OrbitByCameraPitch: cameraTransform not set and no Camera.main found.");
                enabled = false;
                return;
            }
        }

        initialPitch = NormalizeAngle(cameraTransform.localEulerAngles.x);
    }

    void Update()
    {
        if (!cameraTransform) return;

        // Get camera pitch
        float currentPitch = NormalizeAngle(cameraTransform.localEulerAngles.x);
        float deltaPitch = Mathf.Clamp(currentPitch - initialPitch, -pitchClamp, pitchClamp);

        // Create local offset from camera based on public distances
        Vector3 localOffset = new Vector3(sideOffset, heightOffset, forwardDistance);

        // Apply rotation only around X axis (pitch)
        Quaternion rot = Quaternion.AngleAxis(deltaPitch, Vector3.right);
        Vector3 rotatedLocal = rot * localOffset;

        // Prevent going behind the camera
        if (rotatedLocal.z < 0f)
            rotatedLocal.z = Mathf.Abs(rotatedLocal.z);

        // Convert to world space
        Vector3 worldPos = cameraTransform.TransformPoint(rotatedLocal);

        // Apply smoothly or instantly
        if (smooth > 0f)
            transform.position = Vector3.Lerp(transform.position, worldPos, Time.deltaTime * smooth);
        else
            transform.position = worldPos;

        // Keep upright
        //transform.rotation = Quaternion.identity;
    }

    static float NormalizeAngle(float a)
    {
        if (a > 180f) a -= 360f;
        return a;
    }
}
