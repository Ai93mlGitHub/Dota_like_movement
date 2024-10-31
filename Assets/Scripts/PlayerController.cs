using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IDamageble
{
    public event Action OnDeath;
    public event Action OnJumpByNavmeshLink;
    public event Action OnStopJumpByNavMesh;
    public event Action OnStartRunning;
    public event Action OnStopRunning;
    public event Action OnHit;
    public event Action OnSwitchLayerToInjured;

    [SerializeField] private float _healthValue = 100;
    [SerializeField] private float _idleTimeThreshold = 5f;
    [SerializeField] private float _patrolRadius = 10f;
    [SerializeField] private float _maxNavMeshSampleDistance = 2f;
    [SerializeField] private int _attemptOfPatrol = 10;
    [SerializeField] private float _degreeOfInjury = 0.3f;
    [SerializeField] private float baseTraversalSpeed = 2.0f;
    [SerializeField] private AnimationCurve jumpCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private InputController _inputController;
    private NavMeshAgent _agent;
    private PlayerAI _playerAI;

    private float _idleTimer = 0f;
    private bool _isPatrolling = false;
    private Coroutine _traverseNavMeshLink;

    public bool IsDead { get; private set; } = false;
    public Health Health { get; private set; }

    void Start()
    {
        _inputController = FindObjectOfType<InputController>();
        _agent = GetComponent<NavMeshAgent>();
        Health = new Health(_healthValue);
        Health.IsDead += Death;
        _inputController.OnNavMeshPointSelected += MoveToTargetPoint;
        _playerAI = new PlayerAI(transform, _patrolRadius, _maxNavMeshSampleDistance, _attemptOfPatrol);
    }

    private void Death()
    {
        IsDead = true;
        AgentStop();
        OnDeath?.Invoke(); // Триггерим событие смерти
    }

    private void OnDestroy()
    {
        _inputController.OnNavMeshPointSelected -= MoveToTargetPoint;
        if (_traverseNavMeshLink != null)
        {
            StopCoroutine(_traverseNavMeshLink);
            _traverseNavMeshLink = null;
        }
    }

    private void Update()
    {
        if (IsDead)
            return;

        if (_agent.isOnOffMeshLink)
        {
            if (_traverseNavMeshLink == null)
            {
                _traverseNavMeshLink = StartCoroutine(TraverseNavMeshLink());
            }

            return;
        }

        UpdateMovementState();
    }

    private IEnumerator TraverseNavMeshLink()
    {
        OnJumpByNavmeshLink?.Invoke(); // Событие начала прыжка
        OffMeshLinkData linkData = _agent.currentOffMeshLinkData;
        Vector3 startPos = linkData.startPos;
        Vector3 endPos = linkData.endPos + Vector3.up * _agent.baseOffset;

        float linkLength = Vector3.Distance(startPos, endPos);
        float traversalDuration = linkLength * baseTraversalSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < traversalDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / traversalDuration;
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, normalizedTime);
            float jumpOffset = jumpCurve.Evaluate(normalizedTime);
            currentPos.y += jumpOffset;
            _agent.transform.position = currentPos;

            Vector3 targetRotation = endPos - startPos;
            targetRotation.y = 0;

            if (targetRotation != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(targetRotation);
            }

            yield return null;
        }

        OnStopJumpByNavMesh?.Invoke(); // Событие завершения прыжка
        _agent.CompleteOffMeshLink();
        _agent.transform.position = endPos;
        _agent.isStopped = false;
        _traverseNavMeshLink = null;
    }

    public void UpdateMovementState()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                OnStopRunning?.Invoke(); // Событие остановки бега
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
            OnStartRunning?.Invoke(); // Событие начала бега
            _idleTimer = 0f;
            _isPatrolling = false;
        }
    }

    private void Patrol()
    {
        if (_playerAI.TryGetRandomPoint(out Vector3 randomPoint))
        {
            _agent.SetDestination(randomPoint);
            OnStartRunning?.Invoke(); // Событие начала бега
        }
        else
        {
            Debug.Log("Нет места для патрулирования");
        }
    }

    public void TakeDamage(float damage)
    {
        Health.ChangeHealthValue(-damage);
        OnHit?.Invoke(); // Событие попадания

        if (Health.HealthValue / Health.MaxHealth <= _degreeOfInjury)
            OnSwitchLayerToInjured?.Invoke(); // Событие серьезного повреждения
    }
}
