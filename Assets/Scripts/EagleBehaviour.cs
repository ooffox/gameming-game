using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleBehaviour : MonoBehaviour
{
    private Animator Animator;
	private GameObject player;
    private PlayerController playerScript;
    public float minimumDistance;
    private float _correctDir;
    private bool isHoming;
    private Vector2 direction;
    private float rotation;
    void Start()
    {
        Animator = GetComponent<Animator>();
        player = PlayerController.s_PlayerObj;
        playerScript = PlayerController.s_PlayerScript;
    }

    void Update()
    {
        if (playerScript.Dead)
        {
            this.enabled = false;
        }
        _correctDir = transform.position.x > player.transform.position.x ? 1.0f : -1.0f;
        isHoming = Vector3.Distance(transform.position, player.transform.position) < minimumDistance;
        direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (isHoming)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.025f);
            transform.eulerAngles = new Vector3(0, 0, rotation + 37.5f);
        }
        else
        {
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * _correctDir, transform.localScale.y, transform.localScale.z);
        }

        Animator.SetBool("chasing", Vector3.Distance(transform.position, player.transform.position) < minimumDistance);
    }

}
