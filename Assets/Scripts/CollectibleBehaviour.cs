using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollectibleBehaviour : MonoBehaviour
{
    private Vector3 startPos;
    private Animator Animator;
    private DialogueManager manager;
    void Awake()
    {
        startPos = transform.position;
    }

    void Start()
    {
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
            CollectibleManager.addCollected(startPos);
            Animator.SetBool("collected", true);
        }
    }

    void destroyCollectible()
    {
        Destroy(gameObject);
    }
}
