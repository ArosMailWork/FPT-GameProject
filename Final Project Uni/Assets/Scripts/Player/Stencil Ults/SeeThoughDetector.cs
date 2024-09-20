using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class SeeThoughDetector : MonoBehaviour
{
    public GameObject camera, target;
    public float MaskTime = 0.5f, unMaskTime = 0.25f, size = 5;
    public LayerMask myLayerMask;
    void Update()
    {
        RaycastHit hit;
        // Calculate the direction from the camera to the target
        Vector3 direction = (target.transform.position - camera.transform.position).normalized;
        // Check if the ray intersects with any collider in the specified layer mask
        if (Physics.Raycast(camera.transform.position, direction, out hit, Mathf.Infinity, myLayerMask))
        {
            // If it collides with the sphere, scale it to 0 with Dotween
            if (hit.collider.gameObject.CompareTag("StencilMask"))
            {
                target.transform.DOScale(0, MaskTime);
            }
        }
        else
        {
            // If it does not collide, scale it to 18
            target.transform.DOScale(size, unMaskTime);
        }
    }
}