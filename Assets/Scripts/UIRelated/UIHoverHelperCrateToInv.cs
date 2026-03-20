using UnityEngine;
using System.Collections.Generic;

public class UIHoverHelperCrateToInv : MonoBehaviour
{
    public PlayerInventoryManager playerInventoryManager;
    public UI_CrateManager crateManager;
    public UIHoverHelperCombined uIHoverHelperCombined;
    public UI_CrateMenu crateMenu;
    public Vector2Int invPos;
    public Vector2Int cratePos;


    void Update()
    {
        if (crateManager.selected_crate == null)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            invPos = uIHoverHelperCombined.itemPos;
            cratePos = crateMenu.itemPos;

            Debug.Log($"Dragged from {cratePos} to {invPos}");

            //int x,y;
            //playerInventoryManager.get_first_empty_slot(out x, out y);

            if (crateManager.selected_crate.crate_inv[cratePos.x, cratePos.y] != null)
            {
                if (playerInventoryManager.inv[invPos.x, invPos.y] == null)
                {
                    playerInventoryManager.inv[invPos.x, invPos.y] = crateManager.selected_crate.crate_inv[cratePos.x, cratePos.y];
                    crateManager.selected_crate.crate_inv[cratePos.x, cratePos.y] = null;
                }

            }
            
        }
    }



}
