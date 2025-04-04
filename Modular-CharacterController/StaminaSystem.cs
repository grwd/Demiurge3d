using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _maxStamina = 100f;
    [SerializeField] private float _staminaRegenPerSecond = 15f;

    public float CurrentStamina { get; private set; }
    public float MaxStamina => _maxStamina;

    void Start() => CurrentStamina = _maxStamina;

    public void ModifyStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + amount, 0, _maxStamina);
    }

    void Update()
    {
        if (CurrentStamina < _maxStamina)
        {
            ModifyStamina(_staminaRegenPerSecond * Time.deltaTime);
        }
    }
}