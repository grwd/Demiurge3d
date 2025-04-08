using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _baseMoveSpeed = 5f;
    [SerializeField] private bool _invertFlip = false;

    [Header("References")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private AnimationSystem _animationSystem;

    // Public properties
    public bool IsMoving { get; private set; }
    public float CurrentSpeed => _rb.linearVelocity.magnitude;
    public bool IsFacingRight { get; private set; } = true;
    public float BaseMoveSpeed => _baseMoveSpeed;

    // Internal state
    private float _initialMoveSpeed;
    public event System.Action<bool> OnFlipped;
    public struct FlipEventData
    {
        public bool NewFacingRight;
        public Vector3 FlipPosition;
        public float FlipTime;
    }

    public static event System.Action<FlipEventData> OnCharacterFlipped;

    void Start()
    {
        _initialMoveSpeed = _baseMoveSpeed; // Store original speed
    }

    public void Move(float direction)
    {
        // Apply movement using preserved base speed
        _rb.linearVelocity = new Vector3(direction * _baseMoveSpeed, _rb.linearVelocity.y, 0);
        IsMoving = !Mathf.Approximately(direction, 0f);

        HandleCharacterFlip(direction);
    }

    private void HandleCharacterFlip(float direction)
    {
        if (direction == 0) return;

        bool wantsRight = _invertFlip ? direction < 0 : direction > 0;
        if (wantsRight != IsFacingRight)
        {
            FlipCharacter(wantsRight);
        }
    }

    private void FlipCharacter(bool newFacingRight)
    {
        // Visual flip
        float flipSign = newFacingRight ? 1 : -1;
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            flipSign * Mathf.Abs(transform.localScale.z)
        );

        // Update state and trigger events
        IsFacingRight = newFacingRight;
        _animationSystem.TriggerFlip();
        OnFlipped?.Invoke(newFacingRight);

        // Broadcast event with context
        OnCharacterFlipped?.Invoke(new FlipEventData
        {
            NewFacingRight = newFacingRight,
            FlipPosition = transform.position,
            FlipTime = Time.time
        });


    }

    public void SetTemporarySpeed(float multiplier)
    {
        _baseMoveSpeed = _initialMoveSpeed * multiplier;
    }

    public void ResetToBaseSpeed()
    {
        _baseMoveSpeed = _initialMoveSpeed;
    }
}