using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public playerfps playerSc;

    private void Start()
    {
        // cari script playerfps di scene 
        if (playerSc == null)
        {
            playerSc = FindObjectOfType<playerfps>();
        }
    }

    public void TryAttack()
    {
    }

    // Fungsi ini dipanggil dari Animation Event untuk memberikan damage
    public void DealDamage()
    {
        if (playerSc != null)
        {
            playerSc.TakeDamage(25);
            Debug.Log("Player terkena serangan!");
        }
        else
        {
            Debug.LogWarning("Referensi 'playerSc' belum terhubung di MonsterAttack!");
        }
    }
}