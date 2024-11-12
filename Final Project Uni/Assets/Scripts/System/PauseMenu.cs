using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    void PauseGame ()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    void ResumeGame ()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    //this will still work even time scale = 0
    void Update()
    {
        
    }
}
