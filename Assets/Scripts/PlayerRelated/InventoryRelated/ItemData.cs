using UnityEngine;

public class ItemData : MonoBehaviour
{
    public string ItemHolder;
    public int Instance;

    public void AssignItem(int index)
    {
        ItemHolder = gameObject.name + $"I:{index}";
        
    }
}
