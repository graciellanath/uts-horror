using UnityEngine;

public class Level2MusicController : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Music Clips")]
    public AudioClip defaultMusic;           // Atmosphere_008
    public AudioClip chaseMusic;             // atmosphere_Rising_Loop_01
    public AudioClip monsterHitMusic;        // monster hit
    public AudioClip lowHealthChaseMusic;    // dikejar_monster2

    [Header("External References")]
    public MonsterAI monster;
    public playerfps player;

    // HAPUS: private bool isPlayingChase = false; (Ini penyebab warning, tidak perlu dipakai)

    void Start()
    {
        // Pastikan audio source memiliki clip awal
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

        // Ambil status dari MonsterAI (Pastikan MonsterAI punya method IsChasing)
        bool isChasing = monster.IsChasing();

        // Prioritas 1: Player Low Health + Dikejar (Paling Tegang)
        if (player.health <= 30 && isChasing)
        {
            SwitchTo(lowHealthChaseMusic, true);
        }
        // Prioritas 2: Dikejar Biasa (Health Masih Aman)
        else if (isChasing)
        {
            SwitchTo(chaseMusic, true);
        }
        // Prioritas 3: Tidak Dikejar (Kembali ke Normal)
        else
        {
            SwitchTo(defaultMusic, true);
        }
    }

    // Panggil fungsi ini dari script MonsterAttack saat player kena pukul
    public void OnMonsterHit()
    {
        if (monsterHitMusic != null)
        {
            audioSource.PlayOneShot(monsterHitMusic);
        }
    }

    private void SwitchTo(AudioClip clip, bool loop)
    {
        // PENCEGAHAN GLITCH:
        // Jika klip yang mau diputar SUDAH sedang main, jangan lakukan apa-apa.
        if (audioSource.clip == clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}