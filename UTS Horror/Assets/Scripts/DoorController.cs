using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public Animator doorAnimator;      // Animator untuk pintu
    public bool playerHasKey = false;  // Apakah player sudah punya kunci (atur di Inspector)
    public float interactDistance = 3f; // Jarak interaksi dengan pintu

    [Header("References")]
    public Transform player;           // Transform dari player

    private bool isOpen = false;       // Status internal untuk memastikan pintu hanya terbuka sekali
    private bool isAnimating = false;  // Mencegah input berulang saat animasi berjalan

    void Update()
    {
        // Keluar jika player tidak ada atau pintu sedang beranimasi
        if (player == null || isAnimating) return;

        // Hitung jarak antara player dan pintu
        float distance = Vector3.Distance(player.position, transform.position);

        // Cek jika player dalam jangkauan dan menekan tombol 'E'
        if (distance < interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            // Hanya jalankan jika player punya kunci DAN pintu masih tertutup
            if (playerHasKey && !isOpen)
            {
                StartCoroutine(OpenTheDoor());
            }
            else if (!playerHasKey)
            {
                Debug.Log("🚪 Pintu terkunci! Kamu butuh kunci.");
            }
        }
    }

    IEnumerator OpenTheDoor()
    {
        isAnimating = true;

        // Mengubah status pintu menjadi terbuka, sehingga Coroutine ini tidak akan bisa dipanggil lagi
        isOpen = true;

        // Memicu animasi di Animator dengan menyetel parameter "adaKunci" menjadi true
        doorAnimator.SetBool("adaKunci", true);

        Debug.Log("🔑 Pintu berhasil dibuka dengan kunci!");

        // Beri jeda agar animasi selesai
        yield return new WaitForSeconds(1f);

        isAnimating = false;
    }
}