using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool paused = false;
    private GameObject pauseUI;
    private GameObject resumeUI;
    private GameObject exitButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            paused = !paused;
        }
    }

    
    void Pause()
    {
        pauseUI = GameObject.FindWithTag("PauseUI");
        if (paused)
        {
            Time.timeScale = 1.0f;
            foreach(Transform transform in pauseUI.transform)
            {
                transform.gameObject.SetActive(false);
            }
            initVariables();
        }
        if (!paused)
        {
            Time.timeScale = 0.0f;
            foreach(Transform transform in pauseUI.transform)
            {
                transform.gameObject.SetActive(true);
            }
        }
    }

    void initVariables()
    {
        resumeUI = GameObject.FindWithTag("ResumeUI");
    }
}
