using UnityEngine;

public class FirstAidPickup : MonoBehaviour
{
    public int healAmount = 25;          // Jumlah darah yang dipulihkan
    public float interactDistance = 3f;  // Jarak interaksi maksimum
    public Transform player;             // Referensi ke player (drag di Inspector)

    private playerfps playerScript;
    private bool isNear = false;

    void Start()
    {
        if (player != null)
            playerScript = player.GetComponent<playerfps>();
    }

    void Update()
    {
        if (player == null || playerScript == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // Cek apakah player berada dalam jarak interaksi
        if (distance < interactDistance)
        {
            if (!isNear)
            {
                isNear = true;
                Debug.Log("Tekan [E] untuk menggunakan First Aid");
            }

            // Tekan E untuk ambil first aid
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryPickup();
            }
        }
        else
        {
            if (isNear)
            {
                isNear = false;
                Debug.Log("Menjauh dari First Aid");
            }
        }
    }

    void TryPickup()
    {
        if (playerScript.health >= playerScript.maxHealth)
        {
            Debug.Log("💢 Darah penuh! Tidak bisa menggunakan First Aid.");
            return;
        }

        playerScript.Heal(healAmount);
        Debug.Log($"🩹 First Aid digunakan. Darah sekarang: {playerScript.health}%");
        Destroy(gameObject);
    }
}
