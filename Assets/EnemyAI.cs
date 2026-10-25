using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float attackRange = 2.0f;

    [Header("Combat")]
    public float attackDamage = 15f;
    public float attackRate = 1.0f; // Attack every 1 second

    private Transform player;
    private PlayerHealth playerHealth;
    private NavMeshAgent agent;
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange - 0.3f;

        // Find the player object tagged "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogError("EnemyAI: Could not find any GameObject tagged 'Player'!");
        }
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Move toward player
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            // In range: stop and attack
            agent.isStopped = true;

            // Rotate smoothly to face player
            Vector3 lookDirection = (player.position - transform.position).normalized;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 6f);
            }

            // Attack on cooldown
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
    }
}
