using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillHolder : MonoBehaviour
{
    public PlayerController _pc;
    public int currentActiveSkill = 0;
    public List<GameObject> skillPrefabs;  // List of skill prefabs
    private List<GameObject> skillList = new List<GameObject>(); // List of instantiated skills
    private GameObject activeSkill;
    private ISkill currentSkill; 
    
    [ReadOnly] public float currentCD;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate all skills from the skillPrefabs list
        foreach (GameObject skillPrefab in skillPrefabs)
        {
            AddSkill(skillPrefab);
        }

        // Set the initial active skill
        SetActiveSkill(currentActiveSkill);
    }

    [Button]
    void SetActiveSkill(int slot)
    {
        if (slot >= 0 && slot < skillList.Count)
        {
            activeSkill = skillList[slot];
            currentSkill = activeSkill.GetComponent<ISkill>();
        }
    }

    #region Util
    // Utility method for switching to the next skill
    public void NextSkill(InputAction.CallbackContext ctx)
    {
        if (currentActiveSkill + 1 < skillList.Count)
            currentActiveSkill += 1;
        else
            currentActiveSkill = 0;
        
        SetActiveSkill(currentActiveSkill);
    }
    #endregion

    // Method to instantiate and add a skill to the skillList
    void AddSkill(GameObject skillPrefab)
    {
        GameObject skillInstance = Instantiate(skillPrefab);
        skillInstance.transform.SetParent(this.transform);
        skillInstance.transform.localPosition = Vector3.zero;
        skillInstance.transform.localRotation = Quaternion.identity;
        skillInstance.GetComponent<ISkill>().Assign(_pc);
        skillList.Add(skillInstance);
    }

    #region Input
    public void ActivateSkill(InputAction.CallbackContext ctx)
    {
        ActivateSkill();
    }

    public void DeactivateSkill(InputAction.CallbackContext ctx)
    {
        DeactivateSkill();
    }

    public void ActivateSkill()
    {
        if (activeSkill == null) return;
        currentSkill.Activate();
        
    }

    public void DeactivateSkill()
    {
        if (activeSkill == null) return;
        currentSkill.Deactivate();
    }
    #endregion

    #region Timer
    public void Timer()
    {
        if (currentCD >= 0) currentCD -= Time.deltaTime;
    }

    public void TimerAdd(float addTime)
    {
        currentCD += addTime;
    }
    #endregion
}
