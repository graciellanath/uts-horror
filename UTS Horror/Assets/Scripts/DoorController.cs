using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;         // animator with bool parameter "isOpen"
    public float interactDistance = 3f;
    public Transform player;
    public AudioSource openSound;
    public AudioSource lockedSound;

    private bool isOpen = false;
    private bool isAnimating = false;
    private bool isNear = false;

    void Update()
    {
        if (player == null || isAnimating) return;

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < interactDistance)
        {
            if (!isNear)
            {
                isNear = true;
                if (PlayerHasKey.hasKey) Debug.Log("Tekan [E] untuk membuka pintu");
                else Debug.Log("Pintu terkunci. Butuh kunci.");
                // TODO: show UI prompt
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryToggleDoor();
            }
        }
        else if (isNear)
        {
            isNear = false;
            // TODO: hide UI prompt
        }
    }

    void TryToggleDoor()
    {
        if (!PlayerHasKey.hasKey)
        {
            if (lockedSound != null) lockedSound.Play();
            Debug.Log("Pintu terkunci! Kamu butuh kunci.");
            return;
        }

        StartCoroutine(ToggleDoorCoroutine());
    }

    IEnumerator ToggleDoorCoroutine()
    {
        isAnimating = true;
        isOpen = !isOpen;
        doorAnimator.SetBool("isOpen", isOpen); // ensure param name matches animator
        if (openSound != null) openSound.Play();

        Debug.Log("Pintu " + (isOpen ? "terbuka" : "tertutup"));
        yield return new WaitForSeconds(1f); // tunggu anim selesai (sesuaikan)
        isAnimating = false;
    }

    // optional helper for external checks
    public bool IsOpen() => isOpen;
}
