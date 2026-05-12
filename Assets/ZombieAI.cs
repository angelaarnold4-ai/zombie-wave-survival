using System.Collections;
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
    [Tooltip("Delay (seconds) into the attack animation before damage is applied. Match to your swing keyframe.")]
    public float damageDelay = 0.4f;

    private NavMeshAgent agent;
    private Animator animator;
    private float attackTimer;
    private bool isDead = false;
    private Transform player;

    private int _animSpeed;
    private int _animAttack;
    private int _animDead;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        _animSpeed = Animator.StringToHash("Speed");
        _animAttack = Animator.StringToHash("isAttacking");
        _animDead = Animator.StringToHash("isDead");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (agent != null)
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            agent.radius = 0.5f;
            agent.angularSpeed = 180f;
            agent.acceleration = 8f;
            agent.stoppingDistance = attackRange - 0.5f;
        }

        // FIX 1: Initialize timer to cooldown so zombie doesn't attack
        // instantly the first frame it reaches the player.
        attackTimer = attackCooldown;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(
            transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;

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
                attackTimer = attackCooldown;
                animator?.SetBool(_animAttack, true);

                // FIX 2: Delay damage so it lands mid-swing, not instantly.
                // FIX 3: Coroutine also resets isAttacking after the full swing.
                StartCoroutine(DealDamageAfterDelay(damageDelay));
            }

            UpdateAnimationSpeed(0f);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator?.SetBool(_animAttack, false);

            float speed = agent.velocity.magnitude;
            UpdateAnimationSpeed(speed);
        }
    }

    // FIX 2 & 3: Applies damage after the swing animation starts,
    // then resets the attack Bool so the animator returns to idle.
    private IEnumerator DealDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Zombie may have died or player walked away during the swing
        if (isDead || player == null) yield break;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange) yield break;

        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>()
            ?? player.GetComponentInChildren<PlayerHealth>();

        playerHealth?.TakeDamage(damage);

        // Wait for the remainder of the cooldown, then clear the attack animation
        yield return new WaitForSeconds(attackCooldown - delay);
        animator?.SetBool(_animAttack, false);
    }

    void UpdateAnimationSpeed(float speed)
    {
        if (animator == null) return;
        animator.SetFloat(_animSpeed, speed,
            animationDampTime, Time.deltaTime);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Stop any pending damage coroutines so a dying zombie can't still hit
        StopAllCoroutines();

        if (ScoreManager.instance != null)
            ScoreManager.instance.AddKill();

        if (agent != null)
            agent.isStopped = true;

        animator?.SetBool(_animDead, true);

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
            col.enabled = false;

        Destroy(gameObject, 3f);
    }
}
