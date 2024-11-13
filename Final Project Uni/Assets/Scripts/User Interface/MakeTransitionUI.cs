using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MakeTransitionUI : MonoBehaviour
{
    public GameObject maskA;
    public GameObject maskB;

    public void ConfigTwoGameObjectOfTransition(GameObject newChild, GameObject oldChild)
    {
        var tempA = maskA.transform.GetChild(0);
        var tempB = maskB.transform.GetChild(0);
        //clear parent
        // tempA.SetParent(null);
        // tempB.SetParent(null);
        // Ensure both new and old child have FixPosition component
        if (!newChild.GetComponent<FixPosition>())
        {
            newChild.AddComponent<FixPosition>();
        }

        if (!oldChild.GetComponent<FixPosition>())
        {
            oldChild.AddComponent<FixPosition>();
        }

        //false mean left to right
        //true mean right to left
        if (!toggle)
        {
            newChild.transform.SetParent(maskA.transform);
            oldChild.transform.SetParent(maskB.transform);
        }
        else
        {
            newChild.transform.SetParent(maskB.transform);
            oldChild.transform.SetParent(maskA.transform);
        }
    }


    public GameObject WipeSlider;
    public float transitionSpeed = 2000f;
    public float screenEdgeOffset = 100f; // Thêm offset để slider đi ra ngoài màn hình
    public float duration;
    private bool toggle = false;
    public bool isNormalTransition;
    public async void MakeTransition()
    {
        RectTransform wipeRect = WipeSlider.GetComponent<RectTransform>();
        float startX, targetX;
        float halfScreenWidth = (Screen.width + screenEdgeOffset) / 2f; // Half screen width with offset
       
        if (!toggle)
        {
            // Left to right
            startX = -halfScreenWidth;
            targetX = halfScreenWidth;
            toggle = true;
        }
        else
        {
            // Right to left
            startX = halfScreenWidth;
            targetX = -halfScreenWidth;
            toggle = false;
        }

        // Set initial position
        wipeRect.anchoredPosition = new Vector2(startX, wipeRect.anchoredPosition.y);

        float elapsedTime = 0f;
        duration = Mathf.Abs(targetX - startX) / transitionSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float currentX = Mathf.Lerp(startX, targetX, t);
            wipeRect.anchoredPosition = new Vector2(currentX, wipeRect.anchoredPosition.y);
            if (elapsedTime >= 0.7f * duration && isNormalTransition)
            {
                float fadeProgress = (elapsedTime - 0.7f * duration) / (0.3f * duration);
                fadeProgress = Mathf.Clamp01(fadeProgress);
                fadeBlackImage.GetComponent<CanvasGroup>().alpha = fadeProgress;
            }
            await UniTask.Yield();
        }

        wipeRect.anchoredPosition = new Vector2(targetX, wipeRect.anchoredPosition.y);
    }  
    public GameObject fadeBlackImage;

}