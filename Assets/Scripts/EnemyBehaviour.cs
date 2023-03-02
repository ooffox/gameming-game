using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public bool dangerous;
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private AudioManager AudioManager;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        AudioManager = GameObject.FindWithTag("GameManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.tag == "Player" && dangerous)
        {
            PlayerController.s_PlayerScript.Die();
            AudioManager.StopStageMusic();
        }
    }

    public void playerDead()
    {
        if (Rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
        {
            Rigidbody2D.velocity = Vector2.zero;
            Rigidbody2D.isKinematic = true;
            Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        if (Animator)
        {
            Animator.enabled = false;
        }
        
    }
}
