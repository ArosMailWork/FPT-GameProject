using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSystem : MonoBehaviour
{
    protected Character character;
    protected virtual void Awake()
    {
        character = transform.root.GetComponent<Character>();
    }
}
