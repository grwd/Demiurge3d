using System.Collections;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Normal time scale is 1")]
    [SerializeField] private float normalTimeScale = 1f;
    [SerializeField][Range(0, 1)] private float slowDownFactor = 0.2f;
    [SerializeField] private float transitionDuration = 1f;

    [Header("Input Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.T;
    [SerializeField] private bool slowMotionEnabled = false;

    private Coroutine timeTransitionCoroutine;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            slowMotionEnabled = !slowMotionEnabled;

            if (timeTransitionCoroutine != null)
            {
                StopCoroutine(timeTransitionCoroutine);
            }

            float targetTimeScale = slowMotionEnabled ? slowDownFactor : normalTimeScale;
            timeTransitionCoroutine = StartCoroutine(SmoothTimeTransition(targetTimeScale));
        }
    }

    private IEnumerator SmoothTimeTransition(float targetTimeScale)
    {
        float startTimeScale = Time.timeScale;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / transitionDuration;
            Time.timeScale = Mathf.Lerp(startTimeScale, targetTimeScale, t);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        Time.timeScale = targetTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void OnDestroy()
    {
        // Reset time scale when destroyed
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f;
    }
}