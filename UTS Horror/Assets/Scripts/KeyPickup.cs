using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public DoorController doorController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorController.playerHasKey = true;
            Debug.Log("Kunci diambil!");
            Destroy(gameObject); // hilangkan kunci dari scene
        }
    }
}
