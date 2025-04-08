using UnityEngine;

public class SurgeSystem : MonoBehaviour
{
    [Header("Surge Settings")]
    [SerializeField] private float _surgeSpeedMultiplier = 1.5f;
    [SerializeField] private KeyCode _surgeKey = KeyCode.LeftShift;

    [Header("References")]
    [SerializeField] private MovementSystem _movement;
    private StaminaSystem _stamina;
    private AnimationSystem _anim;

    public bool IsSurging { get; private set; }
    public Vector2 LockedFacingDirection { get; private set; }

    void Awake()
    {
        _movement = GetComponent<MovementSystem>();
        _stamina = GetComponent<StaminaSystem>();
        _anim = GetComponent<AnimationSystem>();
    }

    void Update()
    {
        HandleSurgeInput();
        AutoCancelIfEmpty();
    }

    private void HandleSurgeInput()
    {
        if (Input.GetKeyDown(_surgeKey)) StartSurge();
        if (Input.GetKeyUp(_surgeKey)) EndSurge();
    }

    private void StartSurge()
    {
        if (!_stamina.TryPaySurgeCost()) return;

        IsSurging = true;
        _stamina.ApplySurgeRegenPenalty();
        LockedFacingDirection = _movement.IsFacingRight ? Vector2.right : Vector2.left;
        _movement.SetTemporarySpeed(_movement.BaseMoveSpeed * _surgeSpeedMultiplier);
        _anim.SetSurging(true);
    }

    private void EndSurge()
    {
        IsSurging = false;
        _stamina.ResetRegen();
        _movement.ResetToBaseSpeed();
        _anim.SetSurging(false);
    }

    private void AutoCancelIfEmpty()
    {
        if (IsSurging && _stamina.CurrentStamina <= 0)
        {
            EndSurge();
        }
    }
}