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
    private MusicController music;

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

    [Header("Parameter Serangan")]
    public float damageDelay = 0.5f;
    public float attackCooldown = 2.0f;

    [Header("Layer Mask")]
    public LayerMask obstacleMask;

    [Header("Status Internal")]
    public bool isChasing = false;
    public bool isAttacking = false;

    // >>> INI FUNGSI PENYELAMAT AGAR MUSIC CONTROLLER TIDAK ERROR <<<
    public bool IsChasing()
    {
        return isChasing;
    }
    // -------------------------------------------------------------

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        music = MusicController.instance;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (anim == null)
        {
            anim = GetComponent<Animator>();
            if (anim == null) anim = GetComponentInChildren<Animator>();
        }
        if (anim != null) anim.applyRootMotion = false;

        monsterAttack = GetComponent<MonsterAttack>();

        if (movePointParent == null)
        {
            GameObject mp = GameObject.FindGameObjectWithTag("MovePoint");
            if (mp != null) movePointParent = mp.transform;
        }
        if (movePointParent != null)
        {
            foreach (Transform t in movePointParent) patrolPoints.Add(t);
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        HandleStates(distance);

        if (music != null)
        {
            if (isChasing) music.PlayChase();
            else music.PlayNormal();
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        HandleMovement();
    }

    private void HandleStates(float distanceToPlayer)
    {
        if (isAttacking) return;

        bool canSee = CanSeePlayer(distanceToPlayer);

        if (!isChasing && canSee)
        {
            isChasing = true;
        }
        else if (isChasing && (distanceToPlayer > chaseRange * 1.5f))
        {
            isChasing = false;
        }

        if (isChasing)
        {
            if (distanceToPlayer <= attackRange)
            {
                StartCoroutine(AttackRoutine());
            }
            else
            {
                SetAnim(true, false);
            }
        }
        else
        {
            SetAnim(true, false);
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        rb.linearVelocity = Vector3.zero;
        SetAnim(false, true);

        if (player != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir);
        }

        yield return new WaitForSeconds(damageDelay);

        if (player != null)
        {
            float currentDistance = Vector3.Distance(transform.position, player.position);
            if (currentDistance <= attackRange + 0.8f)
            {
                monsterAttack?.TryAttack();
            }
        }

        yield return new WaitForSeconds(attackCooldown - damageDelay);
        isAttacking = false;
    }

    private void HandleMovement()
    {
        if (isAttacking)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        if (isChasing)
        {
            MoveTowards(player.position);
        }
        else
        {
            if (patrolPoints.Count == 0) return;
            Vector3 target = patrolPoints[currentIndex].position;
            MoveTowards(target);

            if (Vector3.Distance(transform.position, target) < 1f)
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
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime));
        }
    }

    private void SetAnim(bool isWalking, bool triggerAttack)
    {
        if (anim == null) return;
        anim.SetBool("isJalan", isWalking);
        if (triggerAttack) anim.SetTrigger("isSerang");
    }

    private bool CanSeePlayer(float dist)
    {
        if (dist > chaseRange) return false;
        Vector3 origin = transform.position + Vector3.up * 1.0f;
        Vector3 target = player.position + Vector3.up * 1.0f;
        Vector3 dir = target - origin;
        if (Physics.Raycast(origin, dir.normalized, out RaycastHit hit, dist, ~obstacleMask))
        {
            if (hit.collider.CompareTag("Player")) return true;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}