using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Drag PauseMenuPanel ke sini di Inspector
    private bool isPaused = false;

    void Start()
    {
        pauseMenuPanel.SetActive(false); // Pastikan awalnya tidak tampil
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f; // Berhenti semua waktu di game
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Jalanin lagi waktu
        isPaused = false;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
