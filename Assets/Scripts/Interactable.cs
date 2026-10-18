using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string prompt = "Press E to interact";
    public virtual string Prompt { get { return prompt; } }
    public abstract void Interact();
}
