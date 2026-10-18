using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
{
    GUIStyle title, body, btn;
    float finishTime = -1f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None; Cursor.visible = true; Time.timeScale = 1f;
        finishTime = Time.realtimeSinceStartup - Progress.startTime;
    }

    void OnGUI()
    {
        if (title == null)
        {
            title = UiUtil.Label(38, FontStyle.Bold, new Color(.2f, 1f, .6f), TextAnchor.MiddleCenter);
            body = UiUtil.Label(18, FontStyle.Normal, Color.white, TextAnchor.MiddleCenter);
            btn = UiUtil.Button(22);
        }
        float s = UiUtil.Scale();
        Matrix4x4 old = GUI.matrix;
        GUI.matrix = Matrix4x4.Scale(new Vector3(s, s, 1));
        float w = Screen.width / s, h = Screen.height / s;
        float x = w * 0.28f;
        int secs = Mathf.Max(0, Mathf.RoundToInt(finishTime));

        UiUtil.Fill(new Rect(x - 270, h / 2 - 200, 540, 400), new Color(.01f, .04f, .05f, .92f));
        GUI.Label(new Rect(x - 260, h / 2 - 185, 520, 56), "MISSION COMPLETE", title);
        GUI.Label(new Rect(x - 260, h / 2 - 110, 520, 30), "You powered up the whole lab.", body);
        GUI.Label(new Rect(x - 260, h / 2 - 70, 520, 30), "Objectives done: " + Progress.itemsTotal + " / 10", body);
        GUI.Label(new Rect(x - 260, h / 2 - 40, 520, 30), "Time: " + (secs / 60) + "m " + (secs % 60) + "s", body);
        if (GUI.Button(new Rect(x - 130, h / 2 + 20, 260, 56), "PLAY AGAIN", btn)) SceneManager.LoadScene("MainMenu");
        if (GUI.Button(new Rect(x - 130, h / 2 + 90, 260, 56), "QUIT", btn)) UiUtil.Quit();
        GUI.matrix = old;
    }
}
