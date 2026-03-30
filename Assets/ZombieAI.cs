using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private Animator animator;
    private float attackTimer;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Auto find player if not assigned
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Stop and attack
            agent.isStopped = true;
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                animator.SetBool("isAttacking", true);
                attackTimer = attackCooldown;
            }
        }
        else
        {
            // Chase player
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isAttacking", false);
        }
    }

    public void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetBool("isDead", true);
    }
}