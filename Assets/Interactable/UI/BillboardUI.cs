using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    void LateUpdate()
    {
        Transform cam = Camera.main.transform;

        // Look at camera but fix inversion
        transform.LookAt(transform.position - cam.forward);
    }
}