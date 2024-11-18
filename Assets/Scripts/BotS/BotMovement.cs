using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BotMovement : MonoBehaviour
{
    [SerializeField][Range(0, 7)] private float speed = 3.5f;
    [SerializeField][Range(0.1f, 1f)] private float rotationSpeed = 0.5f;

    private NavMeshAgent agent;

    public float MovementMagnitude => agent.velocity.magnitude / speed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        InitializeAgent();
    }

    private void InitializeAgent()
    {
        agent.speed = speed;
        agent.angularSpeed = rotationSpeed * 360f; // ���������� � ������� �� �������
    }

    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    public void StopMovement()
    {
        agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        agent.isStopped = false;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        agent.speed = speed;
    }

    public void SetRotationSpeed(float newRotationSpeed)
    {
        rotationSpeed = newRotationSpeed;
        agent.angularSpeed = rotationSpeed * 360f;
    }
}