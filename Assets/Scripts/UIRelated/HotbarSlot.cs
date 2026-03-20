using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public PlayerInventoryManager playerInventoryManager;
    public TextMeshProUGUI UI_text;
    public TextMeshProUGUI info;
    private Image image;
    
    void Awake()
    {
        UI_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        info = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        image = gameObject.GetComponent<Image>();
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
        }else
        {
            UI_text.text = "Empty"; 
            info.text = "0/0";
        }
    }



}
