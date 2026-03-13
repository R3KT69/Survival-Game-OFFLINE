using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
    public PlayerInventoryManager playerInventoryManager;
    public string ITEMID;

    public void CraftItem()
    {
        int x, y;
        if (GetEmptySlot(out x,out y))
        {
            playerInventoryManager.AssignItemInventory(x, y, ITEMID);
        }
    }

    public bool GetEmptySlot(out int x, out int y)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (playerInventoryManager.inv[i, j] == null)
                {
                    x = i;
                    y = j;
                    return true;
                }
            }
        }

        Debug.Log("No empty slot");
        x = -1;
        y = -1;
        return false;
    }
}
