using UnityEngine;

public class FpsCheck : MonoBehaviour
{
    public float updateInterval = 0.5f; // seconds between updates
    private float accum = 0f;           // FPS accumulated over interval
    private int frames = 0;             // Frames drawn
    private float timeLeft;             // Time left for current interval
    private float fps = 0f;             // Calculated FPS

    void Start()
    {
        timeLeft = updateInterval;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        // Update FPS every interval
        if (timeLeft <= 0.0)
        {
            fps = accum / frames;
            timeLeft = updateInterval;
            accum = 0f;
            frames = 0;
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.yellow;

        GUI.Label(new Rect(10, 10, 150, 30), $"FPS: {fps:F1}", style);
    }
}