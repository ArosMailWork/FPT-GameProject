using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField, ReadOnly]
    PlayerStats playerStat;
    [SerializeField, ReadOnly]
    SkillHolder skillHolder;
    public List<GameObject> skillList = new List<GameObject>();
    public List<Image> skillHolderObj;
    // Start is called before the first frame update
    [Button]
    public void UpdateInfo()
    {
        playerStat = PlayerController.Instance._stats;
        skillHolder = SkillHolder.Instance;
        skillList = skillHolder.skillList;
        for (int i = 0; i < skillList.Count; i++)
        {
            skillHolderObj[i].sprite = skillList[i].GetComponent<ISkill>().Icon;
        }
    }

}
