using UnityEngine;
using UnityEngine.SceneManagement;

public static class Progress
{
    public static int itemsTotal;
    public static float startTime;
    public static void Reset() { itemsTotal = 0; startTime = Time.realtimeSinceStartup; }
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public string levelTitle = "Room";
    public string objective = "Complete the task";
    public int required = 1;
    public int levelNumber = 1, levelTotal = 3;
    public string nextScene = "";

    int count;
    float fade = 1f, fadeTarget = 0f, messageUntil;
    string message = "";
    bool loading;
    GUIStyle title, body, small, prompt;

    public int Count { get { return count; } }
    public bool Done { get { return count >= required; } }

    void Awake() { Instance = this; Time.timeScale = 1f; }
    void Start() { Show(objective + " (0/" + required + ")", 4f); }

    public void Show(string text, float seconds = 3f) { message = text; messageUntil = Time.time + seconds; }

    public void Add()
    {
        count++;
        Progress.itemsTotal++;
        Show(Done ? "Objective complete! The exit door is unlocked." : objective + ": " + count + "/" + required);
    }

    public void LoadNext() { fadeTarget = 1f; }

    void Update()
    {
        fade = Mathf.MoveTowards(fade, fadeTarget, Time.deltaTime * 1.5f);
        if (!loading && fadeTarget >= 1f && fade >= 1f && nextScene != "")
        {
            loading = true;
            SceneManager.LoadScene(nextScene);
        }
    }

    void OnGUI()
    {
        GUI.depth = 0;
        if (title == null)
        {
            title = UiUtil.Label(20, FontStyle.Bold, new Color(.2f, .9f, 1f), TextAnchor.MiddleLeft);
            body = UiUtil.Label(15, FontStyle.Normal, Color.white, TextAnchor.MiddleLeft);
            small = UiUtil.Label(12, FontStyle.Normal, new Color(.75f, .85f, .92f), TextAnchor.MiddleLeft);
            prompt = UiUtil.Label(16, FontStyle.Normal, Color.white, TextAnchor.MiddleCenter);
        }
        float s = UiUtil.Scale();
        Matrix4x4 old = GUI.matrix;
        GUI.matrix = Matrix4x4.Scale(new Vector3(s, s, 1));
        float w = Screen.width / s, h = Screen.height / s;

        UiUtil.Fill(new Rect(16, 16, 340, 132), new Color(.02f, .05f, .08f, .88f));
        GUI.Label(new Rect(30, 22, 320, 28), "ROOM " + levelNumber + "/" + levelTotal + "  -  " + levelTitle, title);
        GUI.Label(new Rect(30, 54, 320, 24), objective, body);
        float p = required > 0 ? Mathf.Clamp01((float)count / required) : 1f;
        UiUtil.Fill(new Rect(30, 88, 300, 14), new Color(1, 1, 1, .15f));
        UiUtil.Fill(new Rect(30, 88, 300 * p, 14), Done ? new Color(.2f, 1f, .5f) : new Color(.2f, .8f, 1f));
        GUI.Label(new Rect(30, 108, 320, 22), count + " / " + required + (Done ? "   exit unlocked" : ""), small);

        GUI.Label(new Rect(w - 250, 20, 240, 22), "WASD Move  |  Mouse Look", small);
        GUI.Label(new Rect(w - 250, 42, 240, 22), "E Interact  |  Space Jump", small);
        GUI.Label(new Rect(w - 250, 64, 240, 22), "Esc Release Cursor", small);

        if (Time.time < messageUntil)
        {
            UiUtil.Fill(new Rect(w / 2 - 300, h - 90, 600, 46), new Color(.02f, .04f, .06f, .9f));
            GUI.Label(new Rect(w / 2 - 290, h - 90, 580, 46), message, prompt);
        }
        if (fade > 0.001f) UiUtil.Fill(new Rect(0, 0, w, h), new Color(0, 0, 0, fade));
        GUI.matrix = old;
    }
}
