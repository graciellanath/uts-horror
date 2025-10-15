using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float interactDistance = 3f;
    public Transform player;
    public AudioSource pickupSound; // optional

    private bool isNear = false;

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < interactDistance)
        {
            if (!isNear)
            {
                isNear = true;
                Debug.Log("Tekan [E] untuk mengambil kunci");
                // TODO: show UI prompt
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Pickup();
            }
        }
        else if (isNear)
        {
            isNear = false;
            // TODO: hide UI prompt
        }
    }

    void Pickup()
    {
        PlayerHasKey.hasKey = true;
        Debug.Log("🔑 Kunci diambil");
        if (pickupSound != null) pickupSound.Play();
        Destroy(gameObject);
    }
}
