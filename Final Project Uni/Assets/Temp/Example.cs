using DG.Tweening;
using UnityEngine;

public class Example : MonoBehaviour
{
    public Tween myTween;

    void Start()
    {
        // Animation scale from small (0) to large (1) in 2 seconds
        myTween = transform.DOScale(1, 2).SetAutoKill(false).Pause();
    }

    void OnEnable()
    {
        // Animate scale down from current scale to 0 in 1 second
        transform.DOScale(0, 1).OnComplete(() =>
        {
            // Rewind to reset the tween
            myTween.Rewind();
            // Play the scale-up animation
            myTween.Play();
        });
    }

    void OnDisable()
    {
        // Pause when the object is disabled
        myTween.Pause();
    }
}