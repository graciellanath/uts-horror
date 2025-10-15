using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public playerfps playerSc;
    public float attackCooldown = 10f;
    private float attackTimer = 0f;

    private void Start()
    {
        if (playerSc == null)
            playerSc = FindObjectOfType<playerfps>();
    }

    public void TryAttack()
    {
        if (attackTimer <= 0)
        {
            attackTimer = attackCooldown;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    // Fungsi ini dipanggil dari Animation Event
    public void DealDamage()
    {
        if (playerSc != null)
        {
            playerSc.TakeDamage(25); // langsung Game Over
            Debug.Log("Player terkena serangan!");
        }
        else
        {
            Debug.LogWarning("Player belum terhubung di MonsterAttack!");
        }
    }
}
