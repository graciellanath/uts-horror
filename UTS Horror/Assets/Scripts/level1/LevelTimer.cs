using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // untuk Game Over

public class LevelTimer : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timerText;

    [Header("Settings")]
    public float timeRemaining = 600f; // 10 menit
    public bool isPaused = false;

    [Header("Effects")]
    public AudioSource alarmAudio;  // drag Audio Source
    private bool alarmTriggered = false;

    private bool isBlinking = false;
    private float blinkSpeed = 0.6f;

    void Update()
    {
        if (!isPaused)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver();
            }

            // Efek berkedip saat < 60 detik
            if (timeRemaining <= 60 && !isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkText());
            }

            // Suara alarm saat < 30 detik
            if (timeRemaining <= 30 && !alarmTriggered)
            {
                alarmTriggered = true;
                if (alarmAudio != null)
                    alarmAudio.Play();
            }

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // Timer berkedip
    System.Collections.IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            timerText.enabled = false;
            yield return new WaitForSeconds(blinkSpeed);

            timerText.enabled = true;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    // AUTO GAME OVER
    void GameOver()
    {
        isPaused = true;

        // 1. Jika kamu mau load scene GameOver:
        // SceneManager.LoadScene("GameOver");

        // 2. Atau kalau kamu punya Panel UI:
        // gameOverPanel.SetActive(true);
    }

    // Pause
    public void PauseTimer()
    {
        isPaused = true;
    }

    // Resume
    public void ResumeTimer()
    {
        isPaused = false;
    }

    // Restart
    public void ResetTimer()
    {
        timeRemaining = 600f;
        isPaused = false;
        alarmTriggered = false;
        isBlinking = false;
        timerText.enabled = true;

        if (alarmAudio != null)
            alarmAudio.Stop();
    }
}
