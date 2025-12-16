using UnityEngine;

public class Level2MusicController : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Music Clips")]
    public AudioClip defaultMusic;           // Lagu Santai
    public AudioClip chaseMusic;             // Lagu Dikejar
    public AudioClip monsterHitMusic;        // SFX Kena Pukul (Durasi Pendek)
    public AudioClip lowHealthChaseMusic;    // Lagu Sekarat (Jantung/Tegang)

    [Header("External References")]
    public MonsterAI monster;
    public playerfps player;

    void Start()
    {
        // 1. Matikan MusicController Global (dari menu) jika ada
        if (MusicController.instance != null)
        {
            Destroy(MusicController.instance.gameObject);
        }

        // 2. Mulai musik default
        if (audioSource.clip == null)
        {
            audioSource.clip = defaultMusic;
        }
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        if (monster == null || player == null) return;

        bool isChasing = monster.IsChasing();

        // --- LOGIKA PRIORITAS MUSIK (URUTAN SANGAT PENTING) ---

        // 1. PRIORITAS TERTINGGI: Pemain Sekarat (<= 25%)
        // Lagu ini akan bunyi TERUS (Looping) mau dikejar atau tidak.
        if (player.health <= 25)
        {
            SwitchTo(lowHealthChaseMusic, true);
        }
        // 2. PRIORITAS KEDUA: Pemain Sehat TAPI Dikejar Monster
        else if (isChasing)
        {
            SwitchTo(chaseMusic, true);
        }
        // 3. PRIORITAS TERAKHIR: Pemain Sehat & Aman
        else
        {
            SwitchTo(defaultMusic, true);
        }
    }

    // Dipanggil saat kena pukul (Hanya SFX, tidak ganggu BGM)
    public void OnMonsterHit()
    {
        if (monsterHitMusic != null)
        {
            // PlayOneShot: Bunyi "Deshh!" menimpa lagu sebentar tanpa memotong/restart lagu
            audioSource.PlayOneShot(monsterHitMusic);
        }
    }

    private void SwitchTo(AudioClip clip, bool loop)
    {
        // PENCEGAHAN GLITCH/RESTART:
        // Jika lagu yang diminta SAMA dengan yang sedang main, JANGAN restart.
        if (audioSource.clip == clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}