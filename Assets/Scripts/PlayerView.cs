using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _jetParticles;
    [SerializeField] private AudioSource _footstepAudio; // AudioSource для звука шагов

    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int IsDeathKey = Animator.StringToHash("IsDeath");
    private readonly int IsJumping = Animator.StringToHash("IsJumping");
    private readonly int HitKey = Animator.StringToHash("Hit");
    private readonly int InjuredLayer = 1;

    private PlayerController _playerController;

    private void OnEnable()
    {
        _playerController = FindObjectOfType<PlayerController>();
        if (_playerController != null)
        {
            _playerController.OnDeath += Death;
            _playerController.OnJumpByNavmeshLink += JumpByNavmeshLink;
            _playerController.OnStopJumpByNavMesh += StopJumpByNavMesh;
            _playerController.OnStartRunning += StartRunning;
            _playerController.OnStopRunning += StopRunning;
            _playerController.OnHit += Hit;
            _playerController.OnSwitchLayerToInjured += SwitchLayerToInjured;
        }
    }

    private void OnDisable()
    {
        if (_playerController != null)
        {
            _playerController.OnDeath -= Death;
            _playerController.OnJumpByNavmeshLink -= JumpByNavmeshLink;
            _playerController.OnStopJumpByNavMesh -= StopJumpByNavMesh;
            _playerController.OnStartRunning -= StartRunning;
            _playerController.OnStopRunning -= StopRunning;
            _playerController.OnHit -= Hit;
            _playerController.OnSwitchLayerToInjured -= SwitchLayerToInjured;
        }
    }

    public void StartRunning() => _animator.SetBool(IsRunningKey, true);

    public void StopRunning() => _animator.SetBool(IsRunningKey, false);

    public void FootstepSound() => _footstepAudio.Play();

    public void Death() => _animator.SetBool(IsDeathKey, true);

    public void Hit() => _animator.SetTrigger(HitKey);

    public void JumpByNavmeshLink()
    {
        _animator.SetBool(IsJumping, true);
        _jetParticles.Play();
    }

    public void StopJumpByNavMesh()
    {
        _animator.SetBool(IsJumping, false);
        _jetParticles.Stop();
    }

    public void SwitchLayerToInjured() => _animator.SetLayerWeight(InjuredLayer, 1);

    public void SwitchLayerToHealth() => _animator.SetLayerWeight(InjuredLayer, 0);
}
