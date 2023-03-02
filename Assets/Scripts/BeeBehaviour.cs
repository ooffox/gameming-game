using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBehaviour : MonoBehaviour
{
    public float speed;
    public Vector3[] setDestinations = new Vector3[2];
	private Vector3 currentDestination;
    private PlayerController playerScript;

    void Start()
    {
        playerScript = PlayerController.s_PlayerScript;
        currentDestination = setDestinations[0];
    }

    void Update()
    {
        if (playerScript.Dead)
        {
            this.enabled = false;
        }
        transform.position = Vector3.MoveTowards(transform.position, currentDestination, speed);
        if (transform.position == currentDestination) {
            if (currentDestination == setDestinations[0]) { currentDestination = setDestinations[1]; }
            else { currentDestination = setDestinations[0]; }
        }
    }
}
