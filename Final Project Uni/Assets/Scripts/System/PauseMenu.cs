using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    //this will still work even time scale = 0
    void Update()
    {
        
    }
}
