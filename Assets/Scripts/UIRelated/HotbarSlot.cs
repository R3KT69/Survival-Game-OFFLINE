using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public PlayerInventoryManager playerInventoryManager;
    public TextMeshProUGUI UI_text;
    public TextMeshProUGUI info;
    public Image item_image;
    
    void Awake()
    {
        UI_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        info = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        item_image = transform.GetChild(2).GetComponent<Image>();
    }

    void Start()
    {
        //image.color = Color.black;
        UpdateItemStateHotbar();
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Updating Hotbar State");
            UpdateItemState();
        }*/
        UpdateItemStateHotbar();
    }
    

    public void UpdateItemStateHotbar()
    {
        int ind1 = int.Parse(gameObject.name[0].ToString())-1;
        

        if (playerInventoryManager.hotbar[ind1] != null)
        {
            string ammo = playerInventoryManager.hotbar[ind1].gameObject.GetComponent<Weapon_global>().runtimeAmmo.ToString();
            string maxAmmo = playerInventoryManager.hotbar[ind1].gameObject.GetComponent<Weapon_global>().wep_data.magSize.ToString();
            UI_text.text = playerInventoryManager.hotbar[ind1].id; 
            info.text = $"{ammo}/{maxAmmo}";

            Color alpha = item_image.color;
            alpha.a = 1f;
            item_image.color = alpha;
            
            item_image.sprite = playerInventoryManager.hotbar[ind1].item_icon;
        }else
        {
            Color alpha = item_image.color;
            alpha.a = 0f;
            item_image.color = alpha;
            UI_text.text = "Empty"; 
            info.text = "0/0";
            item_image.sprite = null;
        }
    }



}
