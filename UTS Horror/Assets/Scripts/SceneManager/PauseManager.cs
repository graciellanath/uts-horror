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
        ResumeGame();
    }

    void Update()
    {
        // escape untuk pause/resume
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

        // 1. matiin waktu di game
        Time.timeScale = 0f;

        // 2. matiin script player
        if (playerScript != null)
        {
            playerScript.enabled = false;
        }

        // 3. pastiin cursor muncul & bebas gerak
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;

        if (playerScript != null)
            playerScript.enabled = true;

        // resume balik lock kursor & sembunyiin
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BackToMainMenu()
    {
        // balikin waktu ke normal
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}