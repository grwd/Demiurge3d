using UnityEngine;

public class AnimationSystem : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [Header("Walking Animation")]
    [SerializeField] private float _walkAnimationSmoothing = 0.1f;

    [Header("Walk Stickyness")]
    [SerializeField] private float _walkStartDelay = 0.15f;
    [SerializeField] private float _walkEndDelay = 0.25f;

    private bool _previousWalkState;
    private bool _targetWalkState;
    private float _lastWalkStateChangeTime;
    public bool IsWalking => _targetWalkState;




    // Existing methods (jump, flip, etc.) remain unchanged
    public void TriggerAnticipation() => _animator.SetTrigger("Anticipate");
    public void TriggerJump() => _animator.SetTrigger("Jump");
    public void TriggerLand() => _animator.SetTrigger("Land");

    public void UpdateWalkingState(bool isWalking, float moveSpeed)
    {
        // Update speed parameter immediately
        _animator.SetFloat("Speed", moveSpeed, _walkAnimationSmoothing, Time.deltaTime);

        // Handle delayed boolean transition
        HandleWalkStateDelay(isWalking);
    }

    private void HandleWalkStateDelay(bool currentWalkState)
    {
        if (currentWalkState != _previousWalkState)
        {
            _lastWalkStateChangeTime = Time.time;
            _previousWalkState = currentWalkState;
        }

        float requiredDelay = currentWalkState ? _walkStartDelay : _walkEndDelay;

        if (Time.time - _lastWalkStateChangeTime >= requiredDelay)
        {
            _targetWalkState = currentWalkState;
        }

        _animator.SetBool("IsWalking", _targetWalkState);
    }

    // Rest of your existing methods...
    public void UpdateGroundedState(bool isGrounded) =>
        _animator.SetBool("IsGrounded", isGrounded);

    public void TriggerFlip()
    {
        _animator.ResetTrigger("Flip");
        _animator.SetTrigger("Flip");
    }

    public void TriggerWalkStart()
    {
        //_animator.ResetTrigger("WalkStart");
        _animator.SetTrigger("WalkStart");
    }

    public void TriggerWalkRestart()
    {
        _animator.ResetTrigger("WalkStart");

    }


    public void SetSurging(bool isSurging)
    {
        _animator.SetBool("IsSurging", isSurging);
    }


}
