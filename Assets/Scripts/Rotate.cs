using UnityEngine;

public class Rotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * 0.5f, 0);
    }
}
