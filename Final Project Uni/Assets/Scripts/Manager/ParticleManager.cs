using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region Variables
    public static ParticleManager Instance { get; private set; }


    [SerializeField] public GameObject prefab;
    #endregion

    #region UnityMethod
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion



    #region Event
    public GameObject SpawnParticle(GameObject particleSystem, Vector3 position, Quaternion rotation)
    {
        return PoolManager.Spawn(particleSystem, position, rotation);
    }

    // Set the position of an existing particle system
    public void SetParticlePosition(GameObject particleSystem, Vector3 newPosition)
    {
        particleSystem.transform.position = newPosition;
    }

    // Rotate an existing particle system
    public void RotateParticle(GameObject particleSystem, float rotateX, float rotateY, float RotateZ)
    {
        Quaternion rotation = Quaternion.Euler(rotateX, rotateY, RotateZ);
        particleSystem.transform.rotation = rotation;
    }

    #endregion

    #region Ults
    [FoldoutGroup("Event Test")]
    [Button]
    public void setParticlePositionAndRotation(GameObject particleSystem, Vector3 newPosition, Quaternion newRotation)
    {
        SetParticlePosition(particleSystem, newPosition);
        RotateParticle(particleSystem, newRotation.x, newRotation.y, newRotation.z);
    }
    [FoldoutGroup("Event Test")]
    [Button]
    public GameObject SpawnOppositeParticle(GameObject particlePrefab, Vector3 bulletDirection)
    {
     
        Vector3 oppositeDirection = -bulletDirection.normalized;

        return SpawnParticle(particlePrefab, oppositeDirection, Quaternion.identity);
    }
    #endregion

}
