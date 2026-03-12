using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Crouching : MonoBehaviour
{
    public CharacterController characterController;
    public Rig Crouchrig;
    public List<MultiRotationConstraint> Spine;

    [Header("Crouch Settings")]
    public float crouchHeightFactor = 0.4f; // character height when crouched
    public Vector3 crouchOffset = new Vector3(45, 0, 0); // offset for spine
    public float transitionSpeed = 5f; // higher = faster transition

    private float originalHeight;
    public bool isCrouching = false;

    // Targets
    private float targetHeight;
    private float currentWeight;
    private Vector3 targetOffset;
    private Vector3 currentOffset;

    void Start()
    {
        originalHeight = characterController.height;
        targetHeight = originalHeight;
        targetOffset = Vector3.zero;
        currentOffset = Vector3.zero;
        currentWeight = Crouchrig.weight;
    }

    void Update()
    {
        // Toggle crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            if (isCrouching)
            {
                targetHeight = originalHeight * crouchHeightFactor;
                targetOffset = crouchOffset;
            }
            else
            {
                targetHeight = originalHeight;
                targetOffset = Vector3.zero;
            }
        }

        // Smoothly lerp rig weight
        Crouchrig.weight = Mathf.Lerp(Crouchrig.weight, isCrouching ? 1f : 0f, Time.deltaTime * transitionSpeed);

        // Smoothly lerp character controller height
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * transitionSpeed);

        // Smoothly lerp spine offset
        for (int i = 0; i < Spine.Count; i++)
        {
            var constraint = Spine[i];
            var data = constraint.data;
            
            Vector3 offsetTarget = (i == 2) ? targetOffset : Vector3.zero; // only apply offset to Spine[2]
            data.offset = Vector3.Lerp(data.offset, offsetTarget, Time.deltaTime * transitionSpeed);

            constraint.data = data;
        }
    }
}