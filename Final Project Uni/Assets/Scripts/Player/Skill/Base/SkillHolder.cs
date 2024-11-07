using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillHolder : MonoBehaviour
{
    [ReadOnly] public float currentCD;

    [FoldoutGroup("Skill List")]
    public List<GameObject> skillList = new List<GameObject>();
    [FoldoutGroup("Skill List")]
    public List<GameObject> passiveSkillList = new List<GameObject>();
    [FoldoutGroup("Skill List")]
    public List<GameObject> activeSkillList = new List<GameObject>();

    [FoldoutGroup("Current Active Skill")]
    public int currentActiveSkill = 0;
    [FoldoutGroup("Current Active Skill")]
    [ReadOnly] public GameObject activeSkill;
    [FoldoutGroup("Current Active Skill")]
    [ReadOnly] public ISkill currentSkill;

    [FoldoutGroup("Setup")]
    public PlayerController _pc;
    [FoldoutGroup("Setup")]
    public List<GameObject> StartSkill;
    [FoldoutGroup("Setup")] 
    public Image currentSkillUISprite;
    [FoldoutGroup("Setup")] 
    [CanBeNull] public Button SwitchButton;

    public static SkillHolder Instance;

    void Start()
    {
        Instance = this;
        
        // Instantiate and categorize skills based on their type
        foreach (GameObject skillPrefab in StartSkill)
        {
            AddSkill(skillPrefab);
        }

        // Set the initial active skill from the ActiveSkillList
        SetActiveSkill(currentActiveSkill);
    }

    [Button]
    void SetActiveSkill(int slot)
    {
        if (slot >= 0 && slot < activeSkillList.Count)
        {
            currentActiveSkill = slot;
            activeSkill = activeSkillList[slot];
            currentSkill = activeSkill.GetComponent<ISkill>();
            currentSkillUISprite.sprite = currentSkill.Icon;
        }
    }

    #region Util
    // Utility method for switching to the next active skill
    public void NextSkill()
    {
        if (_pc.blockInput) return;
        Debug.Log("next");
        
        if (currentActiveSkill + 1 < activeSkillList.Count)
            currentActiveSkill += 1;
        else
            currentActiveSkill = 0;

        SetActiveSkill(currentActiveSkill);
    }
    

    [Button]
    public void AddSkill(GameObject skillPrefab)
    {
        GameObject skillInstance = Instantiate(skillPrefab);
        skillInstance.transform.SetParent(this.transform);
        skillInstance.transform.localPosition = Vector3.zero;
        skillInstance.transform.localRotation = Quaternion.identity;

        ISkill skillComponent = skillInstance.GetComponent<ISkill>();
        skillComponent.Assign(_pc);

        // Add the skill to the appropriate list based on its type
        if (skillComponent.type == SkillType.ACTIVE)
        {
            activeSkillList.Add(skillInstance);  // Add to active skill list
        }
        else if (skillComponent.type == SkillType.PASSIVE)
        {
            passiveSkillList.Add(skillInstance);  // Add to passive skill list
            skillComponent.Activate();  // Auto-activate passive skills
        }

        skillList.Add(skillInstance);  // Add all skills to the master list
        //Debug.Log("Add " + skillComponent.type + " Skill: " + skillComponent.name);

        if (SwitchButton != null)
        {
            Debug.Log(activeSkillList.Count);
            SwitchButton.gameObject.SetActive((activeSkillList.Count > 1));
            if(activeSkillList.Count == 1) SetActiveSkill(0);
        }
    }
    
    #endregion

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
