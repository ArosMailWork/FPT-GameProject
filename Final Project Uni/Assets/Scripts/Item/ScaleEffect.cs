using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    [SerializeField]
    private float scaleDuration = 0.3f;

    [SerializeField]
    private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public event Action OnScaleDownComplete;

    public async UniTask ScaleUpAsync()
    {
        await ScaleAsync(0, 1);
    }

    public async UniTask ScaleDownAsync()
    {
        await ScaleAsync(1, 0);
        OnScaleDownComplete?.Invoke();
    }

    private async UniTask ScaleAsync(float startScale, float endScale)
    {
        float elapsedTime = 0f;
        Vector3 startScaleVector = Vector3.one * startScale;
        Vector3 endScaleVector = Vector3.one * endScale;

        while (elapsedTime < scaleDuration)
        {
            float t = elapsedTime / scaleDuration;
            float curveValue = scaleCurve.Evaluate(t);
            transform.localScale = Vector3.Lerp(startScaleVector, endScaleVector, curveValue);

            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        transform.localScale = endScaleVector;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }
}
