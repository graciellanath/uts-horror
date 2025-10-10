using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float patrolRadius = 10f;

    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 startPos;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        startPos = transform.position;

        MoveToRandomPoint();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= attackRange)
        {
            // Serang pemain
            agent.isStopped = true;
            anim.SetBool("isJalan", false);
            anim.SetBool("isSerang", true);
        }
        else if (distance <= detectionRange)
        {
            // Kejar pemain
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("isJalan", true);
            anim.SetBool("isSerang", false);
        }
        else
        {
            // Patroli acak
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                MoveToRandomPoint();
            }
            anim.SetBool("isJalan", true);
            anim.SetBool("isSerang", false);
        }
    }

    void MoveToRandomPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * patrolRadius + startPos;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }
}