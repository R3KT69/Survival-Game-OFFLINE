using System.Collections.Generic;
using UnityEngine;

public class UIHoverHelperInv : MonoBehaviour
{
    public List<UIHover> invSlots;                 // All inventory UI slots in order (row by row)
    public PlayerInventoryManager playerInventoryManager;

    private int dragStartIndex = -1;

    void Update()
    {
        for (int i = 0; i < invSlots.Count; i++)
        {
            UIHover slot = invSlots[i];

            if (slot.isHovering && Input.GetMouseButtonDown(0))
            {
                dragStartIndex = i;
                // optionally highlight drag start slot
            }

            if (slot.isHovering && Input.GetMouseButtonUp(0))
            {
                if (dragStartIndex != -1 && dragStartIndex != i)
                {
                    // Convert linear indices to (x,y)
                    Vector2Int startPos = IndexToXY(dragStartIndex);
                    Vector2Int endPos = IndexToXY(i);

                    // Swap items in the inventory array
                    playerInventoryManager.InvToInvSwap(startPos.x, startPos.y, endPos.x, endPos.y);
                }

                dragStartIndex = -1; // reset
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