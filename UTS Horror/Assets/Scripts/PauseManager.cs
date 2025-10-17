using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public MonoBehaviour cameraController; 
    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        ResumeGame(); 
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        pauseMenuPanel.SetActive(true);

        // hentikan waktu dalam game
        Time.timeScale = 0f;

        // matikan script kamera supaya gak bisa rotasi
        if (cameraController != null)
            cameraController.enabled = false;

        // kursor bebas
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        pauseMenuPanel.SetActive(false);

        // lanjutkan waktu
        Time.timeScale = 1f;

        // aktifkan lagi kamera controller
        if (cameraController != null)
            cameraController.enabled = true;

        // kunci kursor lagi biar bisa lihat sekitar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // reset waktu dulu
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
