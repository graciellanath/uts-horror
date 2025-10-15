using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float interactDistance = 3f;
    public Transform player;

    private bool isNear = false;

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        isNear = dist < interactDistance;

        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        // Mengubah status kunci global menjadi true
        PlayerHasKey.hasKey = true;

        Debug.Log("🔑 Kunci diambil");

        // Hancurkan objek kunci setelah diambil
        Destroy(gameObject);
    }
}