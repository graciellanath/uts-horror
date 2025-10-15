using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public DoorController doorController;
    public Transform player;
    public float interactDistance = 3f; // jarak interaksi
    private bool isNear = false;

    void Update()
    {
        // Cek jarak antara player dan kunci
        if (Vector3.Distance(player.position, transform.position) < interactDistance)
        {
            isNear = true;

            // Tekan E untuk ambil kunci
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpKey();
            }
        }
        else
        {
            isNear = false;
        }
    }

    void PickUpKey()
    {
        if (doorController != null)
        {
            doorController.playerHasKey = true;
            Debug.Log("Kunci diambil!");
            Destroy(gameObject); // hapus kunci dari scene
        }
    }
}
