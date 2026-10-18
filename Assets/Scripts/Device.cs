using UnityEngine;

// A workstation / server that can be switched on. Animates a pulsing screen and a "pop".
public class Device : Interactable
{
    public Renderer screen;
    public Color offColor = new Color(.03f, .05f, .07f);
    public Color onColor = new Color(.1f, 1f, .45f);
    bool on;
    float t, punch;
    Vector3 baseScale;

    public override string Prompt { get { return on ? "" : prompt; } }

    void Start() { baseScale = transform.localScale; }

    public override void Interact()
    {
        if (on) return;
        on = true;
        punch = .4f;
        if (LevelManager.Instance != null) LevelManager.Instance.Add();
    }

    void Update()
    {
        if (!on) return;
        t += Time.deltaTime;
        if (punch > 0f) punch -= Time.deltaTime;
        transform.localScale = baseScale * (1f + Mathf.Max(0f, punch) * .25f);
        if (screen != null)
        {
            float p = .65f + .35f * Mathf.Sin(t * 4f);
            Color c = Color.Lerp(offColor, onColor, p);
            c.a = 1f;
            screen.material.color = c;
        }
    }
}
