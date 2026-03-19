using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerInventoryManager inv;
    public Image health_bar;
    public Image food_bar;
    public int health_pts;
    public int food_pts;

    void Start()
    {
        if (food_bar || health_bar == null)
        {
            health_bar = GameObject.Find("health_bar").GetComponent<Image>();
            food_bar = GameObject.Find("food_bar").GetComponent<Image>();
        }
        
    }

    void Update()
    {
        health_bar.fillAmount = health_pts * 0.01f;
        food_bar.fillAmount = food_pts * 0.01f;
    }

    public void TakeDamage(int amount)
    {
        health_pts -= amount;
    }

    public void ConsumeFood(int amount)
    {
        food_pts -= amount;
    }
}
