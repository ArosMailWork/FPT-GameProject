using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : SerializedMonoBehaviour
{
    #region Variables
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
    public static ParticleManager Instance { get; private set; }
    public bool PrefabManager;

    // Dictionary to map particle names to GameObject references (with ParticleSystem components).
    [SerializeField] public Dictionary<string, GameObject> particleDictionary;
    #endregion

    #region UnityMethod
    private void Awake()
    {
        if (Instance == null && !PrefabManager) Instance = this;
    }
    #endregion

    #region Set Transform
    public GameObject SpawnParticle(string particleName, Vector3 position, Quaternion rotation)
    {
        if (particleDictionary.TryGetValue(particleName, out GameObject particlePrefab))
        {
            return PoolManager.Spawn(particlePrefab, position, rotation);
        }
        else
        {
            Debug.LogError($"Particle '{particleName}' not found in dictionary.");
            return null;
        }
    }
    
    
    
    public void SetParticlePosition(GameObject particleObject, Vector3 newPosition)
    {
        if (particleObject == null) return;
        particleObject.transform.position = newPosition;
    }
    public void RotateParticle(GameObject particleObject, float rotateX, float rotateY, float rotateZ)
    {
        if (particleObject == null) return;
        
        Quaternion rotation = Quaternion.Euler(rotateX, rotateY, rotateZ);
        particleObject.transform.rotation = rotation;
    }
    #endregion

    #region Ults
    [FoldoutGroup("Event")] [Button]
    public void SetParticlePositionAndRotation(GameObject particleObject, Vector3 newPosition, Quaternion newRotation)
    {
        SetParticlePosition(particleObject, newPosition);
        RotateParticle(particleObject, newRotation.eulerAngles.x, newRotation.eulerAngles.y, newRotation.eulerAngles.z);
    }
    [FoldoutGroup("Event")] [Button]
    public GameObject SpawnOppositeParticle(string particleName, Vector3 bulletDirection)
    {
        if (particleDictionary.TryGetValue(particleName, out GameObject particlePrefab))
        {
            Vector3 oppositeDirection = -bulletDirection.normalized;
            return SpawnParticle(particleName, oppositeDirection, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Particle '{particleName}' not found in dictionary.");
            return null;
        }
    }

    public ParticleSystem GetParticleSystem(string particleName)
    {
        if (particleDictionary.TryGetValue(particleName, out GameObject particlePrefab))
        {
            if (particlePrefab.TryGetComponent(out ParticleSystem particleSystem))
            {
                return particleSystem;
            }
            else
            {
                Debug.LogError($"No ParticleSystem component found on '{particleName}'.");
                return null;
            }
        }
        else
        {
            Debug.LogError($"Particle '{particleName}' not found in dictionary.");
            return null;
        }
    }

    
    
    //TEST ONLY
    [Button]
    public void PlayAssignedParticle(string particleName)
    {
        if (particleDictionary.TryGetValue(particleName, out GameObject particlePrefab))
        {
            var particleSystem = particlePrefab.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
            else
            {
                Debug.LogError($"No ParticleSystem component found on '{particleName}'.");
            }
        }
        else
        {
            Debug.LogError($"Particle '{particleName}' not found in dictionary.");
        }
    }
    #endregion
}
