using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float attackRange = 2.0f;

    [Header("Combat")]
    public int attackDamage = 15;
    public float attackRate = 1.0f;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private PlayerHealth playerHealth;
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange * 0.8f;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (playerTransform == null || Time.timeScale == 0f) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Keep pursuing the player
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(playerTransform.position);
        }

        // Attack when within range
        if (distanceToPlayer <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;
                AttackPlayer();
            }
        }
    }

    void AttackPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
