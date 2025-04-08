using UnityEngine;
using UnityEngine.UI;
using static MovementSystem;
using System.Collections.Generic;

public class EnhancedCursorController : MonoBehaviour
{
    [Header("References")]
    public Transform characterTransform;
    public RectTransform cursorVisual;
    public Camera worldCamera;
    public Camera uiCamera;
    public RectTransform canvasRect;

    [Header("Core Settings")]
    public float mouseSensitivity = 1.0f;
    public float maxDistance = 120f;

    [Header("Outer Boundary Settings")]
    public float boundaryStartDistance = 100f;
    public float maxResistanceForce = 5f;
    public float slidingFactor = 0.6f;

    [Header("Inner Zone Settings")]
    public float innerZoneRadius = 25f;
    public float repulsionStrength = 8f;
    public float accelerationFactor = 2.5f;
    public float optimumCombatDistance = 60f;

    [Header("Character Flip Settings")]
    public float flipTransitionSpeed = 10f;
    public float inputBreakThreshold = 0.3f;

    // Runtime properties
    private Vector2 cursorPosition;
    private Vector2 characterScreenPos;
    private Vector2 cursorVelocity = Vector2.zero;
    private Vector2 smoothedForce = Vector2.zero;
    private Vector2 flipTargetPosition;
    private bool inFlipTransition = false;
    private float cursorMomentum = 0f;
    private Vector2 lastRawInput = Vector2.zero;

    void Start()
    {
        if (worldCamera == null)
            worldCamera = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        UpdateCharacterScreenPosition();
        cursorPosition = characterScreenPos + new Vector2(optimumCombatDistance, 0);

        // Subscribe to character flip events
        MovementSystem.OnCharacterFlipped += OnCharacterFlipped;
    }

    void OnDestroy()
    {
        MovementSystem.OnCharacterFlipped -= OnCharacterFlipped;
    }

    void Update()
    {
        // Update character position on screen
        UpdateCharacterScreenPosition();

        // Get raw mouse input
        Vector2 rawInput = new Vector2(
            Input.GetAxis("Mouse X") * mouseSensitivity,
            Input.GetAxis("Mouse Y") * mouseSensitivity
        );

        // Track momentum for predictions
        float inputMagnitude = rawInput.magnitude;
        cursorMomentum = Mathf.Lerp(cursorMomentum, inputMagnitude, Time.deltaTime * 5f);

        // Store for directional intent
        if (inputMagnitude > 0.1f)
        {
            lastRawInput = rawInput.normalized;
        }

        // Process forces
        Vector2 totalForce = CalculateTotalForce(rawInput);

        // Smooth forces for fluid feeling
        smoothedForce = Vector2.Lerp(smoothedForce, totalForce, Time.deltaTime * 8f);

        // Apply to position
        cursorPosition += smoothedForce;

        // Update visual position
        if (cursorVisual != null)
            cursorVisual.anchoredPosition = cursorPosition;
    }

    private Vector2 CalculateTotalForce(Vector2 rawInput)
    {
        Vector2 totalForce = rawInput;
        Vector2 toCharacter = characterScreenPos - cursorPosition;
        float distance = toCharacter.magnitude;

        // Handle flip transition
        if (inFlipTransition)
        {
            // Break transition if strong input detected
            if (rawInput.magnitude > inputBreakThreshold)
            {
                inFlipTransition = false;
            }
            else
            {
                // Calculate transition force
                Vector2 toTarget = flipTargetPosition - cursorPosition;
                float transitionForce = Mathf.Min(flipTransitionSpeed, toTarget.magnitude);

                if (toTarget.magnitude < 1f)
                {
                    inFlipTransition = false;
                }
                else
                {
                    // Apply transition force
                    totalForce += toTarget.normalized * transitionForce;
                }
            }
        }

        // Inner zone handling - repulsion and acceleration
        if (distance < innerZoneRadius)
        {
            float repulsionRatio = 1f - (distance / innerZoneRadius);

            // Calculate acceleration based on approach velocity
            Vector2 directionFromCharacter = (cursorPosition - characterScreenPos).normalized;

            // Blend between current direction and optimal combat direction
            Vector2 characterFacing = GetCharacterFacingDirection();
            Vector2 optimalDirection = Vector2.Lerp(directionFromCharacter, characterFacing, 0.4f);

            // Apply repulsion and acceleration
            float force = repulsionStrength * repulsionRatio;

            // Add directional intent with momentum-based acceleration
            if (lastRawInput.magnitude > 0)
            {
                // Add influence from player's intended direction
                optimalDirection = Vector2.Lerp(optimalDirection, lastRawInput, 0.5f);
                force *= (1f + cursorMomentum * accelerationFactor);
            }

            totalForce += optimalDirection.normalized * force;
        }

        // Outer boundary handling - elastic resistance with sliding
        if (distance > boundaryStartDistance)
        {
            // Calculate how far past the boundary
            float overshoot = distance - boundaryStartDistance;
            float boundaryRatio = Mathf.Clamp01(overshoot / (maxDistance - boundaryStartDistance));

            // Calculate resistance - grows non-linearly
            float resistanceFactor = Mathf.Pow(boundaryRatio, 2) * maxResistanceForce;
            Vector2 resistanceForce = toCharacter.normalized * resistanceFactor;

            // Calculate sliding force along boundary
            Vector2 tangent = new Vector2(-toCharacter.normalized.y, toCharacter.normalized.x);
            float tangentDot = Vector2.Dot(rawInput.normalized, tangent);
            Vector2 slidingForce = tangent * tangentDot * slidingFactor * resistanceFactor;

            // Apply both forces
            totalForce += resistanceForce + slidingForce;
        }

        return totalForce;
    }

    private void UpdateCharacterScreenPosition()
    {
        characterScreenPos = CoordinateConverter.WorldToCanvasPoint(
            characterTransform.position,
            worldCamera,
            canvasRect,
            uiCamera
        );
    }

    private Vector2 GetCharacterFacingDirection()
    {
        // This should be updated to use your actual character facing logic
        MovementSystem movementSystem = characterTransform.GetComponent<MovementSystem>();
        bool isFacingRight = movementSystem != null ? movementSystem.IsFacingRight : true;

        return isFacingRight ? Vector2.right : Vector2.left;
    }

    void OnCharacterFlipped(FlipEventData data)
    {
        // Calculate optimal position on opposite side
        Vector2 relativePos = cursorPosition - characterScreenPos;

        // Mirror position but maintain vertical position and distance
        Vector2 mirrored = new Vector2(-relativePos.x, relativePos.y);

        // Adjust to optimal combat distance if needed
        float currentDistance = mirrored.magnitude;
        if (currentDistance < innerZoneRadius * 1.5f)
        {
            mirrored = mirrored.normalized * optimumCombatDistance;
        }

        flipTargetPosition = characterScreenPos + mirrored;
        inFlipTransition = true;
    }
}