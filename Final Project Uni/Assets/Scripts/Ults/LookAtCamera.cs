using UnityEngine;

[ExecuteInEditMode]
public class LookAtCamera : MonoBehaviour
{
    public Camera targetCamera;
    public bool useMainCamera = true;
    public Vector3 offset = Vector3.zero;

    private void Start()
    {
        if (useMainCamera)
        {
            targetCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (targetCamera != null)
        {
            Vector3 directionToCamera = targetCamera.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-directionToCamera);
            transform.Rotate(offset);
        }
        else
        {
            Debug.LogWarning("LookAtCamera: No target camera assigned!");
        }
    }
}
