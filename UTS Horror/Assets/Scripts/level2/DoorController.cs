using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public Animator doorAnimator;        // animator untuk mengontrol animasi pintu (buka/tutup)
    public float interactDistance = 3f;  // jarak maksimum agar pemain bisa berinteraksi dengan pintu

    [Header("References")]
    public Transform player;             // ref posisi pemain di dalam scene

    private bool isOpen = false;        
    private bool isAnimating = false;    

    void Update()
    {
        // kalau pemain belum diassign atau pintu sedang animasi, stop pengecekan
        if (player == null || isAnimating) return;

        // hitung jarak antara pemain dan pintu
        float distance = Vector3.Distance(player.position, transform.position);

        // jika pemain cukup dekat dan menekan tombol "E"
        if (distance < interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            // jika pemain punya kunci dan pintu belum terbuka, buka pintu
            if (PlayerHasKey.hasKey && !isOpen)
            {
                StartCoroutine(OpenTheDoor());
            }
            // jika pemain tidak punya kunci, tampilkan pesan di Console
            else if (!PlayerHasKey.hasKey)
            {
                Debug.Log("🚪 Pintu terkunci! Kamu butuh kunci.");
            }
        }
    }

    // coroutine untuk membuka pintu (agar bisa ada jeda waktu)
    IEnumerator OpenTheDoor()
    {
        isAnimating = true;                  
        isOpen = true;                       
        doorAnimator.SetBool("adaKunci", true); // Jalankan animasi buka pintu
        Debug.Log("🔑 Pintu berhasil dibuka dengan kunci!");
        yield return new WaitForSeconds(1f);  
        isAnimating = false;                  
    }

    // ketika pemain masuk ke collider trigger
    private void OnTriggerEnter(Collider other)
    {
        // jika pintu sudah terbuka dan yang masuk adalah Player
        if (isOpen && other.CompareTag("Player"))
        {
            // Pindah ke scene "Win" menang
            SceneManager.LoadScene("Win");
        }
    }
}
