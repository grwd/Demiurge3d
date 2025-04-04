using UnityEngine;

public class CharacterCore : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Rigidbody _rb;

    [Header("Systems")]
    [SerializeField] private MovementSystem movement;
    [SerializeField] private GroundSensor groundSensor;
    [SerializeField] private JumpSystem _jumpSystem;
    [SerializeField] private AnimationSystem _animationSystem; 


    void Awake()
    {
        // Auto-get references if not set in Inspector
        if (!_inputHandler) _inputHandler = GetComponent<InputHandler>();
        if (!_rb) _rb = GetComponent<Rigidbody>();

        if (!groundSensor) groundSensor = GetComponent<GroundSensor>();
        if (!_animationSystem) _animationSystem = GetComponent<AnimationSystem>();
    }

    void Update()
    {
        // Temporary passthrough until we add movement system
        //Debug.Log($"Core received input: {_inputHandler.MoveInput}");


        // Make character move
        movement.Move(_inputHandler.MoveInput);
        _jumpSystem.TryJump(); // Add this line
        
        _animationSystem.UpdateGroundedState(groundSensor.IsGrounded);

        // Send movement data to animation system
        _animationSystem.UpdateWalkingState(
            movement.IsMoving,
            movement.CurrentSpeed
        );


    }

    // Add more subsystem integrations later







}