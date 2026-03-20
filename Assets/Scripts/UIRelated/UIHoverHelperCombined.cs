using System.Collections.Generic;
using UnityEngine;

public class UIHoverHelperCombined : MonoBehaviour
{
    public List<UIHover> invSlots;          // Inventory UI slots
    public List<UIHover> hotbarSlots;       // Hotbar UI slots
    public PlayerInventoryManager playerInventoryManager;

    private enum DragSource { None, Inventory, Hotbar }
    private DragSource currentDrag = DragSource.None;

    private int dragStartIndex = -1;        // Inventory index
    private int dragStartHotbarIndex = -1;  // Hotbar index
    public Vector2Int itemPos;

    void Update()
    {
        // -------- Start drag from inventory --------
        if (currentDrag == DragSource.None)
        {
            for (int i = 0; i < invSlots.Count; i++)
            {
                UIHover slot = invSlots[i];
                if (slot.isHovering && Input.GetMouseButtonDown(0))
                {
                    dragStartIndex = i;
                    dragStartHotbarIndex = -1;
                    currentDrag = DragSource.Inventory;

                    Vector2Int pos = IndexToXY(i);
                    Debug.Log($"Drag started from INV slot ({pos.x},{pos.y})");
                    break;
                }
            }
        }

        // -------- Start drag from hotbar --------
        if (currentDrag == DragSource.None)
        {
            for (int i = 0; i < hotbarSlots.Count; i++)
            {
                UIHover slot = hotbarSlots[i];
                if (slot.isHovering && Input.GetMouseButtonDown(0))
                {
                    dragStartHotbarIndex = i;
                    dragStartIndex = -1;
                    currentDrag = DragSource.Hotbar;

                    Debug.Log($"Drag started from HOTBAR slot {i}");
                    break;
                }
            }
        }

        // -------- Handle mouse release --------
        if (Input.GetMouseButtonUp(0))
        {
            // -------- Inventory drag release --------
            if (currentDrag == DragSource.Inventory && dragStartIndex != -1)
            {
                // Release over inventory (Inv→Inv)
                for (int i = 0; i < invSlots.Count; i++)
                {
                    UIHover slot = invSlots[i];
                    if (slot.isHovering)
                    {
                        Vector2Int startPos = IndexToXY(dragStartIndex);
                        Vector2Int endPos = IndexToXY(i);

                        Debug.Log($"Inv→Inv swap from ({startPos.x},{startPos.y}) to ({endPos.x},{endPos.y})");
                        playerInventoryManager.InvToInvSwap(startPos.x, startPos.y, endPos.x, endPos.y);
                        break;
                    }
                }

                // Release over hotbar (Inv→Hotbar)
                for (int i = 0; i < hotbarSlots.Count; i++)
                {
                    UIHover slot = hotbarSlots[i];
                    if (slot.isHovering)
                    {
                        Vector2Int invPos = IndexToXY(dragStartIndex);
                        Debug.Log($"Inv→Hotbar swap from ({invPos.x},{invPos.y}) to hotbarIndex {i}");
                        playerInventoryManager.InvToHotbarSwap(i, invPos.x, invPos.y);
                        break;
                    }
                }
            }
            // -------- Hotbar drag release --------
            else if (currentDrag == DragSource.Hotbar && dragStartHotbarIndex != -1)
            {
                for (int i = 0; i < invSlots.Count; i++)
                {
                    UIHover slot = invSlots[i];
                    if (slot.isHovering)
                    {
                        Vector2Int invPos = IndexToXY(i);
                        Debug.Log($"Hotbar→Inv swap: hotbarIndex={dragStartHotbarIndex} to slot ({invPos.x},{invPos.y})");
                        playerInventoryManager.HotbarToInvSwap(dragStartHotbarIndex, invPos.x, invPos.y);
                        break;
                    }
                }
            }

            // Reset drag state
            dragStartIndex = -1;
            dragStartHotbarIndex = -1;
            currentDrag = DragSource.None;
        }

        if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < invSlots.Count; i++)
            {
                UIHover slot = invSlots[i];
                if (slot.isHovering)
                {
                    itemPos = IndexToXY(i);
                    //Debug.Log($"Inv→Inv swap from ({startPos.x},{startPos.y}) to ({endPos.x},{endPos.y})");
                    //playerInventoryManager.InvToInvSwap(startPos.x, startPos.y, endPos.x, endPos.y);
                    break;
                }
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