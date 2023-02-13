
using UnityEngine;

public class ChangeDayTime : MonoBehaviour
{
    public float rotationSpeed = 5f;

    void Update()
    {
        transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
    }
}
