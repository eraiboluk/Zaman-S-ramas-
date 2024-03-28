using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform targetCamera;

    void Update()
    {
        if (targetCamera != null)
        {
            Vector3 lookDirection = targetCamera.position - transform.position;

            lookDirection.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookDirection);

            rotation *= Quaternion.Euler(0, 180, 0);

            transform.rotation = rotation;
        }
    }
}
