using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;
    public bool invertFlip = false;
    public bool IsMoving { get; private set; }
    public float CurrentSpeed { get; private set; }

    [Header("References")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private AnimationSystem _animationSystem;

    [Header("Surge Settings")]
    [SerializeField] private float _baseMoveSpeed = 5f;
    public float BaseMoveSpeed => _baseMoveSpeed;
    public void SetMoveSpeed(float newSpeed) => moveSpeed = newSpeed;
    public void ResetMoveSpeed() => moveSpeed = _baseMoveSpeed;

    public MovementSystem movement;


    //public bool IsFacingRight { get; private set; } = true;

    public bool _isFacingRight { get; private set; } = true;
    public event System.Action<bool> OnFlipped; // bool = new facing state (true=right)



    public void Move(float direction)
    {
        CurrentSpeed = Mathf.Abs(direction);
        IsMoving = !Mathf.Approximately(direction, 0f);
        _rb.linearVelocity = new Vector3(direction * moveSpeed, _rb.linearVelocity.y, 0);

        if (direction != 0)
        {
            bool wantsRight = invertFlip ? direction < 0 : direction > 0;

            if (wantsRight != _isFacingRight)
            {
                FlipCharacter(wantsRight);
            }
        }
    }

    private void FlipCharacter(bool newFacingRight)
    {
        // 1. FLIP SCALE FIRST
        float flipSign = newFacingRight ? 1 : -1;
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            flipSign * Mathf.Abs(transform.localScale.z)

        );
         OnFlipped?.Invoke(newFacingRight);
        // 2. TRIGGER ANIMATION AFTER VISUAL FLIP
        _animationSystem.TriggerFlip();

        // 3. Update state
        _isFacingRight = newFacingRight;

        // 4. External signal
        OnFlipped?.Invoke(newFacingRight);
    }
}