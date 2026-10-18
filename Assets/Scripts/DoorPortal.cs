using UnityEngine;

public class DoorPortal : Interactable
{
    public float openHeight = 3.3f;
    Vector3 closedPos;
    bool opening, queued, tinted;
    float t;
    Renderer rend;

    public override string Prompt
    {
        get
        {
            if (opening) return "";
            LevelManager lm = LevelManager.Instance;
            if (lm != null && !lm.Done) return "Door locked - finish the objective";
            return "Press E to open the door";
        }
    }

    void Start() { closedPos = transform.position; rend = GetComponent<Renderer>(); }

    void Update()
    {
        LevelManager lm = LevelManager.Instance;
        if (!tinted && lm != null && lm.Done && rend != null) { rend.material.color = new Color(.1f, .75f, .3f); tinted = true; }
        if (!opening) return;
        t += Time.deltaTime;
        float k = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / 1.2f));
        transform.position = closedPos + Vector3.up * (openHeight * k);
        if (t > 1.6f && !queued && lm != null) { queued = true; lm.LoadNext(); }
    }

    public override void Interact()
    {
        if (opening) return;
        LevelManager lm = LevelManager.Instance;
        if (lm == null) return;
        if (!lm.Done) { lm.Show("Door locked: " + lm.objective + " (" + lm.Count + "/" + lm.required + ")"); return; }
        opening = true;
        lm.Show("Door opening...");
    }
}
