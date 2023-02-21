using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DoorBehaviour : MonoBehaviour
{
    public Vector3 spawnPoint;
    public string sceneName;
    private GameObject player;
    private Image fadeImage;
    private bool enterDoor;
    private float xDiff;
    private float yDiff;
    private float fadeCount;
    private float fadeSpeed = 0.02f;
    private PlayerController controller;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<PlayerController>();
        
        fadeImage = GameObject.FindWithTag("FadeImage").GetComponent<Image>();
    }

    void Update()
    {
        if (enterDoor) { return; }
        xDiff = Mathf.Abs(player.transform.position.x - transform.position.x);
        yDiff = Mathf.Abs(player.transform.position.y - transform.position.y);
        if (Input.GetKeyDown(KeyCode.W) && xDiff <= 0.75f && yDiff <= 0.75f && controller.grounded && !enterDoor)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            PlayerController.inCutscene = true;
            PlayerController.spawnPosition = spawnPoint;
            enterDoor = true;
            StartCoroutine(PlayerController.playerScript.FadeIn(sceneName));
        }
    }

    public void loadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

}
