using UnityEngine;

public class SurgeSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _surgeSpeedMultiplier = 1.5f;
    [SerializeField] private float _staminaDrainPerSecond = 25f;
    [SerializeField] private KeyCode _surgeKey = KeyCode.LeftShift;

    // References
    public MovementSystem _movement;
    private StaminaSystem _stamina;
    private AnimationSystem _anim;

    // State
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
        if (Input.GetKeyDown(_surgeKey)) StartSurge(); // Fixed parenthesis
        if (Input.GetKeyUp(_surgeKey)) EndSurge();

        if (IsSurging)
        {
            _stamina.ModifyStamina(-_staminaDrainPerSecond * Time.deltaTime);
            if (_stamina.CurrentStamina <= 0) EndSurge();
        }
    }

    private void StartSurge()
    {
        if (_stamina.CurrentStamina <= 0) return;

        IsSurging = true;
        LockedFacingDirection = _movement._isFacingRight ? Vector2.right : Vector2.left; // Use public property
        _movement.SetMoveSpeed(_movement.BaseMoveSpeed * _surgeSpeedMultiplier);
        _anim.SetSurging(true);
    }

    private void EndSurge()
    {
        IsSurging = false;
        _movement.ResetMoveSpeed();
        _anim.SetSurging(false);
        
    }
}