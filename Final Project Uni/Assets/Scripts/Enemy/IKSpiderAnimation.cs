using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class IKSpiderAnimation : MonoBehaviour
{   
    [FoldoutGroup("Setup")]
    public Transform IKRigPos;
    [FoldoutGroup("Setup")]
    public float maxPositionOffset = 2f; // Maximum offset value for IKRigPos
    
    [FoldoutGroup("LegMovement")]
    public float height = 0.25f;
    [FoldoutGroup("LegMovement")]
    public float stepSize = 0.15f;
    [FoldoutGroup("LegMovement")]
    public AnimationCurve LegMoveCurve;
    [FoldoutGroup("LegMovement")]
    public float totalDuration = 0.2f; // Total duration for the leg movement
    [FoldoutGroup("LegMovement")]
    public float velocityMultiplier = 1f; // this broken, lol
    
    [FoldoutGroup("SpiderLeg")]
    public Transform[] legTargets;
    [FoldoutGroup("SpiderLeg")]
    public bool[] legMoving;
    [FoldoutGroup("SpiderLeg")]
    public bool oddMovedLast = true; // Start with odd legs moving first
    
    int legTargetLength;
    private Vector3[] defaultLegPositions;
    private Vector3[] lastLegPositions;
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastBodyPos;
    private float raycastRange = 1f;
    Rigidbody rb;

    static Vector3[] MatchToSurfaceFromAbove(Vector3 point, float halfRange, Vector3 up)
    {
        Vector3[] res = new Vector3[2];
        RaycastHit hit;
        Ray ray = new Ray(point + halfRange * up, -up);

        if (Physics.Raycast(ray, out hit, 2f * halfRange))
        {
            res[0] = hit.point;
            res[1] = hit.normal;
        }
        else
        {
            res[0] = point;
        }
        return res;
    }

    #region Zic-zac pattern

    bool ToggleOddMoveLast()
    {
        for (int i = 0; i < legTargets.Length; ++i)
        {
            legMoving[i] = false;
        }
        return !oddMovedLast;

    }
    // Method to check if all odd legs are true
    public bool allOddLegsTrue()
    {
        for (int i = 0; i < legMoving.Length; i += 2)
        {
            if (!legMoving[i])
            {
                //StartCoroutine(PerformStep(i, defaultLegPositions[i]));
                return false;
            }
        }
        return true;
    }
    // Method to check if all even legs are true
    public bool allEvenLegsTrue()
    {
        for (int i = 1; i < legMoving.Length; i += 2)
        {
            if (!legMoving[i])
            {
                //StartCoroutine(PerformStep(i, defaultLegPositions[i]));
                return false;
            }
        }
        return true;
    }
    
    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        legTargetLength = legTargets.Length;
        defaultLegPositions = new Vector3[legTargetLength];
        lastLegPositions = new Vector3[legTargetLength];
        legMoving = new bool[legTargetLength];

        for (int i = 0; i < legTargetLength; ++i)
        {
            defaultLegPositions[i] = legTargets[i].position;
            legMoving[i] = false;
            lastLegPositions[i] = defaultLegPositions[i];
        }
        lastBodyPos = transform.position;
    }



    void FixedUpdate()
    {
        // Calculate Vector3 Velocity of the Spider
        //velocity = lastBodyPos - transform.position;
        
        velocity = lastBodyPos - transform.position;
        if (velocity.y > 0) velocity.y = -velocity.y;
        velocity = (velocity + Time.fixedDeltaTime * lastVelocity * velocityMultiplier) / (Time.fixedDeltaTime + 1);
        
        if (velocity.magnitude < 0.000025f) velocity = lastVelocity;
        else lastVelocity = velocity;

        velocity.y = Mathf.Clamp(velocity.y, 0f, float.MaxValue);

        Vector3[] wishPos = new Vector3[legTargets.Length];

        // Variables to keep track of the last moved leg
        int lastMovedIndex = -1;
        float maxDistance = stepSize;
        // Loop through each leg
        for (int i = 0; i < legTargets.Length; ++i)
        {   
            legTargets[i].position = lastLegPositions[i];

            wishPos[i] = Vector3.ProjectOnPlane(defaultLegPositions[i] - (velocity), transform.up);
            
            Vector3[] positionAndNormal = MatchToSurfaceFromAbove(wishPos[i], raycastRange, transform.up);
            defaultLegPositions[i] = positionAndNormal[0];

            // Skip legs that have already moved
            if (i == lastMovedIndex)
                continue;

            // Move odd legs first
            if (oddMovedLast && i % 2 != 0)
            {   
                MoveLeg(i, wishPos, ref maxDistance);
                lastMovedIndex = i;
            }
            // Move even legs after odd legs
            if (!oddMovedLast && i % 2 == 0)
            {
                MoveLeg(i, wishPos, ref maxDistance);
                lastMovedIndex = i;
            }
        }
        
        lastBodyPos = transform.position;
    }
    
    
    IEnumerator PerformStep(int index, Vector3 targetPoint)
    {
        Vector3 startPos = lastLegPositions[index];
        lastLegPositions[index] = defaultLegPositions[index];
        
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float curveTime = elapsedTime / totalDuration;
            float yOffset = LegMoveCurve.Evaluate(curveTime) * height;
            
            Vector3 nextPos = Vector3.Lerp(startPos, targetPoint, curveTime);
            legTargets[index].position = nextPos + transform.up * yOffset;

            elapsedTime += Time.fixedDeltaTime;
            yield return null; // Wait for the next frame
        }

        elapsedTime = 0f;
        while (elapsedTime < totalDuration)
        {
            float curveTime = elapsedTime / totalDuration;
            Vector3 nextPos = Vector3.Lerp(startPos, targetPoint, curveTime);
            legTargets[index].position = nextPos;
            
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        legTargets[index].position = targetPoint;
        lastLegPositions[index] = legTargets[index].position;
        legMoving[0] = false;
        yield return new WaitForSeconds(0.2f); // Introduce a 0.1 second delay
    }
    
    void MoveLeg(int index, Vector3[] desiredPositions, ref float maxDistance)
    {
        float distance = Vector3.Distance(legTargets[index].position, defaultLegPositions[index]);
        
        if (distance > maxDistance+maxDistance*10/100)
        {
            maxDistance = distance;
            StartCoroutine(PerformStep(index, defaultLegPositions[index]));
            legMoving[index] = true;
            
        }
        if (allEvenLegsTrue())
        {
            oddMovedLast = ToggleOddMoveLast();
            //Debug.Log("Even");
        }
        if (allOddLegsTrue())
        {
            oddMovedLast = ToggleOddMoveLast();
            //Debug.Log("Odd");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(!this.enabled) return;
        for (int i = 0; i < legTargetLength; ++i)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(defaultLegPositions[i], 0.05f); 
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(legTargets[i].position, stepSize);
        }
    }
}
