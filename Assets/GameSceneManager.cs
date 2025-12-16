using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }
    public GameObject MainPageCanvas;
    public GameObject LevelCanvas;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        MainPageCanvas.SetActive(true);
        LevelCanvas.SetActive(false);
    }
    public void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void LevelMenu()
    {
        MainPageCanvas.SetActive(false);
        LevelCanvas.SetActive(true);
    }

    public void MainMenu()
    {
        MainPageCanvas.SetActive(true);
        LevelCanvas.SetActive(false);
    }
}