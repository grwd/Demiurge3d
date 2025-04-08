using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [Header("Core Settings")]
    [SerializeField] private float _maxStamina = 100f;
    [SerializeField] private float _baseRegenRate = 15f;

    [Header("Surge Modifiers")]
    [SerializeField][Range(0, 1)] private float _surgeRegenPenalty = 0.3f;
    [SerializeField] private float _surgeActivationCost = 30f;

    private float _currentRegenMultiplier = 1f;
    public float CurrentStamina { get; private set; }
    public float MaxStamina => _maxStamina;
    public float RegenRate => _baseRegenRate * _currentRegenMultiplier;

    void Start() => CurrentStamina = _maxStamina;

    public void ModifyStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + amount, 0, _maxStamina);
    }

    public void ModifyRegenMultiplier(float multiplier)
    {
        _currentRegenMultiplier = Mathf.Clamp(multiplier, 0.1f, 2f);
    }

    void Update()
    {
        if (CurrentStamina < _maxStamina)
        {
            ModifyStamina(RegenRate * Time.deltaTime);
        }
    }

    // Surge-specific interface
    public bool TryPaySurgeCost()
    {
        if (CurrentStamina >= _surgeActivationCost)
        {
            ModifyStamina(-_surgeActivationCost);
            return true;
        }
        return false;
    }

    public void ApplySurgeRegenPenalty() => ModifyRegenMultiplier(_surgeRegenPenalty);
    public void ResetRegen() => ModifyRegenMultiplier(1f);
}