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

    void Start()
    {
        description.text = "Description";
        item_name.text = "Item Name";
    }

    public void UseItem()
    {
        if (selected_item != null && selected_item.id == "BANDAGE")
        {
            playerStatsManager.health_pts += selected_item.defaultCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selected_item != null)
        {
            description.text = selected_item.id;
            item_name.text = selected_item.id;
        }
    }
}
