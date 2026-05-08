using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Chase Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float damage = 10f;

    [Header("Animation")]
    public float animationDampTime = 0.1f;

    private NavMeshAgent agent;
    private Animator animator;
    private float attackTimer;
    private bool isDead = false;
    private Transform player;

    // Animator hashes
    private int _animSpeed;
    private int _animAttack;
    private int _animDead;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Cache animator parameters
        _animSpeed = Animator.StringToHash("Speed");
        _animAttack = Animator.StringToHash("isAttacking");
        _animDead = Animator.StringToHash("isDead");

        // Auto find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // NavMesh Agent settings for better pathfinding
        if (agent != null)
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            agent.radius = 0.5f;
            agent.angularSpeed = 180f;
            agent.acceleration = 8f;
            agent.stoppingDistance = attackRange - 0.5f;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(
            transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Stop and attack
            agent.isStopped = true;

            // Face the player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    Time.deltaTime * 5f);

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                animator?.SetBool(_animAttack, true);
                attackTimer = attackCooldown;

                // Deal damage to player
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                    playerHealth.TakeDamage(damage);
            }

            // Update animation speed to 0 when stopped
            UpdateAnimationSpeed(0f);
        }
        else
        {
            // Chase player
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator?.SetBool(_animAttack, false);

            // Update animation speed based on agent velocity
            float speed = agent.velocity.magnitude;
            UpdateAnimationSpeed(speed);
        }
    }

    void UpdateAnimationSpeed(float speed)
    {
        if (animator == null) return;

        // Use Speed parameter if it exists
        // otherwise fall back to isAttacking bool
        animator.SetFloat(_animSpeed, speed, 
            animationDampTime, Time.deltaTime);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (agent != null)
            agent.isStopped = true;

        animator?.SetBool(_animDead, true);

        // Disable collider so no more hits register
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // Destroy after death animation
        Destroy(gameObject, 3f);
    }
}
