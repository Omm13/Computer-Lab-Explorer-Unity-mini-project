using UnityEngine;

public static class UiUtil
{
    public static void Fill(Rect r, Color c)
    {
        Color old = GUI.color;
        GUI.color = c;
        GUI.DrawTexture(r, Texture2D.whiteTexture);
        GUI.color = old;
    }

    public static GUIStyle Label(int size, FontStyle fs, Color col, TextAnchor anchor)
    {
        GUIStyle s = new GUIStyle(GUI.skin.label);
        s.fontSize = size; s.fontStyle = fs; s.alignment = anchor;
        s.normal.textColor = col;
        return s;
    }

    public static GUIStyle Button(int size)
    {
        GUIStyle s = new GUIStyle(GUI.skin.button);
        s.fontSize = size; s.fontStyle = FontStyle.Bold;
        return s;
    }

    public static float Scale() { return Mathf.Clamp(Screen.height / 800f, .8f, 1.4f); }

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
