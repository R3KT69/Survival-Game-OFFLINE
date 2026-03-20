using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemAction : MonoBehaviour
{
    public TextMeshProUGUI description;
    public TextMeshProUGUI item_name;
    public Item selected_item;
    public PlayerStatsManager playerStatsManager;
    public DropItem dropItem;

    void Start()
    {
        description.text = "Description";
        item_name.text = "Item Name";
    }

    public void UseItem()
    {
        if (selected_item != null)
        {
            if (selected_item.id == "BANDAGE")
            {
                playerStatsManager.AddHealth(selected_item.defaultCount);
                Destroy(selected_item);
            }

            if (selected_item.id == "RATION")
            {
                playerStatsManager.AddFood(selected_item.defaultCount);
                Destroy(selected_item);
            }

            if (selected_item.id == "WATER")
            {
                playerStatsManager.AddFood(selected_item.defaultCount);
                Destroy(selected_item);
            }

            
        }
    }

    public void DiscardItem()
    {
        if (selected_item != null)
        {
            dropItem.DropItemInv(selected_item);
            Destroy(selected_item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selected_item != null)
        {
            item_name.text = selected_item.id;
            description.text = selected_item.description;
        }
    }
}
