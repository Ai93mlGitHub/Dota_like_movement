using System.Collections;
using UnityEngine;

public class PlayerView
{
    private Animator _animator;
    private ParticleSystem _jetParticles;
    
    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int IsDeathKey = Animator.StringToHash("IsDeath");
    private readonly int IsJumping = Animator.StringToHash("IsJumping");
    private readonly int HitKey = Animator.StringToHash("Hit");
    private readonly int InjuredLayer = 1;


    public PlayerView(Animator animator, ParticleSystem particles)
    {
        _animator = animator;
        _jetParticles = particles;
    }

    public void StartRunning() => _animator.SetBool(IsRunningKey, true);

    public void StopRunning() => _animator.SetBool(IsRunningKey, false);

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
