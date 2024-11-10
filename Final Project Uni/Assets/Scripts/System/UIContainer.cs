using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIContainer : MonoBehaviour
{
    public List<GameObject> Gameplay;
    public GameObject SkillChoose;
    public GameObject Transition;
    public GameObject Inventory;
    public GameObject Fade;
    public SelectRandomSkillEvent s;
    public Animator FadeAnimator;

    void Start()
    {
        s = SkillChoose.GetComponent<SelectRandomSkillEvent>();
        //GameplayState();
    }
    public void GameplayState()
    {
        SkillChoose.SetActive(false);
        Transition.SetActive(false);
        Fade.SetActive(false);
        Inventory.SetActive(false);

        foreach (var obj in Gameplay)
        {
            obj.SetActive(true);
        }


    }
    public void InventoryState()
    {
        foreach (var obj in Gameplay)
        {
            obj.SetActive(false);
        }
        Inventory.SetActive(true);
    }
    public void SkillChooseState()
    {
        SkillChoose.SetActive(true);
        foreach (var obj in Gameplay)
        {
            obj.SetActive(false);
        }
        s.SkillSelectStart();
    }
    public void TransitionState()
    {
        Transition.SetActive(true);
        foreach (var obj in Gameplay)
        {
            obj.SetActive(false);
        }
    }
    public void FadeAnim()
    {
        Fade.SetActive(true);
        foreach (var obj in Gameplay)
        {
            obj.SetActive(false);
        }
    }

    IEnumerator FadeInAnimation()
    {
        FadeAnim();
        FadeAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
    }
    IEnumerator FadeOutAnimation()
    {
        FadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        GameplayState();
    }

    [Button]
    public void FadeIn()
    {
        //StartCoroutine(FadeInAnimation());
        FadeAnim();
        FadeAnimator.SetTrigger("FadeIn");
    }


    [Button]
    public void FadeOut()
    {
        StartCoroutine(FadeOutAnimation());
    }
}