using UnityEngine;
using UnityEngine.AI; // Needed for NavMesh

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        // This finds your player so the zombie knows where to go
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

        // Fix the "weird" animation by sending speed to the Animator
        // If speed is 0, it plays Idle. If speed is > 0, it plays Walk.
        float speed = agent.velocity.magnitude;
        anim.SetFloat("Speed", speed);
    }
}