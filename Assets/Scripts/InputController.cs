using UnityEngine;
using UnityEngine.AI;
using System;

public class InputController : MonoBehaviour
{
    [SerializeField] private float _maxDistanceToNavMesh = 1.0f;

    private int _lefClick = 0;
    
    public static event Action<Vector3> OnClick;


    void Update()
    {
        if (Input.GetMouseButtonDown(_lefClick))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider != null)
                {
                    Vector3 targetPoint = hit.point;
                    NavMeshHit navMeshHit;

                    if (NavMesh.SamplePosition(targetPoint, out navMeshHit, _maxDistanceToNavMesh, NavMesh.AllAreas))
                        OnClick?.Invoke(navMeshHit.position);
                }
            }
        }
    }
}
