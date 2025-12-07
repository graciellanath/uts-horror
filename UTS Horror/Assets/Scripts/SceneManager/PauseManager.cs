using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public MonoBehaviour cameraController;

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);

        // Cursor SELALU visible & bebas
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // ESC selalu PAUSE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;

        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;

        if (cameraController != null)
            cameraController.enabled = false;

        // Cursor tetap visible + tidak lock
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;

        if (cameraController != null)
            cameraController.enabled = true;

        // Cursor tetap visible selalu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
