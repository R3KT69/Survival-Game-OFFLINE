using UnityEngine;

public class CopyPosXZ : MonoBehaviour
{
    
    public Transform target;
    
    void Update()
    {
        gameObject.transform.position = new Vector3(target.position.x, gameObject.transform.position.y, target.position.z);       
    }
}
