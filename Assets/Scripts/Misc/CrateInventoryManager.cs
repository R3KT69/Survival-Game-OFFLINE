using UnityEngine;
using System.Collections.Generic;

public class CrateInventoryManager : MonoBehaviour
{
    public Item[,] crate_inv = new Item[7,3];
    private Dictionary<string, Item> ItemLookup;
    public ItemRoot All_Items;
    public Vector2Int itemSize; // Y MAX 7x3 = 21, X min = 0
    public bool PersistentCrate;

    void Awake()
    {
        //PersistentCrate = true;
        ItemLookup = new Dictionary<string, Item>();
        foreach (Item w in All_Items.root_items) ItemLookup[w.id] = w;

        //        [INVENTORY LAYOUT]
        /*     ******COLUMN FIRST******
        0      1     2     3     4     5     6     7
        0    (x,y) (x,y) (x,y) (x,y) (x,y) (x,y) (x,y)
        1    (0,0) (1,0) (2,0) (3,0) (4,0) (5,0) (6,0) 
        2    (0,1) (1,1) (2,1) (3,1) (4,1) (5,1) (6,1)
        3    (0,2) (1,2) (2,2) (3,2) (4,2) (5,2) (6,2)
        */
        
        /*
        crate_inv[0,0] = ItemLookup["PISTOL"];
        crate_inv[0,1] = ItemLookup["SHOTGUN"];
        crate_inv[0,2] = ItemLookup["WATER"];

        crate_inv[6,0] = ItemLookup["PISTOL"];
        crate_inv[6,1] = ItemLookup["SHOTGUN"];
        crate_inv[6,2] = ItemLookup["WATER"];*/
        if (itemSize == null)
        {
            itemSize.x = 2;
            itemSize.y = 4;
        }

        RandomizedCrate();
    }
    void Update()
    {
        Debug.Log($"is crate empty?: {isCrateEmpty()}");
        if (isCrateEmpty() && PersistentCrate == false)
        {
            Destroy(gameObject);
        }
    }

    bool isCrateEmpty()
    {
        foreach (var item in crate_inv)
        {
            if (item != null) return false;
                
        } return true;
    }

    void RandomizedCrate()
    {
        // Clear crate
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                crate_inv[x, y] = null;
            }
        }

        List<Item> allItems = new List<Item>(ItemLookup.Values);

        int itemCount = Random.Range(itemSize.x, itemSize.y); // how many items to place

        int placed = 0;

        // COLUMN FIRST
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (placed >= itemCount)
                    return;

                Item randomItem = allItems[Random.Range(0, allItems.Count)];
                crate_inv[x, y] = randomItem;

                placed++;
            }
        }
    }

    
}
