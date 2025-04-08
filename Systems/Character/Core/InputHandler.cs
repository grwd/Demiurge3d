using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Public API for other systems to consume
    public float MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }

    void Update()
    {
        // Raw input processing
        MoveInput = Input.GetAxisRaw("Horizontal");
        JumpPressed = Input.GetButtonDown("Jump");
        JumpHeld = Input.GetButton("Jump");
    }
}