using UnityEngine;

public class GameSystemGeneral : MonoBehaviour
{
    void Start()
    {
        LockCursor();
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ToggleCursor()
    {
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ?
            CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // In Editor: Toggle cursor visibility and lock state
            ToggleCursor();
#else
            // In Build: Quit application
            Application.Quit();
            Debug.Log("Game closed");
#endif
        }
    }
}