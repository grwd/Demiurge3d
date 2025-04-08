using System.Collections;
using UnityEngine;

public class RandomFacialAnimations : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Animator _faceAnimator;
    [SerializeField] private string _blinkTriggerName = "Blink";

    [Header("Timing Settings")]
    [SerializeField] private float _minWaitTime = 2f;
    [SerializeField] private float _maxWaitTime = 5f;
    [SerializeField] private float _blinkCooldown = 0.1f;

    private void Start()
    {
        if (_faceAnimator == null)
            _faceAnimator = GetComponent<Animator>();

        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            // Wait random time between blinks
            yield return new WaitForSeconds(Random.Range(_minWaitTime, _maxWaitTime));

            // Trigger blink animation
            _faceAnimator.SetTrigger(_blinkTriggerName);

            // Brief cooldown to prevent double-triggers
            yield return new WaitForSeconds(_blinkCooldown);
        }
    }

    // For manual blink triggering (optional)
    public void ForceBlink()
    {
        _faceAnimator.SetTrigger(_blinkTriggerName);
    }
}