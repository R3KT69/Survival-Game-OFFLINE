using UnityEngine;
using System.Collections.Generic;

public class UIHoverHelperCrateToInv : MonoBehaviour
{
    [Header("Inventory & Crate Setup")]
    public PlayerInventoryManager playerInventoryManager;  // Inventory grid
    public UI_CrateManager crateManager;                  // Current selected crate
    public UI_CrateMenu crateMenu;                        // Crate slot UI
    public UIHoverHelperCombined uIHoverHelperCombined;   // For inventory hover info

    [Header("Drag Tracking")]
    public Vector2Int cratePos = new Vector2Int(-1, -1);  // Slot in crate being dragged
    public Vector2Int invPos = new Vector2Int(-1, -1);    // Slot in inventory being hovered on release

    private bool isDragging = false;

    void Update()
    {
        // Do nothing if no crate selected
        if (crateManager.selected_crate == null)
            return;

        // ---------------- Start drag from crate ----------------
        for (int i = 0; i < crateMenu.crateSlots.Count; i++)
        {
            UIHover slot = crateMenu.crateSlots[i];
            if (slot.isHovering && Input.GetMouseButtonDown(0))
            {
                cratePos = crateMenu.IndexToXY(i); // Convert UI index → crate array position
                isDragging = true;
                Debug.Log($"Started dragging from crate slot {cratePos}");
                break;
            }
        }

        // ---------------- Track hover over inventory ----------------
        for (int i = 0; i < uIHoverHelperCombined.invSlots.Count; i++)
        {
            UIHover slot = uIHoverHelperCombined.invSlots[i];
            if (slot.isHovering)
            {
                invPos = uIHoverHelperCombined.IndexToXY(i); // Convert UI index → inventory array position
                break;
            }
        }

        // ---------------- Handle drop ----------------
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Debug.Log($"Dropped from crate {cratePos} to inventory {invPos}");

            var crate = crateManager.selected_crate;

            // Validate positions
            if (cratePos.x >= 0 && cratePos.y >= 0 &&
                invPos.x >= 0 && invPos.y >= 0)
            {
                // Move item if inventory slot is empty
                if (crate.crate_inv[cratePos.x, cratePos.y] != null &&
                    playerInventoryManager.inv[invPos.x, invPos.y] == null)
                {
                    playerInventoryManager.inv[invPos.x, invPos.y] = crate.crate_inv[cratePos.x, cratePos.y];
                    crate.crate_inv[cratePos.x, cratePos.y] = null;
                }
            }

            // Reset drag
            cratePos = new Vector2Int(-1, -1);
            invPos = new Vector2Int(-1, -1);
            isDragging = false;
        }
    }
}