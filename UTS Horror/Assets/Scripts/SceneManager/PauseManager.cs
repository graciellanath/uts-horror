using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuPanel;

    [Header("Script References")]
    // Drag objek Player (yang ada script level1hero) ke sini di Inspector
    public level1hero playerScript;

    private bool isPaused = false;

    void Start()
    {
        // Pastikan game berjalan normal saat mulai
        ResumeGame();
    }

    void Update()
    {
        // Tombol ESC untuk Pause/Resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);

        // 1. Matikan Waktu (Timer & Fisika berhenti total)
        Time.timeScale = 0f;

        // 2. Matikan Script Player (Agar tidak bisa rotasi/gerak sama sekali)
        if (playerScript != null)
        {
            playerScript.enabled = false;
        }

        // 3. Pastikan Cursor Muncul & Bebas untuk klik menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);

        // 1. Kembalikan Waktu Normal
        Time.timeScale = 1f;

        // 2. Nyalakan kembali Script Player
        if (playerScript != null)
        {
            playerScript.enabled = true;
        }

        // Tidak perlu mengatur Cursor di sini, 
        // karena level1hero akan mengaturnya sendiri saat klik kanan.
    }

    public void BackToMainMenu()
    {
        // PENTING: Kembalikan waktu sebelum pindah scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}