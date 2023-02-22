using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public static GameObject playerObj;
    public static PlayerController playerScript;
    public static Vector2 spawnPosition = Vector2.zero;
    private static int lives = 3;
    public static bool inCutscene = false;
    public float runSpeed;
    public float jumpForce;
    public bool grounded;
    private float horizontal;
    private float vertical;
    public float correctDir;
    private float fadeCount;
    private float fadeSpeed = 0.02f;
    public float boostForce;
    public bool canFly;
    private bool boosting;
    public bool dead;
    private float lastJumped = 0.0f;
    private float lastTryJump = 0.0f;
    private bool jumped;
    private RaycastHit2D rayHit;
    private Vector2 boostDir;
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private GameObject Manager;
    private AudioSource AudioSource;
    private Collider2D Collider2D;
    public AudioClip deathSound;
    public AudioClip jumpSound;
    private Image fadeImage;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = gameObject;
        playerScript = this;
        fadeImage = GameObject.FindWithTag("FadeImage").GetComponent<Image>();
        fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        StartCoroutine(FadeOut());
        if (spawnPosition != Vector2.zero)
        {
            transform.position = spawnPosition;
            inCutscene = false;
        }
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Collider2D = GetComponent<Collider2D>();
        Manager = GameObject.FindWithTag("GameManager");
        AudioSource = Manager.GetComponent<AudioSource>();
        Rigidbody2D.gravityScale = PlayerStats.gravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0.0f) { return; }
        changeDirection();
        if (fadeImage.color.a > 0.0f)
        {
            return;
        }
        if (dead || inCutscene)
        {
            horizontal = 0.0f;
            initAnim();
            return;
        }
        rayHit = Physics2D.Raycast(transform.position, Vector2.down, 2.035f);
        grounded = isGrounded();
        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            lastTryJump = Time.time;
        }
        if ((canJump() || canFly))
        {
            Rigidbody2D.velocity = Vector2.zero;
            Rigidbody2D.AddForce(Vector2.up * jumpForce * Rigidbody2D.mass);
            lastJumped = Time.time;
        }
        if (canBoost())
        {
            vertical = Input.GetAxisRaw("Vertical");
            if (horizontal == 0.0f && vertical == 0.0f)
            {
                return;
            }
            boost(horizontal, vertical, boostForce);
        }
        
        if (boosting && grounded)
        {
            boosting = false;
        }
        
        initAnim();
    }
    void FixedUpdate()
    {
        if (boosting)
        {
            return;
        }
        Rigidbody2D.velocity = new Vector2(runSpeed * horizontal, Mathf.Clamp(Rigidbody2D.velocity.y, -20.0f, Mathf.Infinity));
    }

    private bool canBoost()
    {
        return Input.GetKeyDown(KeyCode.Space) && !boosting && PlayerStats.hasBoots;
    }

    public void boost(float dirx, float diry, float force)
    {
        AudioSource.PlayOneShot(jumpSound);
        boostDir = new Vector2(dirx, diry);
        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.AddForce(boostDir * force * Rigidbody2D.mass);
        boosting = true;
    }

    private float changeDirection()
    {
        correctDir = determineDirection();
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * correctDir, transform.localScale.y, transform.localScale.z);
        return correctDir;
    }

    private bool isGrounded()
    {
        if (rayHit)
        {
            if (rayHit.collider.isTrigger) { return false; }
            else { return true; }
        }
        return false;
    }

    private bool canJump()
    {
        Debug.Log(Time.time - lastJumped);
        return grounded && Time.time - lastTryJump < 0.2f && Time.time - lastJumped > 0.1f;
    }


    private float determineDirection()
    {
        if (horizontal > 0.0f)
        {
            return 1.0f;
        }
        else if (horizontal < 0.0f)
        {
            return -1.0f;
        }
        return transform.localScale.x < 0.0f ? -1.0f : 1.0f;
    }

    public void die()
    {
        AudioSource.Stop();
        AudioSource.PlayOneShot(deathSound);
        dead = true;
        Rigidbody2D.velocity = Vector2.zero;
        GameObject.FindObjectOfType<CameraBehaviour>().enabled = false;
        GameObject.FindObjectOfType<CinemachineBrain>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine(deathBoost());
        foreach(EnemyBehaviour obj in GameObject.FindObjectsOfType<EnemyBehaviour>())
        {
            obj.playerDead();
        }
    }

    IEnumerator deathBoost()
    {
        Animator.SetBool("dead", true);
        Rigidbody2D.isKinematic = true;
        yield return new WaitForSeconds(1.10f);
        Rigidbody2D.isKinematic = false;
        Rigidbody2D.AddForce(Vector2.up * 1250 * Rigidbody2D.mass);
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(FadeIn(SceneManager.GetActiveScene().name));
    }

    private void initAnim()
    {
        Animator.SetBool("running", !dead && grounded && horizontal != 0.0f);
        Animator.SetBool("rising", !dead && !grounded && Rigidbody2D.velocity.y > 0.0f);
        Animator.SetBool("falling", !dead && !grounded && Rigidbody2D.velocity.y < 0.0f);
        Animator.SetBool("crouching", !dead && !inCutscene && grounded && horizontal == 0.0f && Input.GetKey(KeyCode.S));
        Animator.SetBool("dead", dead);
        Animator.SetBool("stabbing", Input.GetKey(KeyCode.E));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Deathzone":
            case "Spike":
                die();
                break;
            case "Leatherboots":
                PlayerStats.hasBoots = true;
                PlayerStats.gravity -= 1.0f;
                Rigidbody2D.gravityScale = PlayerStats.gravity;
                break;

            case "Spanner":
                PlayerStats.numberOfSpanners += 1;
                break;

            
        }
    }

    public IEnumerator FadeOut()
    {
        fadeCount = 1.0f; //initial alpha value
        while (fadeCount > 0.0f)
        {
            fadeCount -= fadeSpeed; //lower alpha value 0.01 per 0.01 second 
            yield return new WaitForSeconds(0.01f); //per 0.01 second
            fadeImage.color = new Color(0, 0, 0, fadeCount); //makes image look opaque
        }
    }
    
    public IEnumerator FadeIn(string sName)
    {
        fadeCount = 0.0f; //initial alpha value

        while (fadeCount < 1.0f)
        {
            fadeCount += fadeSpeed; //lower alpha value 0.01 per 0.01 second 
            yield return new WaitForSeconds(0.01f); //per 0.01 second
            fadeImage.color = new Color(0, 0, 0, fadeCount); //makes image look transparent  
        }
        SceneManager.LoadScene(sName);
    }

}
