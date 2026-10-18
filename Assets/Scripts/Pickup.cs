using UnityEngine;

public class Pickup : Interactable
{
    public float spinSpeed = 60f, bobHeight = .12f, bobSpeed = 2f;
    Vector3 start;
    bool taken;

    void Start() { start = transform.position; }

    void Update()
    {
        if (taken) return;
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        transform.position = start + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
    }

    public override void Interact()
    {
        if (taken) return;
        taken = true;
        if (LevelManager.Instance != null) LevelManager.Instance.Add();
        Destroy(gameObject);
    }
}
