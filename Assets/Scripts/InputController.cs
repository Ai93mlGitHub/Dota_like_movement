using UnityEngine;
using UnityEngine.AI;
using System;

public class InputController : MonoBehaviour
{
    [SerializeField] private float _maxDistanceToNavMesh = 1.0f;

    private int _leftClick = 0;

    public event Action<Vector3> OnNavMeshPointSelected;

    void Update()
    {
        if (Input.GetMouseButtonDown(_leftClick))
        {
            Vector3? navMeshPoint = GetNavMeshPoint();

            if (navMeshPoint.HasValue)
            {
                OnNavMeshPointSelected?.Invoke(navMeshPoint.Value);
            }
        }
    }

    private Vector3? GetNavMeshPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
        {
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, _maxDistanceToNavMesh, NavMesh.AllAreas))
            {
                return navMeshHit.position;
            }
        }
        return null;
    }
}
