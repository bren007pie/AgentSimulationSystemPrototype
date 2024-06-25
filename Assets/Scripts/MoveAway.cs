// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;

public class MoveAway : MonoBehaviour
{
    // Simple flee behaviour that calculates the vector away from the avoid object and moves there

    public Transform avoid_this;  // the target to avoid
    NavMeshAgent agent; // the agent component
    int run_multiplier = 1; // scales the destination target vector pointing away from the player
    float max_range = 30.0f; // max distance can point away

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = 5.0f;

    }
    private void Update()
    {
        // Stolen from: https://discussions.unity.com/t/navmesh-flee-ai-flee-from-player/126809
        Vector3 runTo = transform.position + ((transform.position - avoid_this.position) * run_multiplier);
        float distance = Vector3.Distance(transform.position, avoid_this.position);
        if (distance < max_range) agent.SetDestination(runTo);
    }
}
