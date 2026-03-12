using UnityEngine;

public enum ItemType
{
    Weapon,
    Tool,
    Consumable,
    Meele
}

public enum WEP_Type
{
    OneHandedGun, TwoHandedGun, OneHandedTool, Unarmed, NONE
}

public class Item : MonoBehaviour
{
    public bool isStackable;
    public ItemType itemType;
    public WEP_Type weaponType;
    public int runtimeCount;
    public int defaultCount;
    public float Durability;
    public string id;

    //public string instanceId;
    //public int maxStack = 1;
    //public int currentStack = 1;
    public float maxDurability = 100f;

    void Start()
    {
        //id = gameObject.name;
        runtimeCount = defaultCount;
    }

    public void AssignItem(int index)
    {
        
        
    }

    public void ReduceCount(int amount)
    {
        runtimeCount -= amount;

        if (runtimeCount <= 0)
        {
            runtimeCount = 0;
            
        }
    }



    public void IncreaseCount(int amount)
    {
        runtimeCount += amount;
    }

}
