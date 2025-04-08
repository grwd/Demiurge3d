using UnityEngine;

public class FlipAdjuster : MonoBehaviour
{
    [Header("Offsets")]
    [SerializeField] private Vector3 _positionOffsetRight = new Vector3(0.5f, 0, 0);
    [SerializeField] private Vector3 _positionOffsetLeft = new Vector3(-0.5f, 0, 0);
    [SerializeField] private Vector3 _rotationOffset = new Vector3(0, 180f, 0);

    [Header("References")]
    [SerializeField] private MovementSystem _movementSystem;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    void Start()
    {
        CacheOriginalTransform();
        _movementSystem.OnFlipped += HandleFlip;
    }

    void OnDestroy()
    {
        _movementSystem.OnFlipped -= HandleFlip;
    }

    private void CacheOriginalTransform()
    {
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
    }

    private void HandleFlip(bool isFacingRight)
    {
        // Apply position offset
        transform.localPosition = _originalPosition +
            (isFacingRight ? _positionOffsetRight : _positionOffsetLeft);

        // Apply rotation offset
        transform.localRotation = _originalRotation *
            Quaternion.Euler(isFacingRight ? Vector3.zero : _rotationOffset);
    }

    void OnValidate()
    {
        if (!Application.isPlaying && _movementSystem == null)
            _movementSystem = GetComponentInParent<MovementSystem>();
    }
}