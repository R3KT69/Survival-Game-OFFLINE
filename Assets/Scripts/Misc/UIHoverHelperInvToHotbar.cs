using System.Collections.Generic;
using UnityEngine;

public class UIHoverHelperInvToHotbar : MonoBehaviour
{
    public List<UIHover> invSlots;
    public List<UIHover> hotbarSlots;
    public PlayerInventoryManager playerInventoryManager;

    private int dragStartInvIndex = -1;
    private int dragStartHotbarIndex = -1;

    void Update()
    {
        // -------- Drag start from inventory --------
        for (int i = 0; i < invSlots.Count; i++)
        {
            UIHover slot = invSlots[i];

            if (slot.isHovering && Input.GetMouseButtonDown(0))
            {
                dragStartInvIndex = i;
                dragStartHotbarIndex = -1;
            }
        }

        // -------- Drag start from hotbar --------
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            UIHover slot = hotbarSlots[i];

            if (slot.isHovering && Input.GetMouseButtonDown(0))
            {
                dragStartHotbarIndex = i;
                dragStartInvIndex = -1;
            }
        }

        // -------- Release over hotbar (Inventory → Hotbar) --------
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            UIHover slot = hotbarSlots[i];

            if (slot.isHovering && Input.GetMouseButtonUp(0))
            {
                if (dragStartInvIndex != -1)
                {
                    Vector2Int invPos = IndexToXY(dragStartInvIndex);
                    playerInventoryManager.InvToHotbarSwap(i, invPos.x, invPos.y);
                }

                dragStartInvIndex = -1;
                dragStartHotbarIndex = -1;
            }
        }

        // -------- Release over inventory (Hotbar → Inventory) --------
        for (int i = 0; i < invSlots.Count; i++)
        {
            UIHover slot = invSlots[i];

            if (slot.isHovering && Input.GetMouseButtonUp(0))
            {
                if (dragStartHotbarIndex != -1)
                {
                    Vector2Int invPos = IndexToXY(i);
                    playerInventoryManager.HotbarToInvSwap(dragStartHotbarIndex, invPos.x, invPos.y);
                }

                dragStartInvIndex = -1;
                dragStartHotbarIndex = -1;
            }
        }

        // Cancel drag if mouse released outside
        if (Input.GetMouseButtonUp(0))
        {
            dragStartInvIndex = -1;
            dragStartHotbarIndex = -1;
        }
    }

    private Vector2Int IndexToXY(int index)
    {
        int height = playerInventoryManager.inv.GetLength(1);
        int x = index / height;
        int y = index % height;
        return new Vector2Int(x, y);
    }
}