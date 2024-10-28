using UnityEngine;
using UnityEngine.AI;

public class PlayerAI
{
    private readonly Transform _playerTransform;
    private readonly float _patrolRadius;
    private readonly float _maxNavMeshSampleDistance;
    private readonly int _attemptOfPatrol;

    public PlayerAI(Transform playerTransform, float patrolRadius, float maxNavMeshSampleDistance, int attemptOfPatrol)
    {
        _playerTransform = playerTransform;
        _patrolRadius = patrolRadius;
        _maxNavMeshSampleDistance = maxNavMeshSampleDistance;
        _attemptOfPatrol = attemptOfPatrol;
    }

    public bool TryGetRandomPoint(out Vector3 result)
    {
        for (int i = 0; i < _attemptOfPatrol; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _patrolRadius;
            randomDirection += _playerTransform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, _maxNavMeshSampleDistance, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }
}
