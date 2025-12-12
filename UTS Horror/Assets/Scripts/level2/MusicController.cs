using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;

    public AudioSource audioSource;
    public AudioClip normalMusic;
    public AudioClip chaseMusic;
    public AudioClip lowHealthMusic;

    private bool isChasing = false;
    private bool isLowHealth = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayNormal();
    }

    // ===========================
    //       MAIN MUSIC LOGIC
    // ===========================

    public void PlayNormal()
    {
        if (audioSource.clip == normalMusic) return;

        audioSource.clip = normalMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayChase()
    {
        if (audioSource.clip == chaseMusic) return;

        isChasing = true;
        audioSource.clip = chaseMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopChase()
    {
        isChasing = false;

        if (isLowHealth)
            PlayLowHealth();
        else
            PlayNormal();
    }

    public void PlayLowHealth()
    {
        if (audioSource.clip == lowHealthMusic) return;

        audioSource.clip = lowHealthMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SetLowHealth(bool low)
    {
        isLowHealth = low;

        if (low)
        {
            PlayLowHealth();
        }
        else
        {
            if (!isChasing)
                PlayNormal();
        }
    }
}
