using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : MonoBehaviour
{
    private RaycastHit2D groundedHit;
    private Rigidbody2D Rigidbody2D;
    private Collider2D Collider2D;
    private Animator Animator;
    private float xScale;
    private float lastJumped = 0.0f;
    private bool grounded;
    public float hopForce;
    public float leftBoundary;
    public float rightBoundary;
    private float jumpDir;
    private float groundedDir;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(3, 3);
        xScale = transform.localScale.x;
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.s_PlayerScript.Dead)
        {
            Rigidbody2D.velocity = Vector2.zero;
            Rigidbody2D.isKinematic = true;
            Animator.enabled = false;
            this.enabled = false;
        }
        
        grounded = isGrounded();
        jumpDir = changeDirection();
        if (grounded && Time.time - lastJumped > 3.0f && Vector3.Distance(transform.position, PlayerController.s_PlayerObj.transform.position) < 10.0f)
        {
            hop();
        }
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, leftBoundary, rightBoundary), transform.position.y);
        initAnim();
        
    }

    void FixedUpdate()
    {
        if (Rigidbody2D.velocity == Vector2.zero) { groundedDir = jumpDir; }
        else 
        {
            if (Rigidbody2D.velocity.x > 0.0f)
            {
                groundedDir = 1.0f;
            }
            else
            {
                groundedDir = -1.0f;
            }
        }
        if (Physics2D.Raycast(transform.position + new Vector3(1.0f, 0.0f, 0.0f) * groundedDir, new Vector2(0.0f, -5.0f), 10.0f)) { return; }
        Rigidbody2D.velocity = new Vector2(0.0f, Rigidbody2D.velocity.y);
    }

    private bool isGrounded()
    {
        groundedHit = Physics2D.Raycast(transform.position, Vector2.down, 1.35f);
        if (!groundedHit.collider) { return false; }
        if (Physics2D.GetIgnoreCollision(groundedHit.collider, Collider2D))
        {
            return false;
        }
        return true;
    }

    private void hop()
    {
        Rigidbody2D.AddForce(new Vector2(0.5f * jumpDir, 1.5f) * hopForce * Rigidbody2D.mass);
        lastJumped = Time.time;
    }

    private float changeDirection()
    {
        if (transform.position.x > PlayerController.s_PlayerObj.transform.position.x)
        {
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
            return -1.0f;
        }
        else if (transform.position.x < PlayerController.s_PlayerObj.transform.position.x)
        {
            transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
            return 1.0f;
        }
        return transform.localScale.x < 0.0f ? 1.0f : -1.0f;
    }

    private void initAnim()
    {
        Animator.SetBool("rising", Rigidbody2D.velocity.y > 0.0f && !grounded);
        Animator.SetBool("falling", Rigidbody2D.velocity.y < 0.0f && !grounded);
    }
    


}
