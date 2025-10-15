using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;       // Animator pintu
    public bool isOpen = false;         // Status pintu
    public bool playerHasKey = false;   // Status kunci (apakah player punya)
    public float interactDistance = 3f; // Jarak interaksi
    public Transform player;            // Referensi player
    public AudioSource doorSound;       // Suara pintu (opsional)
    public AudioSource lockedSound;     // Suara pintu terkunci (opsional)

    void Update()
    {
        // Jika player cukup dekat
        if (Vector3.Distance(player.position, transform.position) < interactDistance)
        {
            // Tekan E untuk interaksi
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerHasKey)
                {
                    StartCoroutine(OpenDoorWithDelay());
                }
                else
                {
                    // Kalau belum punya kunci, bunyi pintu terkunci
                    if (lockedSound != null)
                    {
                        lockedSound.Play();
                    }
                    Debug.Log("Pintu terkunci! Kamu butuh kunci dulu.");
                }
            }
        }
    }

    IEnumerator OpenDoorWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Delay biar realistis
        isOpen = !isOpen;
        doorAnimator.SetBool("adaKunci", isOpen); // pakai parameter di Animator

        // Suara pintu terbuka
        if (doorSound != null)
        {
            doorSound.Play();
        }

        Debug.Log("Pintu " + (isOpen ? "terbuka" : "tertutup"));
    }
}
