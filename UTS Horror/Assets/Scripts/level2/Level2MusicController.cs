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

    private bool isPlayingChase = false;

    void Start()
    {
        audioSource.clip = defaultMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        if (monster == null || player == null) return;

        // 1. Jika player low health + dikejar → music intense
        if (player.health <= 30 && monster.IsChasing())
        {
            SwitchTo(lowHealthChaseMusic, true);
            return;
        }

        // 2. Jika monster mengejar → music chase
        if (monster.IsChasing())
        {
            SwitchTo(chaseMusic, true);
            return;
        }

        // 3. Kalau monster tidak mengejar → music normal
        if (!monster.IsChasing() && audioSource.clip != defaultMusic)
        {
            SwitchTo(defaultMusic, true);
        }
    }

    // Untuk dipanggil saat monster hit player
    public void OnMonsterHit()
    {
        audioSource.PlayOneShot(monsterHitMusic);
    }

    private void SwitchTo(AudioClip clip, bool loop)
    {
        if (audioSource.clip == clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}
