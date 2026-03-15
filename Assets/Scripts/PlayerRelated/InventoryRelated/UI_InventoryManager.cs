// UNUSED
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryManager : MonoBehaviour
{
    public List<InventorySlot> inventorySlots;
    
    public PlayerInventoryManager playerInventory;
    
    void Start()
    {
        LookupSlot("1", "1", inventorySlots);

        //playerInventory.InitializeItems();
        InitializePlayerInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //playerInventory.InitializeItems();
            InitializePlayerInventory();
        }
    
    }

    void InitializePlayerInventory()
    {
        Debug.Log("Initializing UI slots from player inventory...");

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                InventorySlot slot = LookupSlot((x + 1).ToString(), (y + 1).ToString(), inventorySlots);

                if (slot != null)
                {
                    slot.currentItem = playerInventory.inv[x, y];
                    slot.UI_text.text = playerInventory.inv[x, y].id;

                    Debug.Log($"Slot {slot.name} assigned: {(slot.currentItem != null ? slot.currentItem.name : "Empty")}");
                }
                else
                {
                    Debug.LogWarning($"Slot {x + 1}{y + 1} not found in UI slots!");
                }
            }
        }

        Debug.Log("UI slots initialization complete.");
    }

    

    public InventorySlot LookupSlot(string x, string y, List<InventorySlot> inventorySlots)
    {
        string lookup = $"{x}{y}";

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].gameObject.name == lookup)
            {
                Debug.Log($"Slot Name: {inventorySlots[i].name}");
                
                return inventorySlots[i];
            }
        }

        return null;
    }
}
