using System.Collections.Generic;
using UnityEngine;

public class UIHoverHelperHotbar : MonoBehaviour
{
    public List<UIHover> hotbarslots;         
    public PlayerInventoryManager playerInventoryManager;

    private int dragStartIndex = -1;

    void Update()
    {
        for (int i = 0; i < hotbarslots.Count; i++)
        {
            UIHover slot = hotbarslots[i];

            if (slot.isHovering && Input.GetMouseButtonDown(0))
            {
                dragStartIndex = i;
                // highlight
            }

            if (slot.isHovering && Input.GetMouseButtonUp(0))
            {
                // Only swap if a drag actually started
                if (dragStartIndex != -1 && dragStartIndex != i)
                {
                    playerInventoryManager.HotbarToHotbarSwap(dragStartIndex, i);
                }
                
                dragStartIndex = -1; // reset
            }
        }
    }
}