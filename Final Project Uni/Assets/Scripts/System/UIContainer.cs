using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContainer : MonoBehaviour
{
    public GameObject Gameplay;
    public GameObject SkillChoose;
    public GameObject Transition;

    public SelectRandomSkillEvent s;

    void Start()
    {
        s = SkillChoose.GetComponent<SelectRandomSkillEvent>();
        GameplayState();
    }
    public void GameplayState()
    {
        SkillChoose.SetActive(false);
        Transition.SetActive(false);
        Gameplay.SetActive(true);
    }
    public void SkillChooseState()
    {
        SkillChoose.SetActive(true);
        Gameplay.SetActive(false);
        s.SkillSelectStart();
    }
    public void TransitionState()
    {
        Transition.SetActive(true);
        Gameplay.SetActive(false);
    }

}