using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillHolder : MonoBehaviour
{
    public PlayerController _pc;
    public int currentSkill = 0;
    public List<GameObject> skillList;
    public GameObject activeSkill;
    public GameObject skillPrefab1;
    public GameObject skillPrefab2;

    float currentCD;

    // Start is called before the first frame update
    void Start()
    {
        AddSkill(skillPrefab1);
        AddSkill(skillPrefab2);
        SetActiveSkill(0);
        activeSkill.GetComponent<ISkill>().Activate();
    }
    void SetActiveSkill(int slot)
    {
        activeSkill = skillList[slot];

    }
    #region Util
    //Util 
    public void NextSkill(InputAction.CallbackContext ctx)
    {
        if (currentSkill + 1 < skillList.Count)
        {
            activeSkill = skillList[currentSkill + 1];
            currentSkill += 1;
        }
        else
        {
            activeSkill = skillList[0];
            currentSkill = 0;
        }
    }
    #endregion
    void AddSkill(GameObject skill)
    {
        GameObject g = Instantiate(skill);
        g.transform.SetParent(this.transform);
        g.GetComponent<ISkill>().Assign(_pc);
        skillList.Add(g);
    }
    #region Input
    public void ActivateSkill(InputAction.CallbackContext ctx)
    {
        activeSkill.GetComponent<ISkill>().Activate();
        TimerAdd(activeSkill.GetComponent<ISkill>().Cooldown);
    }
    public void ActivateSkill()
    {
        activeSkill.GetComponent<ISkill>().Activate();
        TimerAdd(activeSkill.GetComponent<ISkill>().Cooldown);
    }
    #endregion

    #region Timer
    public void Timer()
    {
        if (currentCD >= 0)
            currentCD -= Time.deltaTime;
    }

    public void TimerAdd(float addTime)
    {
        currentCD += addTime;
    }
    #endregion
}
