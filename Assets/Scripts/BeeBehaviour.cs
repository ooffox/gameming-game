using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBehaviour : MonoBehaviour
{
    private GameObject player;
    public float speed;
    public Vector3[] setDestinations = new Vector3[2];
	private Vector3 currentDestination;
    private PlayerController playerScript;
    private float _correctDir;

    void Start()
    {
        player = PlayerController.s_PlayerObj;
        playerScript = PlayerController.s_PlayerScript;
        currentDestination = setDestinations[0];
    }

    void Update()
    {
        if (playerScript.Dead)
        {
            this.enabled = false;
        }
        _correctDir = transform.position.x > player.transform.position.x ? 1.0f : -1.0f;
        if (playerScript.CorrectDir != 0.0f) { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * _correctDir, transform.localScale.y, transform.localScale.z); }
        transform.position = Vector3.MoveTowards(transform.position, currentDestination, speed);
        if (transform.position == currentDestination) {
            if (currentDestination == setDestinations[0]) { currentDestination = setDestinations[1]; }
            else { currentDestination = setDestinations[0]; }
        }
    }
}
