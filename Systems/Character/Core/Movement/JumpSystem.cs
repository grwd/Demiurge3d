using UnityEngine;

public class JumpSystem : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForce = 12f;
    public float anticipationTime = 0.3f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

    [Header("References")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GroundSensor _groundSensor;
    [SerializeField] private AnimationSystem _animSystem;

    private float _lastGroundedTime = Mathf.NegativeInfinity;
    private float _lastJumpPressedTime = Mathf.NegativeInfinity; // Initialize to "no jump input"
    private bool _isJumping;


    void Update()
    {

        if (Time.time > 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _lastJumpPressedTime = Time.time;
            }
        }

        // Track time since last grounded/jump press
        if (_groundSensor.IsGrounded) _lastGroundedTime = Time.time;
        if (Input.GetButtonDown("Jump")) _lastJumpPressedTime = Time.time;


        if (_groundSensor.IsGrounded)
            _lastGroundedTime = Time.time;

    }

    public void TryJump()
    {
        bool canUseCoyote = Time.time - _lastGroundedTime <= coyoteTime;
        bool hasBufferedInput = Time.time - _lastJumpPressedTime <= jumpBufferTime;

        if ((_groundSensor.IsGrounded || canUseCoyote) && hasBufferedInput && !_isJumping)
        {
            StartCoroutine(JumpWithAnticipation());
        }
    }

    private System.Collections.IEnumerator JumpWithAnticipation()
    {
        _isJumping = true;

        // 1. ANTICIPATION PHASE
        _animSystem.TriggerAnticipation(); // Visual prep
        yield return new WaitForSeconds(anticipationTime);

        // 2. LIFTOFF PHASE
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, 0); // Reset Y velocity
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _animSystem.TriggerJump();

        _isJumping = false;
    }
}