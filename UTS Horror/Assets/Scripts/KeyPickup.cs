using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public DoorController doorController;
    public Transform player;
    public float interactDistance = 3f; // jarak interaksi

    private bool isNear = false;
    private bool isPickedUp = false;

    void Update()
    {
        if (isPickedUp || player == null) return; // kalau sudah diambil atau player hilang, hentikan update

        float distance = Vector3.Distance(player.position, transform.position);
        isNear = distance < interactDistance;

        // Tekan E hanya saat dekat
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            PickUpKey();
        }
    }

    void PickUpKey()
    {
        if (doorController != null)
        {
            doorController.playerHasKey = true;
            Debug.Log("🔑 Kunci diambil!");
        }

        isPickedUp = true;
        Destroy(gameObject); // hapus object kunci
    }
}