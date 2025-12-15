using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameOverMusicController : MonoBehaviour
{
    public AudioClip gameOverMusic;
    [Range(0f, 1f)] public float volume = 0.8f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = gameOverMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        audioSource.Play();
    }
}
