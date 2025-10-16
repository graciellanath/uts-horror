using UnityEngine;

public class PlayerHasKey : MonoBehaviour
{
    public static bool hasKey = false;
    private void Awake()
    {
        // setiap game baru dimulai, reset status kunci
        hasKey = false;
        Debug.Log("Status kunci di-reset ke false oleh Awake().");
    }
}