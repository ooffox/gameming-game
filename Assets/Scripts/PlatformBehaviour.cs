using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerScript;
    // private Collider2D playerCollider;
    private Collider2D ownCollider;
    private Rigidbody2D playerRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.playerObj;
        playerScript = PlayerController.playerScript;
        // playerCollider = player.GetComponent<Collider2D>();
        ownCollider = GetComponent<Collider2D>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRigidbody.velocity.y < -0.1f && !Input.GetKey(KeyCode.S))
        {
            ownCollider.enabled = true;
        }
        else
        {
            ownCollider.enabled = false;
        }
    }
}
