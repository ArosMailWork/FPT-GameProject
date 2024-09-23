using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
 public class SeeThoughDetector : MonoBehaviour
{
    [FoldoutGroup("Stats")]
    public float MaskTime = 0.5f, unMaskTime = 0.25f, seeThroughSize = 5, sphereRadius = 0.5f;
    [FoldoutGroup("Debug")]
    public bool hitting;
    [FoldoutGroup("Debug/Setup")]
    public GameObject camera, target;
    [FoldoutGroup("Debug/Setup")]
    public LayerMask myLayerMask;
    
    // Calculate
    private RaycastHit hit;
    private Vector3 direction;
    private float distance;
    private bool isShrinking; // Track the previous hitting state
    
    void FixedUpdate()
    {
        // Calculate the direction from the camera to the target
        direction = (target.transform.position - camera.transform.position).normalized;
        distance = Vector3.Distance(camera.transform.position, target.transform.position) - (sphereRadius * 2);
        
         // Perform a sphere cast using the calculated distance
        hitting = Physics.SphereCast(camera.transform.position, sphereRadius, direction, out hit, distance, myLayerMask);

        HitCheck(hitting);
    }

    public void HitCheck(bool hitting)
    {
        if (hitting)
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            if (!hit.collider.gameObject.CompareTag("StencilMask"))
                target.transform.DOScale(seeThroughSize, MaskTime);
        }
        else
        {
            Debug.Log("No Hit");
            target.transform.DOScale(0, unMaskTime);
            
        }
    }
    
    void OnDrawGizmos()
    {
        if (camera != null && target != null)
        {
            // Set the color for the Gizmos
            Gizmos.color = hitting ? Color.green : Color.red;
             // Draw the sphere cast line
            Gizmos.DrawLine(camera.transform.position, target.transform.position); // Draw a line from the camera to the target
            
            /*
             
            Gizmos.DrawSphere(camera.transform.position, sphereRadius);
            Gizmos.DrawSphere(target.transform.position, sphereRadius);
            if (hitting)
                Gizmos.DrawSphere(hit.point, sphereRadius);
             
            */
        }
    }
}