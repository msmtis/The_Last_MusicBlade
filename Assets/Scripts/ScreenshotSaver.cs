using UnityEngine;
using System.IO;
public class Simple : MonoBehaviour
{
    public Camera screenshotCamera;
    public int width = 1920;
    public int height = 1080;
    public string fileName = "ScreenshotMozart.png";

    public void SaveScreenshot()
    {
        // Crea una RenderTexture temporanea
        RenderTexture rt = new RenderTexture(width, height, 24);
        screenshotCamera.targetTexture = rt;

        // Cattura l'immagine
        Texture2D image = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshotCamera.Render();
        RenderTexture.active = rt;
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // Salva il file nella cartella Assets
        string path = Application.dataPath + "/" + fileName;
       File.WriteAllBytes(path, image.EncodeToPNG());

        Debug.Log("Screenshot salvato in: " + path);
    
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveScreenshot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
