using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class CraftingTableUI : MonoBehaviour
{
    [HideInInspector] public CraftingTable craftingTable;

    private void Awake()
    {
        craftingTable = GetComponentInParent<CraftingTable>();
    }

    [Button("Update Recipe")]
    public void OnEnable()
    {
        Debug.Log("yes ?");
        craftingTable.UpdateRecipeList();
    }
}