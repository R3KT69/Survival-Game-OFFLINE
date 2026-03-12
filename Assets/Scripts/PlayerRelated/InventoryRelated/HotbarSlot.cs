using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public Item currentItem;
    public PlayerInventoryManager playerInventoryManager;
    public TextMeshProUGUI UI_text;
    private Image image;
    
    void Awake()
    {
        UI_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        image = gameObject.GetComponent<Image>();
    }

    void Start()
    {
        //image.color = Color.black;
        UpdateItemState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Updating Hotbar State");
            UpdateItemState();
        }
    }
    

    public void UpdateItemState()
    {
        int ind1 = int.Parse(gameObject.name[0].ToString())-1;
        

        if (playerInventoryManager.hotbar[ind1] != null)
        {
            UI_text.text = playerInventoryManager.hotbar[ind1].id; 
        }else
        {
            UI_text.text = "Empty"; 
        }
    }



}
