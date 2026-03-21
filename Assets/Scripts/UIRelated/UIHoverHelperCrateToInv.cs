using UnityEngine;
using System.Collections.Generic;

public class UIHoverHelperDragManager : MonoBehaviour
{
    [Header("Inventory & Crate Setup")]
    public PlayerInventoryManager playerInventoryManager;
    public UI_CrateManager crateManager;
    public UI_CrateMenu crateMenu;
    public UIHoverHelperCombined uIHoverHelperCombined;

    [Header("Drag Tracking")]
    public Vector2Int cratePos = new Vector2Int(-1, -1);
    public Vector2Int invPos = new Vector2Int(-1, -1);

    private enum DragSource { None, Inventory, Crate }
    private DragSource currentDrag = DragSource.None;

    private Vector2Int startInvPos = new Vector2Int(-1, -1);
    private Vector2Int startCratePos = new Vector2Int(-1, -1);
    private bool isDragging = false;

    void Update()
    {
        if (crateManager.selected_crate == null)
            return;

        // ---------------- Start Drag ----------------
        if (!isDragging && Input.GetMouseButtonDown(0))
        {
            // Drag from Inventory
            // Drag from Inventory
        for (int i = 0; i < uIHoverHelperCombined.invSlots.Count; i++)
        {
            UIHover slot = uIHoverHelperCombined.invSlots[i];
            if (slot.isHovering && playerInventoryManager.inv[uIHoverHelperCombined.IndexToXY(i).x, uIHoverHelperCombined.IndexToXY(i).y] != null)
            {
                startInvPos = uIHoverHelperCombined.IndexToXY(i);
                startCratePos = new Vector2Int(-1, -1);
                currentDrag = DragSource.Inventory;
                isDragging = true;
                Debug.Log($"Drag started from Inventory {startInvPos}");
                break;
            }
        }
            
            
                // Drag from Crate
                for (int i = 0; i < crateMenu.crateSlots.Count; i++)
                {
                    UIHover slot = crateMenu.crateSlots[i];
                    Vector2Int pos = crateMenu.IndexToXY(i);
                    if (slot.isHovering &&
                        crateManager.selected_crate.crate_inv[pos.x, pos.y] != null)
                    {
                        startCratePos = pos;
                        startInvPos = new Vector2Int(-1, -1);
                        currentDrag = DragSource.Crate;
                        isDragging = true;
                        Debug.Log($"Drag started from Crate {startCratePos}");
                        break;
                    }
                }
            
        }

        // ---------------- Update Hover Target ----------------
        if (isDragging)
        {
            if (currentDrag == DragSource.Inventory)
            {
                // Hover over Crate slots
                cratePos = new Vector2Int(-1, -1);
                for (int i = 0; i < crateMenu.crateSlots.Count; i++)
                {
                    UIHover slot = crateMenu.crateSlots[i];
                    if (slot.isHovering)
                    {
                        cratePos = crateMenu.IndexToXY(i);
                        break;
                    }
                }
            }
            else if (currentDrag == DragSource.Crate)
            {
                // Hover over Inventory slots
                invPos = new Vector2Int(-1, -1);
                for (int i = 0; i < uIHoverHelperCombined.invSlots.Count; i++)
                {
                    UIHover slot = uIHoverHelperCombined.invSlots[i];
                    if (slot.isHovering)
                    {
                        invPos = uIHoverHelperCombined.IndexToXY(i);
                        break;
                    }
                }
            }
        }

        // ---------------- Handle Drop ----------------
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            if (currentDrag == DragSource.Inventory)
            {
                // Inventory → Crate
                if (cratePos.x >= 0 && cratePos.y >= 0)
                {
                    Item dragged = playerInventoryManager.inv[startInvPos.x, startInvPos.y];
                    Item target = crateManager.selected_crate.crate_inv[cratePos.x, cratePos.y];

                    // Swap or move
                    crateManager.selected_crate.crate_inv[cratePos.x, cratePos.y] = dragged;
                    playerInventoryManager.inv[startInvPos.x, startInvPos.y] = target;

                    Debug.Log($"Inventory {startInvPos} → Crate {cratePos}");
                }
            }
            else if (currentDrag == DragSource.Crate)
            {
                // Crate → Inventory
                if (invPos.x >= 0 && invPos.y >= 0)
                {
                    Item dragged = crateManager.selected_crate.crate_inv[startCratePos.x, startCratePos.y];
                    Item target = playerInventoryManager.inv[invPos.x, invPos.y];

                    // Swap or move
                    playerInventoryManager.inv[invPos.x, invPos.y] = dragged;
                    crateManager.selected_crate.crate_inv[startCratePos.x, startCratePos.y] = target;

                    Debug.Log($"Crate {startCratePos} → Inventory {invPos}");
                }
            }

            // Reset drag
            startInvPos = new Vector2Int(-1, -1);
            startCratePos = new Vector2Int(-1, -1);
            invPos = new Vector2Int(-1, -1);
            cratePos = new Vector2Int(-1, -1);
            currentDrag = DragSource.None;
            isDragging = false;
        }
    }
}