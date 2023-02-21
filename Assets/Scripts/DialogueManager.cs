using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{
    public float moveCameraSpeed;
    private bool manual;
    private bool finishedLoading = false;
    private bool isTimed = false;
    private string loadedText;
    private CameraBehaviour cameraBehaviour;
    private PlayerController controller;
    private Queue<Dialogue> sentences = new Queue<Dialogue>();
    private Dialogue newSentence;
    private TMP_Text dialogueText;
    private TMP_Text dialogueName;
    private Image dialogueBox;
    private GameObject dialogueUI;
    private GameObject dialoguer;
    private CinemachineVirtualCamera VCam;
    private AudioSource AudioSource;
    public AudioClip dialogueSound;
    [System.NonSerialized]
    public GameObject currentTrigger;

    void Start()
    {
        cameraBehaviour = GameObject.FindObjectOfType<CameraBehaviour>();
        AudioSource = GetComponent<AudioSource>();
        VCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentTrigger != null && finishedLoading && !isTimed && !manual)
        {
            loadNextSentence();
            finishedLoading = false;
        }
        /*
        if (dialoguer)
        {
            VCam.m_Follow = dialoguer.transform;
        }
        */
    }

    public void startDialogue(Dialogue[] dialogue, bool m = false)
    {
        manual = m;
        PlayerController.inCutscene = true;
        dialogueUI = GameObject.FindWithTag("DialogueUI");
        foreach (Transform child in dialogueUI.transform)
        {
            child.gameObject.SetActive(true);
        }
        initVariables();
        sentences.Clear();
        foreach(Dialogue dial in dialogue)
        {
            sentences.Enqueue(dial);
        }
        loadedText = "";
        loadNextSentence();
    }

    public void loadNextSentence()
    {
        if (sentences.Count == 0)
        {
            endDialogue();
            return;
        }
        newSentence = sentences.Dequeue();
        StartCoroutine(addText(newSentence.sentence));
        dialogueName.text = newSentence.name;
        dialoguer = newSentence.dialoguer;
        VCam.Follow = dialoguer.transform;
        if (dialoguer)
        {
            // cameraBehaviour.enabled = false;
        }
    }

    void endDialogue()
    {
        // cameraBehaviour.enabled = true;
        finishedLoading = false;
        foreach (Transform child in dialogueUI.transform)
        {
            child.gameObject.SetActive(false);
        }
        PlayerController.inCutscene = false;
        dialoguer = null;
        VCam.Follow = controller.gameObject.transform;
        if (currentTrigger != null)
        {
            CollectibleManager.addCollected(currentTrigger.transform.position);
            isTimed = false;
            Destroy(currentTrigger);
        }
    }

    void initVariables()
    {
        controller = FindObjectOfType<PlayerController>();
        dialogueText = GameObject.FindWithTag("DialogueText").GetComponent<TMP_Text>();
        dialogueName = GameObject.FindWithTag("DialogueName").GetComponent<TMP_Text>();
        dialogueBox = GameObject.FindWithTag("DialogueBox").GetComponent<Image>();
        
        controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    IEnumerator addText(string textLoad)
    {
        for(int i = 0; i < textLoad.Length; i++)
        {
            loadedText += textLoad[i];
            dialogueText.text = loadedText;
            AudioSource.PlayOneShot(dialogueSound, 0.5f);
            yield return new WaitForSeconds(0.03f);
        }
        loadedText = "";
        finishedLoading = true;
    }
}
