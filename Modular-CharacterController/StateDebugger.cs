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

    void OnGUI()
    {
        GUI.skin.label.fontSize = 16;
        GUIStyle headerStyle = new GUIStyle(GUI.skin.label) { normal = { textColor = _headerColor } };
        GUIStyle valueStyle = new GUIStyle(GUI.skin.label) { normal = { textColor = _normalColor } };

        float yPos = _screenPosition.y;
        float labelWidth = 250f;
        float lineHeight = 25f;
        float sectionSpacing = 15f;

        // Input States
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), "INPUT STATES", headerStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), $"Move Input: {_input.MoveInput:0.00}", valueStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), $"Jump Pressed: {_input.JumpPressed}", valueStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), $"Jump Held: {_input.JumpHeld}", valueStyle);
        yPos += lineHeight + sectionSpacing;

        // Movement States
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), "MOVEMENT STATES", headerStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), $"Is Moving: {_movement.IsMoving}", valueStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), $"Current Speed: {_movement.CurrentSpeed:0.00}", valueStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), $"Facing Right: {_movement._isFacingRight}", valueStyle);
        yPos += lineHeight + sectionSpacing;

        // Character States
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), "CHARACTER STATES", headerStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), $"Is Grounded: {_groundSensor.IsGrounded}", valueStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), $"Is Walking: {_animSystem.IsWalking}", valueStyle);
        yPos += lineHeight + sectionSpacing;

        // Surge States
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), "SURGE STATES", headerStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), $"Is Surging: {_surgeSystem.IsSurging}", valueStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(_screenPosition.x, yPos, labelWidth, lineHeight), $"Stamina: {_staminaSystem.CurrentStamina:0}/{_staminaSystem.MaxStamina}", valueStyle);

        if (_surgeSystem.IsSurging)
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Surge activated. Locked facing direction: " + _surgeSystem.LockedFacingDirection);
        }

        if (!_surgeSystem.IsSurging)
        {
            GUI.Label(new Rect(10, 30, 200, 20), "Surge deactivated. Move speed reset to: " + _surgeSystem._movement.BaseMoveSpeed);
        }


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