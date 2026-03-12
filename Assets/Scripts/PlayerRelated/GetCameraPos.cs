using UnityEngine;

public class GetCameraPos : MonoBehaviour
{
    public Transform cameraPos;

    void Start()
    {
        gameObject.transform.position = cameraPos.position;
        gameObject.transform.rotation = cameraPos.rotation;
    }
}
