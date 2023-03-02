using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseManager : MonoBehaviour
{
    private static bool paused = false;
    private GameObject pauseUI;
    private GameObject pauseText;
    private GameObject resumeUI;
    private GameObject exitUI;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        initVariables();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    
    public void Pause()
    {
        if (!paused)
        {
            Time.timeScale = 0.0f;
            enableUI(true, pauseUI.transform);
        }
        else
        {
            Time.timeScale = 1.0f;
            enableUI(false, pauseUI.transform);
        }

        paused = !paused;
    }


    public static void enableUI(bool hide, Transform transformArr)
    {
        foreach(Transform t in transformArr)
        {
            t.gameObject.SetActive(hide);
        }
    }

    void initVariables()
    {
        pauseUI = GameObject.FindWithTag("PauseUI");
        pauseText = pauseUI.transform.GetChild(0).gameObject;
        resumeUI = pauseUI.transform.GetChild(1).gameObject;
        exitUI = pauseUI.transform.GetChild(2).gameObject;
    }
}
