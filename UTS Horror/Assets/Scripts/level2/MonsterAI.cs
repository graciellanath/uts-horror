using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MonsterAI : MonoBehaviour
{
    [Header("Komponen")]
    public Animator anim;
    public Transform player;

    private Rigidbody rb;
    private MonsterAttack monsterAttack;

    [Header("Patroli")]
    public Transform movePointParent;
    private List<Transform> patrolPoints = new List<Transform>();
    private int currentIndex = 0;
    private bool movingForward = true;

    [Header("Gerakan")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 6f;

    [Header("Detection & Combat")]
    public float chaseRange = 10f;
    public float attackRange = 2f; 
    public LayerMask obstacleMask; 

    [Header("Attack Settings")]
    public float damageDelay = 0.5f; 
    public float attackCooldown = 2f;

    [Header("Status (Read Only)")]
    public bool isChasing = false;
    public bool isAttacking = false;
    public bool canSeePlayer = false; 

    private bool isPlayerInZone = false;
    private Coroutine attackCoroutine;

    public bool IsChasing()
    {
        return isChasing;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 🔥 SETTING FISIKA KINEMATIC (Agar gerakan stabil)
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (anim == null) anim = GetComponentInChildren<Animator>();
        if (anim != null) anim.applyRootMotion = false;

        monsterAttack = GetComponent<MonsterAttack>();

        if (movePointParent != null)
        {
            foreach (Transform t in movePointParent) patrolPoints.Add(t);
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        canSeePlayer = CheckVisibility(distance);

        HandleState(distance);
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        HandleMovement();
    }


    private void HandleState(float distance)
    {
        if (isAttacking) return;

        // kejar jika masuk range bagian atau terlihat
        if (isPlayerInZone || canSeePlayer)
        {
            isChasing = true;
        }
        else if (isChasing && distance > chaseRange * 1.5f)
        {
            isChasing = false;
        }

        // chase & dekat = pukul
        if (isChasing && distance <= attackRange)
        {
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackRoutine());
            }
        }
    }


    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        SetAnim(false, true);
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);

        yield return new WaitForSeconds(damageDelay);

        float currentDistance = Vector3.Distance(transform.position, player.position);
        if (currentDistance <= attackRange + 1.0f && CheckVisibility(currentDistance))
        {
            monsterAttack?.TryAttack();
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        attackCoroutine = null;
    }


    private void HandleMovement()
    {
        if (isAttacking) return;

        if (isChasing)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);

            if (!canSeePlayer && !isPlayerInZone)
            {
                SetAnim(false, false);
                return;
            }

            if (distToPlayer > attackRange - 0.2f)
            {
                MoveTowards(player.position);
            }
            else
            {
                // 4. Sudah dekat = diam (tunggu serang)
                SetAnim(false, false);
            }
        }
        else
        {
            // Tidak chasing = patroli
            Patrol();
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0; // Kunci sumbu Y

        if (dir != Vector3.zero)
        {
            // Rotasi halus
            Quaternion rot = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.fixedDeltaTime));
            rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
        }

        SetAnim(true, false);
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Vector3 target = patrolPoints[currentIndex].position;
        Vector3 flatTarget = new Vector3(target.x, transform.position.y, target.z);

        if (Vector3.Distance(transform.position, flatTarget) > 0.5f)
        {
            MoveTowards(target);
        }
        else
        {
            NextPatrolPoint();
        }
    }

    private void NextPatrolPoint()
    {
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
        currentIndex = Mathf.Clamp(currentIndex, 0, patrolPoints.Count - 1);
    }


    private bool CheckVisibility(float distance)
    {
        if (distance > chaseRange) return false;
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.5f;

        // Cek Linecast: Apakah ada garis lurus dari Monster ke Player yang terhalang OBSTACLE?
        if (Physics.Linecast(origin, target, obstacleMask))
        {
            return false; // Ada tembok menghalangi
        }

        return true; // Pandangan bersih 
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInZone = false;
    }


    private void SetAnim(bool walk, bool attack)
    {
        if (anim == null) return;

        anim.SetBool("isJalan", walk);

        // Trigger serang hanya dipanggil sekali saat true
        if (attack) anim.SetTrigger("isSerang");
    }


    private void OnDrawGizmos()
    {
        if (player == null) return;

        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.5f;

        bool hitObstacle = Physics.Linecast(origin, target, obstacleMask);
        Gizmos.color = hitObstacle ? Color.red : Color.green;
        Gizmos.DrawLine(origin, target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}