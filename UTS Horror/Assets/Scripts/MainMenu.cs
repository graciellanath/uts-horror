using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Tombol Play
    public void PlayGame()
    {
        SceneManager.LoadScene("Map_Hosp1"); // Ganti dengan nama scene gameplay kamu
    }

    // Tombol How To Play
    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay"); // Pastikan scene HowToPlay sudah ditambahkan di Build Settings
    }

    // Tombol Quit
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed!");
    }
}
