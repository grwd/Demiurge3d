using UnityEngine;

public static class CoordinateConverter
{
    /// <summary>
    /// Converts a world position into a local position relative to a given canvas RectTransform.
    /// </summary>
    /// <param name="worldPos">The world position to convert.</param>
    /// <param name="worldCamera">The camera rendering the world (e.g., main camera).</param>
    /// <param name="canvasRect">The RectTransform of the UI canvas or its parent.</param>
    /// <param name="uiCamera">
    /// The camera used by the canvas (if Screen Space - Camera). Pass null if the canvas is in Screen Space - Overlay.
    /// </param>
    /// <returns>Local position in canvas space.</returns>
    public static Vector2 WorldToCanvasPoint(Vector3 worldPos, Camera worldCamera, RectTransform canvasRect, Camera uiCamera)
    {
        Vector3 screenPos = worldCamera.WorldToScreenPoint(worldPos);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out localPoint);
        return localPoint;
    }
}
