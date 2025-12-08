using UnityEngine;

public class FirstAidPickup : MonoBehaviour
{
    public int healAmount = 25;
    public float interactDistance = 3f;

    // Player dicari otomatis via script, tidak perlu drag manual
    private Transform player;
    private playerfps playerScript;
    private bool isNear = false;

    void Start()
    {
        // CARI PLAYER OTOMATIS BERDASARKAN TAG "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerScript = playerObj.GetComponent<playerfps>();
        }
        else
        {
            Debug.LogError("Player tidak ditemukan! Pastikan object Hero Tag-nya sudah 'Player'.");
        }
    }

    void Update()
    {
        if (player == null || playerScript == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < interactDistance)
        {
            if (!isNear)
            {
                isNear = true;
                Debug.Log("Tekan [E] untuk ambil First Aid");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryPickup();
            }
        }
        else
        {
            isNear = false;
        }
    }

    void TryPickup()
    {
        if (playerScript.health >= playerScript.maxHealth)
        {
            Debug.Log("Darah penuh! Tidak bisa ambil.");
            return;
        }

        playerScript.Heal(healAmount);
        Debug.Log("First Aid diambil.");
        Destroy(gameObject); // Hapus object obat
    }
}