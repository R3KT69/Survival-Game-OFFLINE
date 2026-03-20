using UnityEngine;
using System.Collections.Generic;

public class Crate : MonoBehaviour
{
    public Item[,] crate_inv = new Item[5,5];
    private Dictionary<string, Item> ItemLookup;
    public ItemRoot All_Items;

    void Start()
    {
        ItemLookup = new Dictionary<string, Item>();
        foreach (Item w in All_Items.root_items) ItemLookup[w.id] = w;

        
    }

    
}
