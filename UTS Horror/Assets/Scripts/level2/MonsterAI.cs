using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MonsterAI : MonoBehaviour
{
    [Header("Komponen")]
    public Animator anim;
    public Transform player;
    private Rigidbody rb;
    private MonsterAttack monsterAttack; // referensi ke script attack

    [Header("Patroli")]
    public Transform movePointParent;
    private List<Transform> patrolPoints = new List<Transform>();
    private int currentIndex = 0;
    private bool movingForward = true;

    [Header("Parameter Gerak")]
    public float moveSpeed = 2f;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float rotationSpeed = 5f;

    [Header("Layer Mask")]
    public LayerMask obstacleMask;

    [Header("Status Internal")]
    private bool isChasing = false;
    private bool isAttacking = false;

    [Header("Debug")]
    public bool showDebugLog = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (anim == null)
        {
            anim = GetComponent<Animator>();
            if (anim == null)
                anim = GetComponentInChildren<Animator>();
        }

        if (anim != null)
            anim.applyRootMotion = false;

        // hubungkan ke script MonsterAttack
        monsterAttack = GetComponent<MonsterAttack>();

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

    private void Update()
    {
        if (player == null || patrolPoints.Count == 0) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = CanSeePlayer();

        HandleStates(distanceToPlayer, canSeePlayer);
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        HandleMovement();
    }

    private void HandleStates(float distanceToPlayer, bool canSeePlayer)
    {
        if (isChasing)
        {
            //= berhenti ngejar kalau gabisa lihat player atau terlalu jauh
            if (!canSeePlayer || distanceToPlayer > chaseRange)
            {
                isChasing = false;
                isAttacking = false;
                SetAnimationState(true, false);
                currentIndex = FindNearestPointIndex();
                return;
            }

            // serang kalau dekat
            if (distanceToPlayer <= attackRange)
            {
                isAttacking = true;
                SetAnimationState(false, true);

                // panggil serangan dari MonsterAttack
                if (monsterAttack != null)
                    monsterAttack.TryAttack();
            }
            else
            {
                isAttacking = false;
                SetAnimationState(true, false);
            }
        }
        else
        {
            // kejar
            if (canSeePlayer && distanceToPlayer <= chaseRange)
            {
                isChasing = true;
                isAttacking = false;
            }
        }
    }

    private void HandleMovement()
    {
        if (isAttacking) return; // ga gerak saat nyerang

        if (isChasing)
        {
            MoveTowards(player.position);
        }
        else
        {
            if (patrolPoints.Count == 0) return;

            Vector3 target = patrolPoints[currentIndex].position;
            MoveTowards(target);

            if (Vector3.Distance(transform.position, target) < 0.5f)
            {
                GoToNextPoint();
            }
        }
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        Vector3 dir = (targetPosition - transform.position).normalized;
        dir.y = 0;

        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

        if (dir != Vector3.zero)
            RotateTowards(dir);
    }

    private void RotateTowards(Vector3 dir)
    {
        Quaternion targetRot = Quaternion.LookRotation(dir);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime));
    }

    private void SetAnimationState(bool walking, bool attacking)
    {
        if (anim == null) return;
        anim.SetBool("isJalan", walking);
        if (attacking)
            anim.SetTrigger("isSerang");
    }

    private bool CanSeePlayer()
    {
        if (player == null) return false;
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.5f;
        Vector3 dir = target - origin;
        float dist = dir.magnitude;

        if (Physics.Raycast(origin, dir.normalized, out RaycastHit hit, dist, ~obstacleMask))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    private void GoToNextPoint()
    {
        if (patrolPoints.Count == 0) return;

        if (movingForward)
        {
            currentIndex++;
            if (currentIndex >= patrolPoints.Count)
            {
                currentIndex = patrolPoints.Count - 2;
                movingForward = false;
            }
        }
        else
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = 1;
                movingForward = true;
            }
        }
    }

    private int FindNearestPointIndex()
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
