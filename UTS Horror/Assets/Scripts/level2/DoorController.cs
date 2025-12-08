using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [Header("Settings")]
    public Animator doorAnimator;
    public float interactDistance = 3f;

    // Player dicari otomatis
    private Transform player;

    private bool isOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        // Cari Player Otomatis
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null || isAnimating) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerHasKey.hasKey && !isOpen)
            {
                StartCoroutine(OpenTheDoor());
            }
            else if (!PlayerHasKey.hasKey)
            {
                Debug.Log("Pintu terkunci! Cari kuncinya dulu.");
            }
        }
    }

    IEnumerator OpenTheDoor()
    {
        isAnimating = true;
        isOpen = true;

        if (doorAnimator != null)
            doorAnimator.SetBool("adaKunci", true); // Play animasi

        Debug.Log("Pintu terbuka...");

        yield return new WaitForSeconds(1f); // Tunggu animasi selesai
        isAnimating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Jika pintu sudah terbuka DAN yang masuk adalah Player
        if (isOpen && other.CompareTag("Player"))
        {
            Debug.Log("Menang! Pindah ke WinLvl2...");

            // Munculkan kursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Pindah Scene
            SceneManager.LoadScene("WinLvl2");
        }
    }
}