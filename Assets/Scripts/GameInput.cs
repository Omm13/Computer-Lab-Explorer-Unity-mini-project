using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

// Works with Input Manager (Old), Input System (New) or Both.
public static class GameInput
{
#if ENABLE_LEGACY_INPUT_MANAGER
    public static Vector2 Move() { return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); }
    public static Vector2 Look() { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
    public static bool JumpPressed() { return Input.GetKeyDown(KeyCode.Space); }
    public static bool InteractPressed() { return Input.GetKeyDown(KeyCode.E); }
    public static bool EscapePressed() { return Input.GetKeyDown(KeyCode.Escape); }
    public static bool ClickPressed() { return Input.GetMouseButtonDown(0); }
#elif ENABLE_INPUT_SYSTEM
    public static Vector2 Move()
    {
        Keyboard k = Keyboard.current; if (k == null) return Vector2.zero;
        float x = (k.dKey.isPressed ? 1f : 0f) - (k.aKey.isPressed ? 1f : 0f);
        float y = (k.wKey.isPressed ? 1f : 0f) - (k.sKey.isPressed ? 1f : 0f);
        return new Vector2(x, y);
    }
    public static Vector2 Look() { return Mouse.current == null ? Vector2.zero : Mouse.current.delta.ReadValue() * 0.05f; }
    public static bool JumpPressed() { return Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame; }
    public static bool InteractPressed() { return Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame; }
    public static bool EscapePressed() { return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame; }
    public static bool ClickPressed() { return Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame; }
#else
    public static Vector2 Move() { return Vector2.zero; }
    public static Vector2 Look() { return Vector2.zero; }
    public static bool JumpPressed() { return false; }
    public static bool InteractPressed() { return false; }
    public static bool EscapePressed() { return false; }
    public static bool ClickPressed() { return false; }
#endif
}
