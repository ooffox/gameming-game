using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoBehaviour : MonoBehaviour
{
    public float distance;
    private float lastJumped;
    private float correctDir;
    public Vector2 force;
    private GameObject playerObj;
    private PlayerController playerScript;
    private Animator Animator;
    private Collider2D Collider2D;
    private Rigidbody2D Rigidbody2D;
    private RaycastHit2D rayHit;
    private LayerMask playerMask;
    private bool limit;
    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = PlayerController.s_PlayerScript;
        playerObj = PlayerController.s_PlayerObj;
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        lastJumped = Time.time;
        playerMask = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        correctDir = transform.position.x > playerObj.transform.position.x ? 1.0f : -1.0f;
        rayHit = Physics2D.Raycast(transform.position, Vector2.down, 0.9f);
        grounded = (bool) rayHit;
        limit = canAdvance();

        initAnim();
        
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * correctDir, transform.localScale.y, transform.localScale.z);
        if (Vector2.Distance(transform.position, playerObj.transform.position) < distance && Time.time - lastJumped > 2.5f && !limit)
        {
            Animator.Play("DinoLunge");
            Rigidbody2D.AddForce(new Vector2(force.x * -correctDir, force.y) * Rigidbody2D.mass);
            lastJumped = Time.time;
        }
        
    }
    void FixedUpdate()
    {
        if (limit || grounded)
        {
            Rigidbody2D.velocity = new Vector2(0.0f, Rigidbody2D.velocity.y);
        }
    }
    void initAnim()
    {
        Animator.SetBool("falling", Rigidbody2D.velocity.y < 0.0f && !grounded);
    }

    private bool canAdvance()
    {
        RaycastHit2D limitHit = Physics2D.Raycast(transform.position + new Vector3(1.0f, 0.0f, 0.0f) * -correctDir, Vector2.down, 10.0f, playerMask);
        Debug.DrawRay(transform.position + new Vector3(1.0f, 0.0f, 0.0f) * -correctDir, Vector2.down * 10.0f);
        if (limitHit)
        {
            if (Physics2D.GetIgnoreCollision(Collider2D, limitHit.collider) || limitHit.collider.isTrigger) { return true; }
            else { return false; }
        }
        return true;
    }
}
