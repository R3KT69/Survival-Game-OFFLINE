using TMPro;
using UnityEngine;

public class CrateSlot : MonoBehaviour
{
    public Item currentItem;
    public UI_CrateManager cratemanager;
    public TextMeshProUGUI UI_text;
    public TextMeshProUGUI info;
    

    void Awake()
    {
        UI_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        info = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Updating Inventory State");
            UpdateItemState();
        }*/
        UpdateItemStateCrate();
    }

    public void UpdateItemStateCrate()
    {
        int ind1 = int.Parse(gameObject.name[0].ToString())-1;
        int ind2 = int.Parse(gameObject.name[1].ToString())-1;

        if (cratemanager.selected_crate.crate_inv[ind1,ind2] != null)
        {
            UI_text.text = cratemanager.selected_crate.crate_inv[ind1,ind2].id;

            if (cratemanager.selected_crate.crate_inv[ind1,ind2].itemType == ItemType.Weapon)
            {
                string currentAmmo = cratemanager.selected_crate.crate_inv[ind1,ind2].GetComponent<Weapon_global>().runtimeAmmo.ToString();
                string maxAmmo = cratemanager.selected_crate.crate_inv[ind1,ind2].GetComponent<Weapon_global>().wep_data.magSize.ToString();
                info.text = $"{currentAmmo}/{maxAmmo}";
            } else
            {
                string count = cratemanager.selected_crate.crate_inv[ind1,ind2].runtimeCount.ToString();
                info.text = $"x{count}";
            }
            
        } else
        {
            UI_text.text = "Empty"; 
            info.text = "0/0";
        }
    }

    void Start()
    {
        UpdateItemStateCrate();
    }






}
