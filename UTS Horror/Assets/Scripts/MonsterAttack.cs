using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public playerfps playerSc;

    private void Start()
    {
        // Mencari referensi player jika belum di-assign di Inspector
        if (playerSc == null)
        {
            playerSc = FindObjectOfType<playerfps>();
        }
    }

    // Fungsi ini tidak lagi diperlukan untuk cooldown, 
    // tapi tetap ada agar tidak error jika dipanggil dari script lain.
    public void TryAttack()
    {
        // Logika cooldown telah dihapus.
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