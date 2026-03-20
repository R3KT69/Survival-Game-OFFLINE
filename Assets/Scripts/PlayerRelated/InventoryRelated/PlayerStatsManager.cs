using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerInventoryManager inv;
    public Image health_bar;
    public Image food_bar;
    public int health_pts;
    public int food_pts;
    public float food_consume_rate_idle;

    void Start()
    {
        if (food_bar || health_bar == null)
        {
            health_bar = GameObject.Find("health_bar").GetComponent<Image>();
            food_bar = GameObject.Find("food_bar").GetComponent<Image>();
        }

        StartCoroutine(UseFood());
        StartCoroutine(UseFoodAuto());
        
    }

    IEnumerator UseFood()
    {
        while (true)
        {
            if (food_pts > 50)
            {
                Debug.Log("Sufficient food");
                health_pts = Mathf.Min(health_pts + 1, 100);
                food_pts = Mathf.Max(food_pts - 1, 0);
            }

            if (food_pts < 20)
            {
                Debug.Log("Low Food");
                health_pts = Mathf.Clamp(health_pts - 1, 0, 100);
            }

            if (food_pts < 10)
            {
                Debug.Log("Starving");
                health_pts = Mathf.Clamp(health_pts - 3, 0, 100);
            }

            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator UseFoodAuto()
    {
        while (true)
        {
            food_pts = Mathf.Max(food_pts - 1, 0);

            yield return new WaitForSeconds(food_consume_rate_idle);
        }
    }

    

    void Update()
    {
        health_bar.fillAmount = health_pts * 0.01f;
        food_bar.fillAmount = food_pts * 0.01f;
    }

    public void AddHealth(int amount)
    {
        health_pts += amount;
        health_pts = Mathf.Clamp(health_pts, 0, 100);
    }

    public void AddFood(int amount)
    {
        food_pts += amount;
        food_pts = Mathf.Clamp(food_pts, 0, 100);
    }

    public void TakeDamage(int amount)
    {
        health_pts -= amount;
        health_pts = Mathf.Clamp(health_pts, 0, 100);
    }

    public void ConsumeFood(int amount)
    {
        food_pts -= amount;
        food_pts = Mathf.Clamp(food_pts, 0, 100);
    }
}
