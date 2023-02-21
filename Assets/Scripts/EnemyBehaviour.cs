using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public bool dangerous;
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.tag == "Player" && dangerous)
        {
            PlayerController.playerScript.die();
        }
    }

    public void playerDead()
    {
        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.isKinematic = true;
        Animator.enabled = false;
    }
}
