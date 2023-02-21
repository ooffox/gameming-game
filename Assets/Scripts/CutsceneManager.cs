using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    void Start()
    {
        
    }
    public void startCutscene(string cutsceneName)
    {
        Invoke(cutsceneName, 0.0f);
    }
    private IEnumerator cutscene1(GameObject gameObj)
    {
        GameObject player = PlayerController.playerObj;
        PlayerController.inCutscene = true;
        dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        Dialogue[] dInfo = {
            new Dialogue("xd", "xddddddddddddddddd"),
            new Dialogue("bruh", "bruhhsfisfbashilfbals")
        };
        dialogueManager.startDialogue(dInfo);
        dialogueManager.currentTrigger = gameObj;
        yield return new WaitForSeconds(0.0f);
        PlayerController.inCutscene = false;
    }
}
