using UnityEngine;

public class Rotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, Time.fixedDeltaTime * 20.0f, 0);
    }
}
