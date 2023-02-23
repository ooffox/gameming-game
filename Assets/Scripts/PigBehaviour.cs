using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBehaviour : MonoBehaviour
{
    private float lastDirected;
    private float rayDistance = 5.0f;
    public float pushDistance;
    public float pushForce;
    public float leftBoundary;
    public float rightBoundary;
    public float runSpeed;
    private float direction = 1;
    private GameObject player;
    private PlayerController _playerScript;
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.s_PlayerObj;
        _playerScript = PlayerController.s_PlayerScript;
        lastDirected = Time.time;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.s_PlayerScript.Dead)
        {
            this.enabled = false;
        }
        if (leftBoundary == Mathf.Infinity || rightBoundary == Mathf.Infinity)
        {
            if (!(Physics2D.Raycast(transform.position, new Vector2(0.0f, -1.0f), rayDistance) || Time.time - lastDirected < 0.2f))
            {
                direction = -direction;
                lastDirected = Time.time;
            }
        }
        else
        {
            if (transform.position.x <= leftBoundary)
            {
                direction = 1.0f;
            }
            else if (transform.position.x >= rightBoundary)
            {
                direction = -1.0f;
            }
        }
        
        
        if (Rigidbody2D.velocity.x > 0.0f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        
    }

    void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(runSpeed * direction, Rigidbody2D.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject collided = col.collider.gameObject;
        if (collided.tag == "Player" && Mathf.Abs(transform.position.y - collided.transform.position.y) < pushDistance)
        {
            float dir;
            collided.transform.position += new Vector3(0.0f, 0.25f, 0.0f);
            if (transform.position.x > collided.transform.position.x)
            {
                dir = -1.0f;
            }
            else { dir = 1.0f; }
            _playerScript.Boost(dir, 1.0f, pushForce);
        }
        else if (collided.tag != "Player")
        {
            direction = -direction;
        }
    }
}
