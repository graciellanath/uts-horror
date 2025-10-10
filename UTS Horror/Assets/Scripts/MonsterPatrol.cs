using UnityEngine;
using UnityEngine.AI;

public class MonsterPatrol : MonoBehaviour
{
    public float patrolRadius = 10f;
    public float patrolDelay = 3f;

    private NavMeshAgent agent;
    private Vector3 startPos;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        MoveToNextPoint();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Kalau sudah dekat target dan sudah lewat waktu delay, cari titik baru
        if (!agent.pathPending && agent.remainingDistance < 0.5f && timer >= patrolDelay)
        {
            MoveToNextPoint();
            timer = 0f;
        }
    }

    void MoveToNextPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += startPos;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }
}