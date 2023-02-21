using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CutsceneTrigger : MonoBehaviour
{
    private CutsceneManager cutMan;
    private GameObject player;
    public string cutsceneName;
    // Start is called before the first frame update
    void Start()
    {
        cutMan = GameObject.FindWithTag("GameManager").GetComponent<CutsceneManager>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            cutMan.StartCoroutine(cutsceneName, gameObject);
        }
    }
}
