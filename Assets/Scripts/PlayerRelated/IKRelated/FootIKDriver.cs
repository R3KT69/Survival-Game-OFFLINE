using UnityEngine;

public class FootIKDriver : MonoBehaviour
{
    [Header("Bones & Targets")]
    public Transform leftFootBone;
    public Transform rightFootBone;
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public Transform leftKnee;
    public Transform rightKnee;
    public Transform leftKneeHint;
    public Transform rightKneeHint;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public float raycastDistance = 1.5f;
    public float footOffset = 0.05f;
    public float smoothSpeed = 10f;

    // Store initial offsets between bone and target
    private Quaternion leftFootInitialRot;
    private Quaternion rightFootInitialRot;
    private Vector3 leftFootInitialPos;
    private Vector3 rightFootInitialPos;

    void Start()
    {
        if (leftFootBone && leftFootTarget)
        {
            leftFootInitialPos = leftFootTarget.position - leftFootBone.position;
            leftFootInitialRot = Quaternion.Inverse(leftFootBone.rotation) * leftFootTarget.rotation;
        }
        if (rightFootBone && rightFootTarget)
        {
            rightFootInitialPos = rightFootTarget.position - rightFootBone.position;
            rightFootInitialRot = Quaternion.Inverse(rightFootBone.rotation) * rightFootTarget.rotation;
        }

        //leftKneeHint.position = leftKnee.position + Vector3.forward * 0.1f;
        //rightKneeHint.position = rightKnee.position + Vector3.forward * 0.1f;
    }

    void Update()
    {
        UpdateFootTarget(leftFootBone, leftFootTarget, leftFootInitialPos, leftFootInitialRot);
        UpdateFootTarget(rightFootBone, rightFootTarget, rightFootInitialPos, rightFootInitialRot);
    }

    void UpdateFootTarget(Transform foot, Transform target, Vector3 posOffset, Quaternion rotOffset)
    {
        if (!foot || !target) return;

        Vector3 origin = foot.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
        {
            // Desired position with offset
            Vector3 targetPos = hit.point + hit.normal * footOffset;
            target.position = Vector3.Lerp(target.position, targetPos, Time.deltaTime * smoothSpeed);

            // Desired rotation: align ground normal but keep initial rotation relative to bone
            Quaternion groundRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Quaternion desiredRot = groundRotation * foot.rotation * rotOffset;

            target.rotation = Quaternion.Slerp(target.rotation, desiredRot, Time.deltaTime * smoothSpeed);
        }
        else
        {
            // If nothing is hit, smoothly return to bone's animated pose
            target.position = Vector3.Lerp(target.position, foot.position + posOffset, Time.deltaTime * smoothSpeed);
            target.rotation = Quaternion.Slerp(target.rotation, foot.rotation * rotOffset, Time.deltaTime * smoothSpeed);
        }
    }
}
