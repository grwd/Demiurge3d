using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform checkOrigin;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded { get; private set; }

    void Update()
    {
        // Draw visible circle in Scene view
        Debug.DrawRay(checkOrigin.position, Vector3.down * checkRadius, Color.cyan);

        // Check if touching ground
        IsGrounded = Physics.CheckSphere(
            checkOrigin.position,
            checkRadius,
            groundLayer
        );
    }
}