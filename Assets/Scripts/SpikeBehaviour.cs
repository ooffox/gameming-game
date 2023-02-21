using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehaviour : MonoBehaviour
{
    public float vel;
    private Rigidbody2D Rigidbody2D;
    private GameObject player;
    private PlayerController playerScript;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        player = PlayerController.playerObj;
        playerScript = PlayerController.playerScript;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerScript.dead)
        {
            playerDead();
            return;
        }

        if (Rigidbody2D.velocity.y <= -1.0f)
        {
            Rigidbody2D.velocity = new Vector2(0.0f, Rigidbody2D.velocity.y);
        }

        else
        {
            Rigidbody2D.velocity = new Vector2(vel, Rigidbody2D.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.tag == "Player")
        {
            playerScript.die();
        }
    }
    
    void playerDead()
    {
        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.isKinematic = true;
        Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.enabled = false;
    }
}
