using UnityEngine;
using System.Collections.Generic;

public class CrateInventoryManager : MonoBehaviour
{
    public Item[,] crate_inv = new Item[7,3];
    private Dictionary<string, Item> ItemLookup;
    public ItemRoot All_Items;

    void Start()
    {
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
        
        crate_inv[0,0] = ItemLookup["PISTOL"];
        crate_inv[0,1] = ItemLookup["SHOTGUN"];
        crate_inv[0,2] = ItemLookup["WATER"];

        crate_inv[6,0] = ItemLookup["PISTOL"];
        crate_inv[6,1] = ItemLookup["SHOTGUN"];
        crate_inv[6,2] = ItemLookup["WATER"];
    }

    
}
