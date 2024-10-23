using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class Landmine : MonoBehaviour
{
    [SerializeField] private float _activationRadius = 5f;
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private float _explosionDelay = 2f;
    [SerializeField] private int _damage = 50;
    [SerializeField] private LayerMask _ignoredLayers;
    [SerializeField] private ParticleSystem _explosionVFXPrefab;

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
        if (((1 << other.gameObject.layer) & _ignoredLayers) != 0)
            return;

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

        if (_explosionVFXPrefab != null)
            Instantiate(_explosionVFXPrefab, gameObject.transform.position, transform.rotation);

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
