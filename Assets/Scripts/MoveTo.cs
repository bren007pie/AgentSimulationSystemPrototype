// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{

    public Transform goal;  // the goal target to get to
    NavMeshAgent agent; // the agent component

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = 5.0f;

    }
    private void Update()
    {
        // How to make an npc run away from a destination?
        agent.destination = goal.position;
    }
}
