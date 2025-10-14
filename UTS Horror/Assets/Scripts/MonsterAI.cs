using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public Transform player;

    [Header("Patrol Points")]
    public Transform movePointParent;
    private List<Transform> patrolPoints = new List<Transform>();
    private int currentIndex = 0;
    private bool movingForward = true;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    [Header("Detection Settings")]
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public LayerMask obstacleMask;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    private float attackTimer = 0f;
    private bool isAttacking = false;
    private bool isChasing = false;

    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public int damage = 1;

    void Start()
    {
        // Cari Player otomatis kalau belum diassign
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // Ambil Animator otomatis
        if (anim == null)
        {
            anim = GetComponent<Animator>();
            if (anim == null) anim = GetComponentInChildren<Animator>();
        }

        // Root motion dimatikan (AI jalan via script)
        if (anim != null) anim.applyRootMotion = false;

        // Ambil patrol point dari parent yang ditandai tag MovePoint
        if (movePointParent == null)
        {
            GameObject mp = GameObject.FindGameObjectWithTag("MovePoint");
            if (mp != null) movePointParent = mp.transform;
        }

        if (movePointParent != null)
        {
            foreach (Transform t in movePointParent)
                patrolPoints.Add(t);
        }

        if (patrolPoints.Count > 0)
            transform.LookAt(patrolPoints[0]);
    }

    void Update()
    {
        if (player == null || patrolPoints.Count == 0) return;

        attackTimer -= Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = CanSeePlayer();

        if (isChasing)
        {
            if (!canSeePlayer)
            {
                isChasing = false;
                isAttacking = false;
                SetAnimationState(true, false);
                currentIndex = FindNearestPointIndex();
                return;
            }

            if (distanceToPlayer <= attackRange)
            {
                LookAtPlayer();

                if (!isAttacking && attackTimer <= 0f)
                {
                    StartCoroutine(AttackRoutine());
                    attackTimer = attackCooldown;
                }
            }
            else if (distanceToPlayer <= chaseRange)
            {
                if (!isAttacking)
                {
                    SetAnimationState(true, false);
                    MoveTowards(player.position);
                }
            }
            else
            {
                isChasing = false;
                isAttacking = false;
                SetAnimationState(true, false);
                currentIndex = FindNearestPointIndex();
            }
        }
        else
        {
            Patrol();
            if (distanceToPlayer <= chaseRange && canSeePlayer)
            {
                isChasing = true;
                isAttacking = false;
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        SetAnimationState(false, true);
        yield return new WaitForSeconds(1.2f); // tunggu sampai animasi selesai
        isAttacking = false;
    }

    void LookAtPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void SetAnimationState(bool walking, bool attacking)
    {
        if (anim == null) return;
        anim.SetBool("isJalan", walking);
        if (attacking)
        {
            anim.ResetTrigger("isSerang");
            anim.SetTrigger("isSerang");
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.5f;
        Vector3 direction = target - origin;
        float distance = direction.magnitude;

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distance))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }
        return false;
    }

    void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Vector3 targetPos = patrolPoints[currentIndex].position;
        float distance = Vector3.Distance(transform.position, targetPos);
        SetAnimationState(true, false);
        MoveTowards(targetPos);

        if (distance < 0.5f)
            GoToNextPoint();
    }

    void MoveTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime
            );
        }
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Count < 2) return;

        if (movingForward)
        {
            if (currentIndex < patrolPoints.Count - 1)
                currentIndex++;
            else
            {
                movingForward = false;
                currentIndex--;
            }
        }
        else
        {
            if (currentIndex > 0)
                currentIndex--;
            else
            {
                movingForward = true;
                currentIndex++;
            }
        }
    }

    int FindNearestPointIndex()
    {
        int nearest = 0;
        float minDist = Mathf.Infinity;
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, patrolPoints[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = i;
            }
        }
        return nearest;
    }

    // === Dipanggil oleh Animation Event pada animasi "isSerang" ===
    public void DealDamage()
    {
        if (attackPoint == null) return;

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerfps playerScript = hit.GetComponent<playerfps>();
                if (playerScript != null)
                {
                    playerScript.TakeDamage(damage); // langsung GameOver
                }
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
