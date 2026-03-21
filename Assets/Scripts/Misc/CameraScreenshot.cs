using UnityEngine;
using System.Collections;
using System.IO;

public class CameraScreenshot : MonoBehaviour
{
    public KeyCode captureKey = KeyCode.P;
    public string folderName = "Screenshots";

    private int fileIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(captureKey))
        {
            StartCoroutine(CaptureScreenshot());
        }
    }

    IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();

        // Create folder if not exists
        string folderPath = Path.Combine(Application.dataPath, folderName);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Auto increment filename
        string filePath;
        do
        {
            filePath = Path.Combine(folderPath, $"screenshot_{fileIndex:D3}.png");
            fileIndex++;
        }
        while (File.Exists(filePath));

        File.WriteAllBytes(filePath, bytes);
        Debug.Log($"Saved screenshot: {filePath}");

        Destroy(tex);
    }
}