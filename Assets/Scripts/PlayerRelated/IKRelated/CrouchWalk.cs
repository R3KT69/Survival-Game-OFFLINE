using UnityEngine;

public class CrouchWalk : MonoBehaviour
{
    public Transform leftFoot, rightFoot;
    public float stride = 0.1f;   // max forward/back offset
    public float speed = 3f;      // how fast feet alternate
    public Movement movement;
    private float timer = 0f;

    void Update()
    {
        if (movement.isMoving() == false)
        {
            return;
        }

        timer += Time.deltaTime * speed;

        float leftOffsetZ = Mathf.Sin(timer) * stride;
        float rightOffsetZ = Mathf.Sin(timer + Mathf.PI) * stride; // opposite phase

        leftFoot.localPosition = new Vector3(leftFoot.localPosition.x, leftFoot.localPosition.y, leftOffsetZ);
        rightFoot.localPosition = new Vector3(rightFoot.localPosition.x, rightFoot.localPosition.y, rightOffsetZ);
    }
}