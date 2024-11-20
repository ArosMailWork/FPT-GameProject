using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public enum UIState
{
    Gameplay, Event, Inventory
}

public class UIContainer : MonoBehaviour
{
    public UIState CurrentUIState;
    public List<GameObject> Gameplay;
    public GameObject SkillChoose;
    [CanBeNull] public GameObject Transition;
    public GameObject Inventory;
    public GameObject Fade;
    public Image FadeImage;
    public SelectRandomSkillEvent s;
    public Animator FadeAnimator;

    void OnEnable()
    {
        s = SkillChoose.GetComponent<SelectRandomSkillEvent>();
        FadeAnimator.SetTrigger("FadeOut");
        //GameplayState();
    }
    public void GameplayState()
    {
        CurrentUIState = UIState.Gameplay;
        
        SkillChoose.SetActive(false);
        if(Transition != null) Transition.SetActive(false);
        Fade.SetActive(false);
        Inventory.SetActive(false);

        foreach (var obj in Gameplay)
        {
            obj.SetActive(true);
        }
    }
    public void InventoryState()
    {
        CurrentUIState = UIState.Inventory;
        
        foreach (var obj in Gameplay)
        {
            obj.SetActive(false);
        }
        Inventory.SetActive(true);
    }
    public void SkillChooseState()
    {
        CurrentUIState = UIState.Event;
        
        SkillChoose.SetActive(true);
        foreach (var obj in Gameplay)
        {
            obj.SetActive(false);
        }
        s.SkillSelectStart();
    }
    public void TransitionState()
    {
        if(Transition != null) Transition.SetActive(true);
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

    public void SetColorFade(Color color = default)
    {
        if (color == default) color = Color.black; // Set default to black if not provided
    
        Color currentColor = FadeImage.color;
        FadeImage.color = new Color(color.r, color.g, color.b, currentColor.a);
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
        yield return new WaitForSeconds(1.5f);
        
        if(CurrentUIState != UIState.Event)
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