using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogue;
    public AudioClip charMusic;
    private DialogueManager DialogueManager;
    void Awake()
    {
        DialogueManager = FindObjectOfType<DialogueManager>();
    }
    void Start()
    {
        if (CollectibleManager.getCollected().Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }
    private void triggerDialogue(Dialogue[] dialogue, bool manual = false)
    {
        DialogueManager.StartDialogue(dialogue, manual);
        DialogueManager.currentTrigger = gameObject;
        Destroy(GetComponent<Collider2D>());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            triggerDialogue(dialogue);
        }
        
    }

}
