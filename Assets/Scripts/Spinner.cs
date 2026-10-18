using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Vector3 degreesPerSecond = new Vector3(0, 40, 0);
    void Update() { transform.Rotate(degreesPerSecond * Time.deltaTime, Space.World); }
}
