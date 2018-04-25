using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//debug commands for program control
public class DebugControl : MonoBehaviour
{
    public string sceneName;
    bool paused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))    //switch scenes
        {
            SceneManager.LoadScene(sceneName);
        }
        if (Input.GetKeyDown(KeyCode.Escape))   //quit the program
        {
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.P))         //pause time, makes checking RL values easier
        {
            if(paused)
            {
                paused = false;
                Time.timeScale = 1.0f;
            }
            else
            {
                paused = true;
                Time.timeScale = 0.0f;
            }
        }
    }
}
