using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
public class DialogueManager : MonoBehaviour
{
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

    void Awake()
    {
        if (AudioManager.manager && AudioManager.manager != gameObject) { return; }
        SceneManager.sceneLoaded += LoadStartVariables;
    }

    void LoadStartVariables(Scene scene, LoadSceneMode mode)
    {
        cameraBehaviour = GameObject.FindObjectOfType<CameraBehaviour>();
        AudioSource = GetComponent<AudioSource>();
        VCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (CanContinue())
        {
            LoadNextSentence();
            finishedLoading = false;
        }
        /*
        if (dialoguer)
        {
            VCam.m_Follow = dialoguer.transform;
        }
        */
    }

    private bool CanContinue()
    {
        return Input.GetKeyDown(KeyCode.Space) && currentTrigger != null && finishedLoading && !isTimed && !manual;
    }

    public void StartDialogue(Dialogue[] dialogue, bool m = false)
    {
        manual = m;
        PlayerController.s_InCutscene = true;
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
        LoadNextSentence();
    }

    public void LoadNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        newSentence = sentences.Dequeue();
        StartCoroutine(addText(newSentence.sentence));
        dialogueName.text = newSentence.name;
        dialoguer = newSentence.dialoguer;
        VCam.Follow = dialoguer.transform;
    }

    void EndDialogue()
    {
        // cameraBehaviour.enabled = true;
        finishedLoading = false;
        foreach (Transform child in dialogueUI.transform)
        {
            child.gameObject.SetActive(false);
        }
        PlayerController.s_InCutscene = false;
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
