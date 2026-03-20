using UnityEngine;

public class DropItem : MonoBehaviour
{
    public PlayerInventoryManager inv;
    public Weapon_driver weapon_Driver;
    public ChangingArm arm;
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
        Destroy(weapon_Driver.currentWeapon.gameObject);
        weapon_Driver.currentWeapon = droppedObj.GetComponent<Weapon_global>();
        arm.Unarmed();

        //MonoBehaviour[] scripts = droppedObj.GetComponents<MonoBehaviour>();
        //foreach (MonoBehaviour script in scripts) Destroy(script);

        Rigidbody rigidObj = droppedObj.AddComponent<Rigidbody>();
        rigidObj.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidObj.interpolation = RigidbodyInterpolation.Interpolate;
        droppedObj.GetComponent<BoxCollider>().enabled = true;
        //droppedObj.AddComponent<BoxCollider>();
        //droppedObj.GetComponent<BoxCollider>().size = new Vector3(0.1f,0.15f,0.5f);
        //droppedObj.GetComponent<BoxCollider>().excludeLayers = playerLayer;

        
        
        rigidObj.linearVelocity = transform.forward * 3f + Vector3.up * 1.5f;
        
    }

    public void DropItemInv(Item item)
    {
        GameObject currentObj = item.gameObject;
        currentObj.SetActive(true);
        currentObj.transform.localScale = currentObj.gameObject.GetComponent<Weapon_global>().scale * Vector3.one;
        Vector3 dropPos = transform.localPosition;
        dropPos.y += dropHeight;

        Vector3 dropRot = transform.localRotation.eulerAngles;
        dropRot.z += 90f;
        Quaternion rotation = Quaternion.Euler(dropRot);
        

        GameObject droppedObj = Instantiate(currentObj, dropPos, rotation);
        //Destroy(weapon_Driver.currentWeapon.gameObject);
        //weapon_Driver.currentWeapon = droppedObj.GetComponent<Weapon_global>();
        //arm.Unarmed();

        //MonoBehaviour[] scripts = droppedObj.GetComponents<MonoBehaviour>();
        //foreach (MonoBehaviour script in scripts) Destroy(script);

        Rigidbody rigidObj = droppedObj.AddComponent<Rigidbody>();
        rigidObj.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidObj.interpolation = RigidbodyInterpolation.Interpolate;
        droppedObj.GetComponent<BoxCollider>().enabled = true;
        //droppedObj.AddComponent<BoxCollider>();
        //droppedObj.GetComponent<BoxCollider>().size = new Vector3(0.1f,0.15f,0.5f);
        //droppedObj.GetComponent<BoxCollider>().excludeLayers = playerLayer;

        
        
        rigidObj.linearVelocity = transform.forward * 3f + Vector3.up * 1.5f;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (arm.currentslot < 0 || arm.currentslot >= inv.hotbar.Length || inv.hotbar[arm.currentslot] == null)
            {
                Debug.Log("Nothing to drop!");
                return;
            }

            DropItemCurrent();
        }
        
    }
}
