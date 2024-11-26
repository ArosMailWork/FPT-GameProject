using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class SceneLoader : MonoBehaviour
{
    public GameObject LoaderUI;
    public Slider progressSlider;
    public UnityEvent CompletedLoad;
 
    public void LoadScene(int index)
    {
        StartCoroutine(LoadScene_Coroutine(index));
    }
 
    public IEnumerator LoadScene_Coroutine(int index)
    {
        progressSlider.value = 0;
        LoaderUI.SetActive(true);
 
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;
 
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progress;
            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                yield return new WaitForSeconds(0.75f);
                asyncOperation.allowSceneActivation = true;
                CompletedLoad?.Invoke(); 
            }
            yield return null;
        }
    }
}