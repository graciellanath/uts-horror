using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float interactDistance = 3f;
    private Transform player;

    void Start()
    {
        // Cari Player Otomatis
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist < interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        PlayerHasKey.hasKey = true; // Set status kunci jadi true
        Debug.Log("Kunci diambil!");
        Destroy(gameObject); // Hapus object kunci
    }
}