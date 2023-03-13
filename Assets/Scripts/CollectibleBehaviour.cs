using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollectibleBehaviour : MonoBehaviour
{
    private Vector3 startPos;
    private Animator Animator;
    private DialogueManager manager;
    public AudioClip ObtainSound;
    private AudioSource _audioSource;
    void Awake()
    {
        startPos = transform.position;
    }

    void Start()
    {
        _audioSource = GameObject.FindWithTag("GameManager").GetComponent<AudioSource>();
        if (CollectibleManager.getCollected().Contains(startPos))
        {
            Destroy(gameObject);
        }
        Animator = GetComponent<Animator>();
        manager = GameObject.FindWithTag("GameManager").GetComponent<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GetComponent<Collider2D>().enabled = false;
            CollectibleManager.addCollected(startPos);
            Animator.Play("ItemCollected");
            if (ObtainSound)
            {
                _audioSource.PlayOneShot(ObtainSound);
            }
        }
    }

    void destroyCollectible()
    {
        gameObject.SetActive(false);
    }
}
