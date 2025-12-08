using UnityEngine;

public class DynamicMusicLevel2 : MonoBehaviour
{
    public static DynamicMusicLevel2 instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Music Clips")]
    public AudioClip normalMusic;            // Atmosphere_008
    public AudioClip chaseMusic;             // atmosphere_Rising_Loop_01
    public AudioClip monsterAttackMusic;     // saat kena hit
    public AudioClip lowHealthChaseMusic;    // dikejar_monster2

    private bool isInChase = false;
    private bool isLowHealth = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayNormalMusic();
    }

    // ---------------------------------------------------------
    // PUBLIC METHODS dipanggil dari luar (ZombieAI, PlayerHealth, dll)
    // ---------------------------------------------------------

    public void PlayNormalMusic()
    {
        isInChase = false;
        isLowHealth = false;
        SwitchMusic(normalMusic);
    }

    public void StartChase()
    {
        if (isLowHealth) return;

        isInChase = true;
        SwitchMusic(chaseMusic);
    }

    public void StopChase()
    {
        isInChase = false;
        if (!isLowHealth)
            SwitchMusic(normalMusic);
    }

    public void MonsterHit()
    {
        audioSource.PlayOneShot(monsterAttackMusic);
    }

    public void LowHealthChase()
    {
        isLowHealth = true;
        SwitchMusic(lowHealthChaseMusic);
    }


    // ---------------------------------------------------------
    // Helper
    // ---------------------------------------------------------
    private void SwitchMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
