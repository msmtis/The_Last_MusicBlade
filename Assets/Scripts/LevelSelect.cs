using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
     public string levelSelectSceneName = "MainMenu"; 

    public void GoToLevelSelect()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(levelSelectSceneName);
    }
}
