using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSPlayer : MonoBehaviour
{
    public Transform cameraPivot;
    public Camera cam;
    public float moveSpeed = 4.5f, jumpHeight = 1.1f, gravity = -20f, mouseSensitivity = 2f, lookLimit = 80f, reach = 3f;

    CharacterController controller;
    float vy, pitch;
    Interactable focus;
    GUIStyle cross, promptStyle;

    void Start() { controller = GetComponent<CharacterController>(); SetCursor(true); }

    void Update()
    {
        if (GameInput.EscapePressed()) SetCursor(false);
        if (GameInput.ClickPressed() && Cursor.lockState != CursorLockMode.Locked) SetCursor(true);
        bool locked = Cursor.lockState == CursorLockMode.Locked;

        if (locked)
        {
            Vector2 look = GameInput.Look();
            transform.Rotate(0, look.x * mouseSensitivity, 0);
            pitch = Mathf.Clamp(pitch - look.y * mouseSensitivity, -lookLimit, lookLimit);
            if (cameraPivot != null) cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);
        }

        Vector2 mv = GameInput.Move();
        Vector3 move = (transform.right * mv.x + transform.forward * mv.y).normalized;
        controller.Move(move * moveSpeed * Time.deltaTime);
        if (controller.isGrounded && vy < 0) vy = -2f;
        if (controller.isGrounded && GameInput.JumpPressed()) vy = Mathf.Sqrt(jumpHeight * -2f * gravity);
        vy += gravity * Time.deltaTime;
        controller.Move(Vector3.up * vy * Time.deltaTime);

        focus = null;
        if (locked && cam != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, reach))
                focus = hit.collider.GetComponentInParent<Interactable>();
        }
        if (focus != null && GameInput.InteractPressed()) focus.Interact();
    }

    void SetCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    void OnGUI()
    {
        GUI.depth = 1;
        if (cross == null)
        {
            cross = UiUtil.Label(24, FontStyle.Normal, Color.white, TextAnchor.MiddleCenter);
            promptStyle = UiUtil.Label(17, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter);
        }
        GUI.Label(new Rect(Screen.width / 2f - 12, Screen.height / 2f - 16, 24, 32), "+", cross);
        if (focus != null && !string.IsNullOrEmpty(focus.Prompt))
        {
            UiUtil.Fill(new Rect(Screen.width / 2f - 170, Screen.height / 2f + 28, 340, 32), new Color(0, 0, 0, .55f));
            GUI.Label(new Rect(Screen.width / 2f - 170, Screen.height / 2f + 28, 340, 32), focus.Prompt, promptStyle);
        }
    }
}
