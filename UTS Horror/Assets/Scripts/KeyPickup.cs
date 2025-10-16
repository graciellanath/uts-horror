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
        // ubah status kunci di PlayerHasKey
        PlayerHasKey.hasKey = true;

        Debug.Log("🔑 Kunci diambil");

        // hancurin object kunci saat diambil
        Destroy(gameObject);
    }
}