using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    GUIStyle title, sub, btn, small;

    void Start() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; Time.timeScale = 1f; }

    void OnGUI()
    {
        if (title == null)
        {
            title = UiUtil.Label(34, FontStyle.Bold, new Color(.2f, .9f, 1f), TextAnchor.MiddleCenter);
            sub = UiUtil.Label(16, FontStyle.Normal, Color.white, TextAnchor.MiddleCenter);
            small = UiUtil.Label(14, FontStyle.Normal, new Color(.75f, .85f, .92f), TextAnchor.MiddleCenter);
            btn = UiUtil.Button(22);
        }
        float s = UiUtil.Scale();
        Matrix4x4 old = GUI.matrix;
        GUI.matrix = Matrix4x4.Scale(new Vector3(s, s, 1));
        float w = Screen.width / s, h = Screen.height / s;
        float x = w * 0.28f;

        UiUtil.Fill(new Rect(x - 270, h / 2 - 230, 540, 460), new Color(.02f, .05f, .08f, .9f));
        GUI.Label(new Rect(x - 260, h / 2 - 215, 520, 50), "COMPUTER LAB EXPLORER", title);
        GUI.Label(new Rect(x - 260, h / 2 - 165, 520, 28), "Interactive 3D tour  -  5 scenes", sub);
        if (GUI.Button(new Rect(x - 130, h / 2 - 110, 260, 56), "START TOUR", btn))
        {
            Progress.Reset();
            SceneManager.LoadScene("ComputerLab");
        }
        if (GUI.Button(new Rect(x - 130, h / 2 - 40, 260, 56), "QUIT", btn)) UiUtil.Quit();
        GUI.Label(new Rect(x - 260, h / 2 + 40, 520, 140),
            "Rooms:  Computer Lab  >  Server Room  >  Control Room\n\n" +
            "WASD move  |  Mouse look  |  Space jump\nE interact  |  Esc release cursor", small);
        GUI.matrix = old;
    }
}
