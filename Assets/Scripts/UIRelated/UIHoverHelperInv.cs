using System.Collections.Generic;
using UnityEngine;

public class UIHoverHelperInv : MonoBehaviour
{
    public List<UIHover> invSlots;                 
    public PlayerInventoryManager playerInventoryManager;
    public ItemAction itemAction;
    //public UIHoverHelperInvToHotbar uIHoverHelperInvToHotbar;


    void Update()
    {
        for (int i = 0; i < invSlots.Count; i++)
        {
            UIHover slot = invSlots[i];

            if (slot.isHovering && Input.GetMouseButtonDown(0))
            {
                Vector2Int pos = IndexToXY(i);

                if (playerInventoryManager.inv[pos.x, pos.y] == null)
                {
                    Debug.Log("Clicked on empty inv");
                    return;
                }
    
                itemAction.selected_item = playerInventoryManager.inv[pos.x, pos.y];
                Debug.Log($"Selected item: ({itemAction.selected_item.id})");
                Debug.Log($"inv pos: Clicked slot at ({pos.x},{pos.y})");
            }
        }
    }

    private Vector2Int IndexToXY(int index)
    {
        int height = playerInventoryManager.inv.GetLength(1); // number of rows
        int x = index / height;   // column
        int y = index % height;   // row
        return new Vector2Int(x, y);
    }
}