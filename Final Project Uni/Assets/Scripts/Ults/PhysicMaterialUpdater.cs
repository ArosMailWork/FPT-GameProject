using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//just to fix unity dumb shiet
public class PhysicMaterialUpdater : MonoBehaviour
{
    private Collider collider;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider>();
        if (collider == null || collider.sharedMaterial == null)
        {
            return;
        }

      
        collider.material = new PhysicMaterial()
        {
            dynamicFriction = collider.sharedMaterial.dynamicFriction,
            staticFriction = collider.sharedMaterial.staticFriction,
            bounciness = collider.sharedMaterial.bounciness,
            frictionCombine = collider.sharedMaterial.frictionCombine,
            bounceCombine = collider.sharedMaterial.bounceCombine
        };
    }
}
