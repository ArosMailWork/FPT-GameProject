using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region Variables
    public static ParticleManager Instance { get; private set; }

    // Instead of a single prefab, use a list to store multiple particle prefabs.
    [SerializeField] public List<GameObject> particlePrefabs;
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
    // Overload for spawning by prefab index
    public GameObject SpawnParticle(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        if (prefabIndex >= 0 && prefabIndex < particlePrefabs.Count)
        {
            return PoolManager.Spawn(particlePrefabs[prefabIndex], position, rotation);
        }
        else
        {
            Debug.LogError("Invalid prefab index.");
            return null;
        }
    }

    // Overload for spawning using a specific prefab from the list
    public GameObject SpawnParticle(GameObject particlePrefab, Vector3 position, Quaternion rotation)
    {
        if (particlePrefabs.Contains(particlePrefab))
        {
            return PoolManager.Spawn(particlePrefab, position, rotation);
        }
        else
        {
            Debug.LogError("Particle prefab not found in the list.");
            return null;
        }
    }

    // Set the position of an existing particle system
    public void SetParticlePosition(GameObject particleSystem, Vector3 newPosition)
    {
        particleSystem.transform.position = newPosition;
    }

    // Rotate an existing particle system
    public void RotateParticle(GameObject particleSystem, float rotateX, float rotateY, float rotateZ)
    {
        Quaternion rotation = Quaternion.Euler(rotateX, rotateY, rotateZ);
        particleSystem.transform.rotation = rotation;
    }
    #endregion

    #region Ults
    [FoldoutGroup("Event Test")]
    [Button]
    public void SetParticlePositionAndRotation(GameObject particleSystem, Vector3 newPosition, Quaternion newRotation)
    {
        SetParticlePosition(particleSystem, newPosition);
        RotateParticle(particleSystem, newRotation.eulerAngles.x, newRotation.eulerAngles.y, newRotation.eulerAngles.z);
    }
    
    [FoldoutGroup("Event Test")]
    [Button]
    public GameObject SpawnOppositeParticle(int prefabIndex, Vector3 bulletDirection)
    {
        if (prefabIndex >= 0 && prefabIndex < particlePrefabs.Count)
        {
            Vector3 oppositeDirection = -bulletDirection.normalized;
            return SpawnParticle(prefabIndex, oppositeDirection, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Invalid prefab index.");
            return null;
        }
    }


    //TEST ONLY
    [FoldoutGroup("Event Test")]
    [Button]
    public void PlayAssignedParticle()
    {
        var particle = particlePrefabs[0].GetComponent<ParticleSystem>();
        particle.Play();
    }
    
    public void PlayAssignedParticleParam(string idInput, int intValue)
    {
        Debug.Log("ITS WORKED!!!! " + intValue);
    }
    
    #endregion
}
