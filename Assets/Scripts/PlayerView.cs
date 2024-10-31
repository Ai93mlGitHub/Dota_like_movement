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

    private void OnEnable()
    {
        PlayerController player = GetComponent<PlayerController>();
        player.OnDeath += Death;
        player.OnJumpByNavmeshLink += JumpByNavmeshLink;
        player.OnStopJumpByNavMesh += StopJumpByNavMesh;
        player.OnStartRunning += StartRunning;
        player.OnStopRunning += StopRunning;
        player.OnHit += Hit;
        player.OnSwitchLayerToInjured += SwitchLayerToInjured;
    }

    private void OnDisable()
    {
        PlayerController player = GetComponent<PlayerController>();
        player.OnDeath -= Death;
        player.OnJumpByNavmeshLink -= JumpByNavmeshLink;
        player.OnStopJumpByNavMesh -= StopJumpByNavMesh;
        player.OnStartRunning -= StartRunning;
        player.OnStopRunning -= StopRunning;
        player.OnHit -= Hit;
        player.OnSwitchLayerToInjured -= SwitchLayerToInjured;
    }

    public void StartRunning()
    {
        _animator.SetBool(IsRunningKey, true);
        if (_footstepAudio != null && !_footstepAudio.isPlaying)
        {
            _footstepAudio.Play(); // Проигрываем звук шагов
        }
    }

    public void StopRunning()
    {
        _animator.SetBool(IsRunningKey, false);
        if (_footstepAudio != null && _footstepAudio.isPlaying)
        {
            _footstepAudio.Stop(); // Останавливаем звук шагов
        }
    }

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
