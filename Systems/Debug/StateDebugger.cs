using UnityEngine;

public class StateDebugger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputHandler _input;
    [SerializeField] private MovementSystem _movement;
    [SerializeField] private GroundSensor _groundSensor;
    [SerializeField] private AnimationSystem _animSystem;
    [SerializeField] private SurgeSystem _surgeSystem;
    [SerializeField] private StaminaSystem _staminaSystem;

    [Header("Display Settings")]
    [SerializeField] private Color _headerColor = Color.yellow;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Vector2 _screenPosition = new Vector2(20, 20);
    [SerializeField] private float _lineHeight = 25f;
    [SerializeField] private float _sectionSpacing = 15f;

    void OnGUI()
    {
        GUI.skin.label.fontSize = 14;
        var headerStyle = new GUIStyle(GUI.skin.label) { normal = { textColor = _headerColor } };
        var valueStyle = new GUIStyle(GUI.skin.label) { normal = { textColor = _normalColor } };

        float yPos = _screenPosition.y;

        DrawSection("INPUT STATES", ref yPos, headerStyle, valueStyle,
            $"Move: {_input.MoveInput:0.00}",
            $"Jump Pressed: {_input.JumpPressed}",
            $"Jump Held: {_input.JumpHeld}"
        );

        DrawSection("MOVEMENT STATES", ref yPos, headerStyle, valueStyle,
            $"Moving: {_movement.IsMoving}",
            $"Speed: {_movement.CurrentSpeed:0.00} m/s",
            $"Facing Right: {_movement.IsFacingRight}"
        );

        DrawSection("CHARACTER STATES", ref yPos, headerStyle, valueStyle,
            $"Grounded: {_groundSensor.IsGrounded}",
            $"Walking: {_animSystem.IsWalking}"
        );

        DrawSection("SURGE STATES", ref yPos, headerStyle, valueStyle,
            $"Active: {_surgeSystem.IsSurging}",
            $"Stamina: {_staminaSystem.CurrentStamina:0}/{_staminaSystem.MaxStamina}",
            $"Locked Facing: {_surgeSystem.LockedFacingDirection}",
            $"Base Speed: {_movement.BaseMoveSpeed:0.00}"
        );
    }

    void DrawSection(string header, ref float yPos, GUIStyle headerStyle, GUIStyle valueStyle, params string[] lines)
    {
        GUI.Label(new Rect(_screenPosition.x, yPos, 300, _lineHeight), header, headerStyle);
        yPos += _lineHeight;

        foreach (var line in lines)
        {
            GUI.Label(new Rect(_screenPosition.x, yPos, 300, _lineHeight), line, valueStyle);
            yPos += _lineHeight;
        }
        yPos += _sectionSpacing;
    }

    void OnValidate()
    {
        if (!_input) _input = GetComponent<InputHandler>();
        if (!_movement) _movement = GetComponent<MovementSystem>();
        if (!_groundSensor) _groundSensor = GetComponent<GroundSensor>();
        if (!_animSystem) _animSystem = GetComponent<AnimationSystem>();
        if (!_surgeSystem) _surgeSystem = GetComponent<SurgeSystem>();
        if (!_staminaSystem) _staminaSystem = GetComponent<StaminaSystem>();
    }
}