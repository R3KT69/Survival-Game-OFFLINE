using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigShifting : MonoBehaviour
{
    [Header("Rigs")]
    public Rig rigA; // always active rig
    public Rig rigB; // aim rig
    public Camera playerCamera;
    public MultiRotationConstraint neckConstraint;

    [Header("Settings")]
    [Tooltip("Key to switch aiming (toggle).")]
    public KeyCode switchKey = KeyCode.Mouse1; // right mouse button

    [Tooltip("How fast the rig blends (weight per second).")]
    public float blendSpeed = 2f;

    [Tooltip("FOV for aiming.")]
    public float aimFOV = 50f;

    [Tooltip("Default camera FOV.")]
    public float defaultFOV = 90f;

    public bool isAiming = false;

    void Update()
    {
        // Toggle aiming state
        if (Input.GetKeyDown(switchKey))
        {
            isAiming = !isAiming;
        }

        // Always keep rigA active
        if (rigA != null)
            rigA.weight = 1f;

        if (rigB != null)
        {
            float targetB = isAiming ? 1f : 0f;
            rigB.weight = Mathf.MoveTowards(rigB.weight, targetB, blendSpeed * Time.deltaTime);
            
            Vector3 targetOffset = isAiming ? new Vector3(0f, 0f, -30f) : Vector3.zero;
            neckConstraint.data.offset = Vector3.Lerp(neckConstraint.data.offset, targetOffset, Time.deltaTime * 8f);

        }

        // Smoothly blend camera FOV
        if (playerCamera != null)
        {
            float targetFOV = isAiming ? aimFOV : defaultFOV;
            playerCamera.fieldOfView = Mathf.MoveTowards(playerCamera.fieldOfView, targetFOV, blendSpeed * 40f * Time.deltaTime);
        }
    }
}
