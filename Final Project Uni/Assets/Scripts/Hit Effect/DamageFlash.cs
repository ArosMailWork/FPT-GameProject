using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{

    #region Variables
    [SerializeField] public Material FlashMaterial;

    [SerializeField] public float flashTime = 0.25f;

    private SkinnedMeshRenderer[] _meshRenderers;
    private List<Material[]> _oldMaterials = new List<Material[]>();
    private Coroutine _damageFlashCoroutine;

    [SerializeField, CanBeNull] private HitStop _hitStop;

    #endregion

    
    #region UnityMethods

    private void Awake()
    {
        _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        Init();
    }

    #endregion

    #region Events

    private void Init()
    {
        //_oldMaterials = new Material[_meshRenderers.Length];

        //assign mesh renderer materials to _materials
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in _meshRenderers)
        {
            Material[] materials = new Material[skinnedMeshRenderer.materials.Length];
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
            {
                materials[i] = skinnedMeshRenderer.materials[i];
            }
            _oldMaterials.Add(materials);
            //for (int i = 0; i < _meshRenderers.Length; i++)
            //{
            //    _oldMaterials[i] = _meshRenderers[i].material;
            //}
        }
        
    }

    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());

        FindObjectOfType<HitStop>().Stop();
    }
    public IEnumerator DamageFlasher()
    {
        ChangeMaterial();

        yield return new WaitForSeconds(flashTime);

        RevertMaterial();
    }

    public void ChangeMaterial()
    {
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in _meshRenderers)
        {
            Material[] flashMaterials = skinnedMeshRenderer.materials;
            for (int i = 0; i < flashMaterials.Length; i++)
            {
                flashMaterials[i] = FlashMaterial;
            }
            skinnedMeshRenderer.materials = flashMaterials;
        }
        //Debug.Log("Flashed");
    }

    public void RevertMaterial()
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].materials = _oldMaterials[i];
        }
        //Debug.Log("Reverted");

    }

    #endregion

    #region Ults

    [FoldoutGroup("Event Test")]
    [Button]
    public void DoDamageFlash()
    {
        CallDamageFlash();
        
        if(_hitStop != null) _hitStop.Stop();
    }

    #endregion
}
