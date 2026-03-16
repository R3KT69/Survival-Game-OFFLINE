using UnityEngine;

public class DropItem : MonoBehaviour
{
    public PlayerInventoryManager playerInventoryManager;
    public Weapon_driver weapon_Driver;
    public LayerMask playerLayer;
    public float dropHeight;

    public void DropItemCurrent()
    {
        GameObject currentObj = weapon_Driver.currentWeapon.gameObject;
        Vector3 dropPos = transform.localPosition;
        dropPos.y += dropHeight;

        Vector3 dropRot = transform.localRotation.eulerAngles;
        dropRot.z += 90f;
        Quaternion rotation = Quaternion.Euler(dropRot);
        

        GameObject droppedObj = Instantiate(currentObj, dropPos, rotation);

        MonoBehaviour[] scripts = droppedObj.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts) Destroy(script);

        Rigidbody rigidObj = droppedObj.AddComponent<Rigidbody>();
        droppedObj.AddComponent<BoxCollider>();
        droppedObj.GetComponent<BoxCollider>().size = new Vector3(0.1f,0.15f,0.5f);
        droppedObj.GetComponent<BoxCollider>().excludeLayers = playerLayer;

        rigidObj.collisionDetectionMode = CollisionDetectionMode.Continuous;
        
        rigidObj.linearVelocity = transform.forward * 3f + Vector3.up * 1.5f;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItemCurrent();
        }
    }
}
