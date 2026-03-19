using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerInventoryManager inv;
    public Image health_bar;
    public Image food_bar;
    public int health_pts;
    public int food_pts;

    // Update is called once per frame
    void Update()
    {
        health_bar.fillAmount = health_pts * 1/100;
        food_bar.fillAmount = food_pts * 1/100;
    }
}
