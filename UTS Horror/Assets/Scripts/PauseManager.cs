using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public MonoBehaviour cameraController; // drag script kamera kamu di Inspector
    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        ResumeGame(); // Pastikan game mulai dalam keadaan jalan
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        pauseMenuPanel.SetActive(true);

        // ⏸ Hentikan waktu dalam game
        Time.timeScale = 0f;

        // ⛔ Matikan script kamera supaya gak bisa rotasi
        if (cameraController != null)
            cameraController.enabled = false;

        // ⛔ Kursor bebas
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        pauseMenuPanel.SetActive(false);

        // ▶️ Lanjutkan waktu
        Time.timeScale = 1f;

        // 🔒 Aktifkan lagi kamera controller
        if (cameraController != null)
            cameraController.enabled = true;

        // 🔒 Kunci kursor lagi biar bisa lihat sekitar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // Reset waktu dulu
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
