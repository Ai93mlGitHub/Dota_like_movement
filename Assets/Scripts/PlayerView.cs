using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int IsDeathKey = Animator.StringToHash("IsDeath");
    private readonly int HitKey = Animator.StringToHash("Hit");
    private readonly int InjuredLayer = 1;

    private void Start()
    {
        if (_animator == null)
            Debug.Log("Animator не найден!");
    }

    public void StartRunning() => _animator.SetBool(IsRunningKey, true);

    public void StopRunning() => _animator.SetBool(IsRunningKey, false);

    public void Death() => _animator.SetBool(IsDeathKey, true);

    public void Hit() => _animator.SetTrigger(HitKey);

    public void SwitchLayerToInjured() => _animator.SetLayerWeight(InjuredLayer, 1);

    public void SwitchLayerToHealth() => _animator.SetLayerWeight(InjuredLayer, 0);
}
