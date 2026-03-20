using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Camera mainCamera;
    public PlayerInventoryManager inv;
    public ChangingArm arm;
    public int emptyIndex;

    void Update()
    {
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * 3f, Color.green);
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetItem();
        }
    }  

    public void GetItem()
    {
        emptyIndex = -1;

        for (int i = 0; i < inv.hotbar.Length; i++)
        {
            if (inv.hotbar[i] == null)
            {
                emptyIndex = i;
                Debug.Log($"Pickup: {emptyIndex}");
                break;
            } 
        }

        if (emptyIndex == -1)
        {
            Debug.Log("Hotbar full");
            return;
        }
        
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            //if (GetCrate(hit)) return;
            
            if (hit.collider.CompareTag("Pickable"))
            {
                Item pickableItem = hit.collider.GetComponent<Item>();
                Debug.Log($"Pickup: idname {pickableItem.id}");

                if (pickableItem.itemType == ItemType.Consumable)
                {
                    GetConsumable(hit);
                    return;
                }

                inv.AssignItemHotbar(emptyIndex, pickableItem.id);
                Weapon_global assignedWeapon = inv.hotbar[emptyIndex].gameObject.GetComponent<Weapon_global>();
                
                //inv.hotbar[emptyIndex] = pickableItem;
                int savedAmmo = pickableItem.gameObject.GetComponent<Weapon_global>().runtimeAmmo;
                assignedWeapon.runtimeAmmo = savedAmmo;
                Debug.Log($"Pickup: after pickup {pickableItem.name}");
                
                arm.ChangeArm(emptyIndex, inv.hotbar, inv.hotbar[emptyIndex].weaponType, assignedWeapon);
                arm.currentslot = emptyIndex;

                Destroy(hit.collider.gameObject);
            }
        }
    }

    public void GetConsumable(RaycastHit hit)
    {
        Item pickableItem = hit.collider.GetComponent<Item>();
        Debug.Log($"Pickup: idname {pickableItem.id}");

        int x,y;
        inv.get_first_empty_slot(out x, out y);
        inv.AssignItemInventory(x,y, pickableItem.id);

        //Weapon_global assignedWeapon = inv.hotbar[emptyIndex].gameObject.GetComponent<Weapon_global>();
        
        //inv.hotbar[emptyIndex] = pickableItem;
        //int savedAmmo = pickableItem.gameObject.GetComponent<Weapon_global>().runtimeAmmo;
        //assignedWeapon.runtimeAmmo = savedAmmo;
        Debug.Log($"Pickup: after pickup {pickableItem.name}");
        
        //arm.ChangeArm(emptyIndex, inv.hotbar, inv.hotbar[emptyIndex].weaponType, assignedWeapon);
        //arm.currentslot = emptyIndex;

        Destroy(hit.collider.gameObject);
    }

    public bool GetCrate(RaycastHit hit)
    {
        if (hit.collider.tag == "CRATE")
        {
            Debug.Log("Crate found");
            return true;
        }

        return false;
    }


}
