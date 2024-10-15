using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Basic Move Object
public class DynamicObstacle : MonoBehaviour
{
    public Transform mesh;  // Transform of the obstacle to move
    public List<HurtBox> HurtBoxes;  // List of HurtBoxes to toggle
    public List<Transform> destinationList;  // List of destination positions

    // Expose Ease to be set in the Inspector
    public Ease moveEase = Ease.InOutQuad;  // Default ease set to InOutQuad
    public float moveTime;

    Vector3 defaultPos;  // Store the initial position
    
    private void Start()
    {
        // Save the initial position as the default position
        defaultPos = mesh.position;
    }

    // Toggle HurtBox activation
    public void ToggleHurtBox(bool toggle)
    {
        Debug.Log("yes ? " + toggle);
        foreach (var hurtBox in HurtBoxes)
        {
            hurtBox.Activate = toggle;
        }
    }

    // Method to move the obstacle to a specific destination over 'moveTime'
    public void MoveToDestination(int destinationIndex)
    {
        if (destinationIndex >= 0 && destinationIndex < destinationList.Count)
        {
            Vector3 targetPos = destinationList[destinationIndex].position;
            mesh.DOMove(targetPos, moveTime).SetEase(moveEase);
        }
        else
            Debug.LogWarning("Invalid destination index");
    }

    // Move back to the default position
    public void MoveToDefault()
    {
        mesh.DOMove(defaultPos, moveTime).SetEase(moveEase);
    }
}