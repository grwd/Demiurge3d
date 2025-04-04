using UnityEngine;

// Add this script as a new component to your character
public class AnimationOverlayHandler : MonoBehaviour
{
    [Header("Walk Start Settings")]
    [SerializeField] private float _idleTimeRequired = 0.3f;

    [Header("Dependencies")]
    [SerializeField] private MovementSystem _movement;
    [SerializeField] private AnimationSystem _animation;

    private float _idleTimer;

    void Update()
    {
        // Track idle time
        if (!_movement.IsMoving)
        {
            _idleTimer += Time.deltaTime;
            _animation.TriggerWalkRestart();
        }
        else
        {
            // Trigger walk-start only after sufficient idle time
            if (_idleTimer >= _idleTimeRequired)
            {
                _animation.TriggerWalkStart();
            }
            
            _idleTimer = 0f; // Reset
           
        }
    }
}