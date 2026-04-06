using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        // Zombie tracking
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Move toward player
            agent.SetDestination(player.position);
        }

    
        float speed = agent.velocity.magnitude;
        anim.SetFloat("Speed", speed);
    }
}