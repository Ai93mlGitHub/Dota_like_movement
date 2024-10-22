using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour, IDamageble
{
    [SerializeField] private float _healthValue = 100;
    [SerializeField] private float _idleTimeThreshold = 5f;
    [SerializeField] private float _patrolRadius = 10f;
    [SerializeField] private float _maxNavMeshSampleDistance = 2f;
    [SerializeField] private int _attemptOfPatrol = 10;
    [SerializeField] private float _degreeOfInjury = 0.3f;

    private NavMeshAgent _agent;
    private PlayerView _playerView;

    private float _idleTimer = 0f;
    private bool _isPatrolling = false;

    public bool IsDead { get; private set; } = false;
    public Health Health { get; private set; }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerView = GetComponent<PlayerView>();
        Health = new Health(_healthValue);
        InputController.OnClick += MoveToTargetPoint;
    }

    private void OnDestroy() => InputController.OnClick -= MoveToTargetPoint;

    private void Update()
    {
        if (IsDead)
            return;

        UpdateMovementState();
    }

    public void UpdateMovementState()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                _playerView.StopRunning();
                _idleTimer += Time.deltaTime;

                if (_idleTimer >= _idleTimeThreshold && !_isPatrolling)
                {
                    _isPatrolling = true;
                    Patrol();
                }
            }
        }
        else
        {
            _idleTimer = 0f;
            _isPatrolling = false;
        }
    }

    private void AgentStop()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _agent.ResetPath();
    }

    private void MoveToTargetPoint(Vector3 point)
    {
        if (!IsDead)
        {
            _agent.SetDestination(point);
            _playerView.StartRunning();
            _idleTimer = 0f;
            _isPatrolling = false;
        }
    }

    private void Patrol()
    {
        Vector3 randomPoint;

        if (TryGetRandomPoint(transform.position, _patrolRadius, out randomPoint))
        {
            _agent.SetDestination(randomPoint);
            _playerView.StartRunning();
        }
        else
        {
            Debug.Log("Нет места для патрулирования");
        }
    }

    private bool TryGetRandomPoint(Vector3 center, float radius, out Vector3 result)
    {
        for (int i = 0; i < _attemptOfPatrol; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
            randomDirection += center;
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

    public void TakeDamage(float damage)
    {
        Health.ChangeHealthValue(-damage);
        _playerView.Hit();

        if (Health.HealthValue <= 0)
        {
            IsDead = true;
            AgentStop();
            _playerView.Death();
        }

        if (Health.HealthValue / Health.MaxHealth <= _degreeOfInjury)
            _playerView.SwitchLayerToInjured();
    }
}
