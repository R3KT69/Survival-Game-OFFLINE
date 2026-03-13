using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public Item[,] inv = new Item[5,5];
    public Item[] hotbar = new Item[6];
    public ItemRoot All_Items;
    public Transform holding_point_twohanded;
    public Transform holding_point_onehanded;
    public Transform holding_point_tool;
    public Transform ItemInventory;
    public Weapon_driver weapon_Driver;
    
    //public List<Item> AllWeapons; 
    //public List<Item> AllTools; 
    //public List<Item> AllConsumeable; 
    //public List<Item> AllMeele; 
    
    

    private Dictionary<string, Item> ItemLookup;
    
    
    void Awake()
    {
        // Populate Dictionary
        ItemLookup = new Dictionary<string, Item>();
        foreach (Item w in All_Items.root_items) ItemLookup[w.id] = w;

        
        
        /*Inventory Grid:
            (0,0) (1,0) (2,0) (3,0) (4,0)
            (0,1) (1,1) (2,1) (3,1) (4,1)
            (0,2) (1,2) (2,2) (3,2) (4,2)
            (0,3) (1,3) (2,3) (3,3) (4,3)
            (0,4) (1,4) (2,4) (3,4) (4,4)
        */

        //InitializeItems();
        
        //inv[0,0] = ItemLookup["AMMO"];
        //Debug.Log(inv[0,0].runtimeCount);

        AssignItemInventory(2,3, "AMMO");
        AssignItemInventory(1,1, "AMMO");
        

        //Debug.Log(ItemLookup["PISTOL"].name);
        AssignItemHotbar(0, "PISTOL");
        AssignItemHotbar(1, "SHOTGUN");
        AssignItemHotbar(2, "RIFLE");
        AssignItemHotbar(3, "AUTOSHOT");
        AssignItemHotbar(4, "MP5");
        AssignItemHotbar(5, "HATCHET");
        
        
        /*
        inv[0,0] = WeaponLookup["PISTOL"];
        inv[1,0] = WeaponLookup["SHOTGUN"];

        inv[2,0] = ToolLookup["PICKAXE"];
        inv[3,0] = ToolLookup["HATCHET"];

        // Hotbar: (0) (1) (2) (3) (4) (5)
        hotbar[0] = WeaponLookup["PISTOL"];
        hotbar[1] = WeaponLookup["SHOTGUN"];
        hotbar[2] = WeaponLookup["RIFLE"];
        hotbar[3] = ToolLookup["PICKAXE"];
        hotbar[4] = ToolLookup["HATCHET"];
        hotbar[5] = WeaponLookup["RIFLE"];*/
        
        PrintInventoryState();
    }

    public void AssignItemHotbar(int index, string id)
    {
        if (index < 0 || index >= hotbar.Length) return;

        weapon_Driver.currentWeapon = ItemLookup[id].gameObject.GetComponent<Weapon_global>();
        if (ItemLookup[id].weaponType == WEP_Type.OneHandedGun)
        {
            hotbar[index] = SpawnItem(holding_point_onehanded, ItemLookup[id].gameObject).GetComponent<Item>();
        } else if (ItemLookup[id].weaponType == WEP_Type.TwoHandedGun)
        {
            hotbar[index] = SpawnItem(holding_point_twohanded, ItemLookup[id].gameObject).GetComponent<Item>();
        } else if (ItemLookup[id].weaponType == WEP_Type.OneHandedTool)
        {
            hotbar[index] = SpawnItem(holding_point_tool, ItemLookup[id].gameObject).GetComponent<Item>();
        }
    }

    public void AssignItemInventory(int x, int y, string id)
    {
        if (x < 0 || y < 0) return; // Also not bigger than 5
        
        inv[x,y] = SpawnItem(ItemInventory, ItemLookup[id].gameObject).GetComponent<Item>();
        
    }

    public bool get_item_index(string ID, out int x, out int y) // Returns the first item
    {
        for (x = 0; x < 5; x++)
        {
            for (y = 0; y < 5; y++)
            {
                if (inv[x, y] != null && inv[x, y].id == ID)
                {
                    return true;
                }
            }
        }
        Debug.Log($"{ID} not found/destroyed/depleted");
        x = -1;
        y = -1;
        return false;
    }

    public GameObject SpawnItem(Transform spawnPoint, GameObject itemObject)
    {
        if (itemObject == null || spawnPoint == null)
        {
            Debug.LogWarning("SpawnItem: itemObject or spawnPoint is null!");
            return null;
        }

        GameObject spawnedItem = Instantiate(itemObject, spawnPoint.position, spawnPoint.rotation);
        spawnedItem.transform.SetParent(spawnPoint);

        Weapon_global weapon = spawnedItem.GetComponent<Weapon_global>();

        if (weapon != null)
        {
            spawnedItem.transform.localPosition = weapon.position;
            spawnedItem.transform.localRotation = Quaternion.Euler(weapon.rotation);
            spawnedItem.transform.localScale = weapon.scale * Vector3.one;
        }
        else // Not a weapon
        {
            spawnedItem.transform.localPosition = Vector3.zero;
            spawnedItem.transform.localRotation = Quaternion.identity;
            spawnedItem.transform.localScale = Vector3.one;
        }

        return spawnedItem;
        
    }

    public void RemoveItem()
    {
        
    }

    public void PopulateInv(int x, int y, Item item)
    {
        inv[x,y] = item;
    }

    public void RemoveItemHotbar(int x, Item item)
    {
        hotbar[x] = null;
    }
/*
    public void InitializeItems()
    {
        for (int i = 0; i < AllWeapons.Count; i++)
        {
            AllWeapons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < AllTools.Count; i++)
        {
            AllTools[i].gameObject.SetActive(true);
        }
    }
*/
    // Update is called once per frame
    public int ammoX, ammoY;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            
            PrintInventoryState();
            //Debug.Log($"Ammo Count: {inv[0,0].runtimeCount}");
            //get_ammo_index("AMMO", out ammoX, out ammoY);
            Debug.Log($"Ammo Index: X:{ammoX} Y: {ammoY}");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //PopulateInv(4,0, AllWeapons[0]);
            //RemoveItemHotbar(0, AllWeapons[0]);
        }
    }

    public void PrintInventoryState()
    {
        string invState = "Inventory Grid:\n";
        int width = inv.GetLength(0);
        int height = inv.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (inv[x, y] != null)
                    invState += inv[x, y].id + "\t"; // show the ID
                else
                    invState += "[Empty]\t";
            }
            invState += "\n";
        }

        string hotbarState = "Hotbar:\n";
        for (int i = 0; i < hotbar.Length; i++)
        {
            if (hotbar[i] != null)
                hotbarState += $"Slot {i} Key {i+1}: {hotbar[i].id}\n";
            else
                hotbarState += $"Slot {i} Key {i+1}: [Empty]\n";
        }

        Debug.Log(invState + "\n" + hotbarState);
    }
}
