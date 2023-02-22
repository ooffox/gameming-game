using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehaviour : MonoBehaviour
{
    public void Pause()
    {
        GameObject.FindObjectOfType<PauseManager>().Pause();
    }

    public void Exit()
    {
        Debug.Log("quitting");
        Application.Quit();
    }
}
