using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrateSlot : MonoBehaviour
{
    public Item currentItem;
    public UI_CrateManager cratemanager;
    public TextMeshProUGUI UI_text;
    public TextMeshProUGUI info;
    public Image item_image;

    void Awake()
    {
        UI_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        info = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        item_image = transform.GetChild(2).GetComponent<Image>();
        item_image.preserveAspect = true;
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

            Color alpha = item_image.color;
            alpha.a = 1f;
            item_image.color = alpha;
            
            item_image.sprite = cratemanager.selected_crate.crate_inv[ind1,ind2].item_icon;
            
        } else
        {
            Color alpha = item_image.color;
            alpha.a = 0f;
            item_image.color = alpha;
            UI_text.text = "Empty"; 
            info.text = "0/0";
            item_image.sprite = null;
        }
    }

    void Start()
    {
        UpdateItemStateCrate();
    }






}
