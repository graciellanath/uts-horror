using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MonsterFootstep : MonoBehaviour
{
    [Header("Footstep Audio")]
    public AudioClip footstepClip;

    [Header("Timing")]
    [Tooltip("Jarak antara kiri dan kanan")]
    public float footGap = 0.12f;

    [Tooltip("Jeda antar langkah penuh")]
    public float stepInterval = 0.8f;

    public float speedThreshold = 0.1f;

    private AudioSource audioSource;
    private MonsterAI monsterAI;
    private Rigidbody rb;
    private bool isStepping;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        monsterAI = GetComponent<MonsterAI>();
        rb = GetComponent<Rigidbody>();

        audioSource.clip = footstepClip;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (monsterAI == null || rb == null) return;

        if (monsterAI.isAttacking)
        {
            StopAllCoroutines();
            audioSource.Stop();
            isStepping = false;
            return;
        }

        Vector3 vel = rb.linearVelocity;
        vel.y = 0;

        if (vel.magnitude < speedThreshold || isStepping)
            return;

        StartCoroutine(StepRoutine());
    }

    IEnumerator StepRoutine()
    {
        isStepping = true;

        // Kaki kiri
        audioSource.pitch = Random.Range(0.88f, 0.92f);
        audioSource.Play();
        yield return new WaitForSeconds(footGap);

        // Kaki kanan
        audioSource.pitch = Random.Range(0.90f, 0.95f);
        audioSource.Play();
        yield return new WaitForSeconds(stepInterval);

        isStepping = false;
    }
}
