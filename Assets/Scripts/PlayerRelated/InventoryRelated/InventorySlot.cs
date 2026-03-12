using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public Item currentItem;
    public PlayerInventoryManager playerInventoryManager;
    public TextMeshProUGUI UI_text;
    

    void Awake()
    {
        UI_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Updating Inventory State");
            UpdateItemState();
        }
    }

    public void UpdateItemState()
    {
        int ind1 = int.Parse(gameObject.name[0].ToString())-1;
        int ind2 = int.Parse(gameObject.name[1].ToString())-1;

        if (playerInventoryManager.inv[ind1,ind2] != null)
        {
            UI_text.text = playerInventoryManager.inv[ind1,ind2].id; 
        } else
        {
            UI_text.text = "Empty"; 
        }
    }

    void Start()
    {
        UpdateItemState();
    }






}
