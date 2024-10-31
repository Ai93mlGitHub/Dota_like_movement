using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Landmine : MonoBehaviour
{
    public event Action<Vector3, Quaternion> OnExplode;

    [SerializeField] private float _activationRadius = 5f;
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private float _explosionDelay = 2f;
    [SerializeField] private int _damage = 50;

    private bool _isActivated = false;
    private bool _isExploded = false;
    private float _explosionTimer = 0f;
    private SphereCollider _sphereCollider;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        _sphereCollider.radius = _activationRadius;
    }

    private void Update()
    {
        if (_isActivated && !_isExploded)
        {
            _explosionTimer += Time.deltaTime;

            if (_explosionTimer >= _explosionDelay)
                Explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActivated)
            _isActivated = true;
    }

    private void Explode()
    {
        _isExploded = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider collider in colliders)
        {
            IDamageble damageTarget = collider.gameObject.GetComponent<IDamageble>();

            if (damageTarget != null)
                damageTarget.TakeDamage(_damage);
        }

        OnExplode?.Invoke(transform.position, transform.rotation); // Вызов события взрыва

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _activationRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
