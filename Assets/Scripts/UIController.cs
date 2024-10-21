using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthField;
    public string HealthName { get; private set; } = "HP";

    private void Start() => Health.HealthChanged += UpdateHealthField;

    public void UpdateHealthField(float value) => _healthField.text = $"{HealthName}: {value.ToString("F0")}";
}
