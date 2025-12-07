using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManagerLv2 : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("✅ GameOverManager aktif!");
    }

    public void RetryGame()
{
    Debug.Log("Retry diklik!");
    SceneManager.LoadScene("Level2");
}


    public void BackToMainMenu()
    {
        Debug.Log("Balik ke menu utama!");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Keluar game!");
        Application.Quit();
    }
}
