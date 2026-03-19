using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerInventoryManager inv;
    public Image health_bar;
    public Image food_bar;
    public int health_pts;
    public int food_pts;

    
    void Update()
    {
        health_bar.fillAmount = health_pts * 0.01f;
        food_bar.fillAmount = food_pts * 0.01f;
    }
}
