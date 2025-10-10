using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [Header("Komponen")]
    public Animator anim;
    public Transform player;

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
    public LayerMask obstacleMask; // tambahkan layer tembok di sini di Inspector (misal: "Wall")

    [Header("Status Internal")]
    private bool isChasing = false;
    private bool isAttacking = false;

    private void Start()
    {
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

        if (movePointParent == null)
        {
            GameObject mp = GameObject.FindGameObjectWithTag("MovePoint");
            if (mp != null) movePointParent = mp.transform;
        }

        if (movePointParent != null)
        {
            foreach (Transform t in movePointParent)
            {
                patrolPoints.Add(t);
            }
        }

        if (patrolPoints.Count > 0)
            transform.LookAt(patrolPoints[0]);
    }

    private void Update()
    {
        if (player == null || patrolPoints.Count == 0) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = CanSeePlayer(); // cek apakah player kelihatan (tidak ketutup tembok)

        if (isChasing)
        {
            if (!canSeePlayer) // kalau kehilangan line of sight, balik ke patrol
            {
                isChasing = false;
                isAttacking = false;
                anim.SetBool("isSerang", false);
                currentIndex = FindNearestPointIndex();
                return;
            }

            if (distanceToPlayer <= attackRange)
            {
                isAttacking = true;
                anim.SetBool("isSerang", true);
                anim.SetBool("isJalan", false);
            }
            else if (distanceToPlayer <= chaseRange)
            {
                isAttacking = false;
                anim.SetBool("isSerang", false);
                anim.SetBool("isJalan", true);
                MoveTowards(player.position);
            }
            else
            {
                isChasing = false;
                anim.SetBool("isSerang", false);
                currentIndex = FindNearestPointIndex();
            }
        }
        else
        {
            Patrol();

            // hanya kejar jika dalam range dan terlihat langsung
            if (distanceToPlayer <= chaseRange && canSeePlayer)
            {
                isChasing = true;
            }
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.5f;
        Vector3 direction = target - origin;

        // lakukan raycast untuk memastikan tidak ada tembok di antara monster dan player
        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, chaseRange, ~0, QueryTriggerInteraction.Ignore))
        {
            // hanya detect kalau yang kena pertama adalah Player
            if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Vector3 targetPos = patrolPoints[currentIndex].position;
        float distance = Vector3.Distance(transform.position, targetPos);

        anim.SetBool("isJalan", true);
        anim.SetBool("isSerang", false);

        MoveTowards(targetPos);

        if (distance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    private void MoveTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            rotationSpeed * Time.deltaTime
        );

        transform.position += direction * moveSpeed * Time.deltaTime;
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
}
