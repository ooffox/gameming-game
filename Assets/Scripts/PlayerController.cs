using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public bool Dead { get; private set; }
    public bool Grounded { get; private set; }
    public bool canFly;
    public float BoostForce;
    [System.NonSerialized]
    public float CorrectDir;
    public static bool s_InCutscene = false;
    public static GameObject s_PlayerObj;
    public static PlayerController s_PlayerScript;
    public AudioClip DeathSound;
    public AudioClip JumpSound;
    private float RunSpeed = 10.0f;
    private float JumpForce = 750.0f;

    private bool _isDoubleJumping;
    private bool _boosting;
    private float _coyoteTime;
    private float _coyoteJumpThreshold = 0.02f;
    private float _verticalClamp = -15.0f;
    private float _horizontal;
    private float _vertical;
    private float _fadeCount;
    private float _fadeSpeed = 0.02f;
    private float _lastJumped = 0.0f;
    private float _lastTryJump = 0.0f;
    private float _mayCoyoteJump;
    private RaycastHit2D _rayHit;
    private Vector2 _boostDir;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private GameObject _manager;
    private AudioSource _audioSource;
    private Collider2D _collider2D;
    private Image _fadeImage;
    private LayerMask _ownMask;

    #region Events

    void Start()
    {
        s_PlayerObj = gameObject;
        s_PlayerScript = this;
        StartFade();
        InitializeStartVariables();
    }


    void Update()
    {
        if (IsPaused() || IsFading() || CheckDead()) 
        {
            return;
        }

        ChangeDirection();

        UpdateMovementVariables();

        CheckLastTriedJump();

        CheckDoubleJump();

        CheckJump();

        InitializeAnimation();
    
    }

    
    void FixedUpdate()
    {
        if (_boosting)
        {
            ClampVerticalVelocity();
            return;
        }
        UpdateVelocity();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Leatherboots":
                PlayerStats.hasBoots = true;
                PlayerStats.gravity -= 1.0f;
                _rigidbody2D.gravityScale = PlayerStats.gravity;
                break;

            case "Spanner":
                PlayerStats.numberOfSpanners += 1;
                break;
        }
    }

    #endregion


    #region Variable Assignation

    private void InitializeAnimation()
    {
        _animator.SetBool("running", !Dead && Grounded && _horizontal != 0.0f);
        _animator.SetBool("rising", !Dead && !Grounded && _rigidbody2D.velocity.y > 0.0f);
        _animator.SetBool("falling", !Dead && !Grounded && _rigidbody2D.velocity.y < 0.0f);
        _animator.SetBool("crouching", !Dead && !s_InCutscene && Grounded && _horizontal == 0.0f && Input.GetKey(KeyCode.S));
        _animator.SetBool("dead", Dead);
    }

    private void InitializeStartVariables()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _manager = GameObject.FindWithTag("GameManager");
        _audioSource = _manager.GetComponent<AudioSource>();
        _rigidbody2D.gravityScale = PlayerStats.gravity;
        _ownMask = LayerMask.GetMask("Player");
        _mayCoyoteJump = _coyoteJumpThreshold;
    }

    private void UpdateMovementVariables()
    {
        Grounded = IsGrounded();
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        _coyoteTime -= 0.1f * Time.deltaTime;
        if (Grounded)
        {
            _coyoteTime = _coyoteJumpThreshold;
        }
    }

    #endregion


    #region Checks

    private void CheckBoost()
    {
        if (_boosting && Grounded)
        {
            _boosting = false;
        }
        if (CanBoost())
        {
            Boost(_horizontal, _vertical, BoostForce);
        }
    }

    private void CheckDoubleJump()
    {
        if (Grounded) { _isDoubleJumping = false; }
        if (CanJump() || _isDoubleJumping || !Input.GetKeyDown(KeyCode.W)) { return; }
        _isDoubleJumping = true;
        Jump();
    }

    private void CheckJump()
    {
        

        if (CanJump() || (canFly && Input.GetKeyDown(KeyCode.W)))
        {
            Jump();
        }
    }

    private void CheckLastTriedJump()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            _lastTryJump = Time.time;
        }

    }

    private bool CheckDead()
    {
        if (Dead || s_InCutscene)
        {
            _horizontal = 0.0f;
            InitializeAnimation();
            return true;
        }

        return false;
    }

    private bool IsPaused()
    {
        return Time.timeScale == 0.0f;
    }

    private bool IsFading()
    {
        return _fadeImage.color.a > 0.0f;
    }

    private bool IsGrounded()
    {
        _rayHit = Physics2D.Raycast(transform.position, Vector2.down, 2.035f, ~_ownMask);
        if (_rayHit)
        {
            if (_rayHit.collider.isTrigger || _rayHit.collider.gameObject.tag == "Deathzone") { return false; }
            else { return true; }
        }
        return false;
    }

    private bool CanBoost()
    {
        return Input.GetKeyDown(KeyCode.Space) && !_boosting && PlayerStats.hasBoots && (_horizontal != 0.0f || _vertical != 0.0f);
    }

    private bool CanJump()
    {
        return (Grounded || (_coyoteTime >= 0.0f && Time.time - _lastJumped > 0.5f)) && Time.time - _lastTryJump < 0.15f && Time.time - _lastJumped > 0.05f;
    }
    
    void CheckSpawnPos()
    {
        if (PlayerStats.s_SpawnPosition != Vector2.zero)
        {
            transform.position = PlayerStats.s_SpawnPosition;
        }
        s_InCutscene = false;
    }

    #endregion


    #region Player Actions

    public void Boost(float dirx, float diry, float force)
    {
        _audioSource.PlayOneShot(JumpSound);
        _boostDir = new Vector2(dirx, diry);
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(_boostDir * force * _rigidbody2D.mass);
        _boosting = true;
    }

    void Jump()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(Vector2.up * JumpForce * _rigidbody2D.mass);
        _lastJumped = Time.time;
    }

    public void Die()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(DeathSound);
        Dead = true;
        GameObject.FindObjectOfType<CameraBehaviour>().enabled = false;
        GameObject.FindObjectOfType<CinemachineBrain>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine(DeathBoost());
        foreach(EnemyBehaviour obj in GameObject.FindObjectsOfType<EnemyBehaviour>())
        {
            obj.playerDead();
        }
    }

    #endregion


    #region Physics

    void UpdateVelocity()
    {
        _rigidbody2D.velocity = new Vector2(RunSpeed * _horizontal, Mathf.Clamp(_rigidbody2D.velocity.y, _verticalClamp, Mathf.Infinity));
    }

    void ClampVerticalVelocity()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Mathf.Clamp(_rigidbody2D.velocity.y, _verticalClamp, Mathf.Infinity));
    }

    IEnumerator DeathBoost()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.isKinematic = true;
        yield return new WaitForSeconds(1.10f);
        _rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(Vector2.up * 1250 * _rigidbody2D.mass);
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(FadeIn(SceneManager.GetActiveScene().name, Vector2.zero));
    }

    

    #endregion


    #region UI

    void StartFade()
    {
        _fadeImage = GameObject.FindWithTag("FadeImage").GetComponent<Image>();
        _fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        CheckSpawnPos();
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn(string sName, Vector2 sPoint)
    {
        _fadeCount = 0.0f;
        while (_fadeCount < 1.0f)
        {
            _fadeCount += _fadeSpeed; 
            yield return new WaitForSeconds(0.01f);
            _fadeImage.color = new Color(0, 0, 0, Mathf.Clamp(_fadeCount, 0.0f, 1.0f)); // You have to clamp the value cause otherwise it goes negative lol
        }
        StartFade();
        if (sPoint != Vector2.zero) {
            PlayerStats.s_SpawnPosition = sPoint;
        }
        if (sName != "") 
        {
            SceneManager.LoadScene(sName);
        }
        
        
    }

    public IEnumerator FadeOut()
    {
        _fadeCount = 1.0f;
        while (_fadeCount > 0.0f)
        {
            _fadeCount -= _fadeSpeed;
            yield return new WaitForSeconds(0.01f);
            _fadeImage.color = new Color(0, 0, 0, Mathf.Clamp(_fadeCount, 0.0f, 1.0f)); // You have to clamp the value cause otherwise it goes negative lol
        }
    }

    #endregion

    
    #region Miscellaneous
    
    private float ChangeDirection()
    {
        CorrectDir = determineDirection();
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * CorrectDir, transform.localScale.y, transform.localScale.z);
        return CorrectDir;
    }

    private float determineDirection()
    {
        if (_horizontal > 0.0f)
        {
            return 1.0f;
        }
        else if (_horizontal < 0.0f)
        {
            return -1.0f;
        }
        return transform.localScale.x < 0.0f ? -1.0f : 1.0f;
    }

    #endregion
}
