using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Tombol Play
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1"); 
    }

    // Tombol How To Play
    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay"); 
    }

    // Tombol Quit
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed!");
    }
}
