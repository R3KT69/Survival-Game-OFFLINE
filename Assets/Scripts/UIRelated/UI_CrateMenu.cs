using System.Collections.Generic;
using UnityEngine;

public class UI_CrateMenu : MonoBehaviour
{
    public List<UIHover> crateSlots;                 
    public PlayerInventoryManager playerInventoryManager;
    public UI_CrateManager crateManager;
    public Vector2Int itemPos;
    //public ItemAction itemAction;
    //public UIHoverHelperInvToHotbar uIHoverHelperInvToHotbar;


    void Update()
    {
        for (int i = 0; i < crateSlots.Count; i++)
        {
            UIHover slot = crateSlots[i];

            if (slot.isHovering && Input.GetMouseButtonDown(0))
            {
                Vector2Int pos = IndexToXY(i);

                if (crateManager.selected_crate.crate_inv[pos.x, pos.y] == null)
                {
                    Debug.Log("Clicked on empty crate inv");
                    itemPos = pos;
                    return;
                }
    
                //itemAction.selected_item = playerInventoryManager.inv[pos.x, pos.y];
                //Debug.Log($"Selected item: ({itemAction.selected_item.id})");
                Debug.Log($"crate pos: Clicked slot at ({pos.x},{pos.y}) which is {crateManager.selected_crate.crate_inv[pos.x, pos.y].id}");
                itemPos = pos;
            }
        }
    }

    public Vector2Int IndexToXY(int index)
    {
        int height = crateManager.selected_crate.crate_inv.GetLength(1); // number of rows
        int x = index / height;   // column
        int y = index % height;   // row
        return new Vector2Int(x, y);
    }
}