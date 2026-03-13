using UnityEngine;

public class FpsHeadController : MonoBehaviour
{
    [Header("View Settings")]
    public float mouseSensitivity = 2f;
    public float minYAngle = -89f;
    public float maxYAngle = 89f;

    [Header("Movement Settings")]
    public float bodyRotationSpeed = 10f;
    public Transform cameraSocket;

    private float xAngle = 0f;
    private float yAngle = 0f;
    public Transform playerTransform;
    private Animator animator;

    public Quaternion rotationOffset;
    public HudConsole hud;

    void Start()
    {
        //playerTransform = transform.parent;
        animator = GetComponentInParent<Animator>();
        rotationOffset = Quaternion.Euler(0,0,0);
        
        if (playerTransform == null)
        {
            enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = 0f;
        float mouseY = 0f;

        if (!hud.isInventoryOpen)
        {
            mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        }

        xAngle += mouseX;
        yAngle -= mouseY;
        yAngle = Mathf.Clamp(yAngle, minYAngle, maxYAngle);

        

        // Rotate player body
        Quaternion targetRotation = Quaternion.Euler(0f, xAngle, 0f);
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * bodyRotationSpeed);

        Quaternion camRotation = Quaternion.Euler(yAngle, xAngle, 0f);
        transform.rotation = camRotation * rotationOffset;
        //transform.position = new Vector3(cameraSocket.position.x, cameraSocket.position.y, cameraSocket.position.z);
        transform.position = Vector3.Lerp(transform.position, cameraSocket.position, Time.deltaTime * 10f);
        

        //headTransform.rotation = camRotation;
        //transform.position = new Vector3(cameraSocket.position.x, cameraSocket.position.y, cameraSocket.position.z);
        //transform.position = Vector3.Lerp(transform.position, cameraSocket.position, Time.deltaTime * 20f);



    }
}
