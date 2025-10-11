using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RetryGame()
    {
        // Ulangi level saat ini
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        // Kembali ke scene menu utama
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        // Keluar dari game
        Application.Quit();
    }
}
