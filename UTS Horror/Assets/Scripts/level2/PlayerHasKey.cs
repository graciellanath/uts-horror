using UnityEngine;

public class PlayerHasKey : MonoBehaviour
{
    public static bool hasKey = false;

    private void Awake()
    {
        hasKey = false; // Reset kunci setiap restart game
    }
}