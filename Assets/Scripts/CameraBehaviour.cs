using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float xMax;
    public float xMin;
    public float yMax;
    public float yMin;
    private Vector3 newPos;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.playerObj;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // newPos = new Vector3(Mathf.Clamp(player.transform.position.x, xMin, xMax), Mathf.Clamp(player.transform.position.y, yMin, yMax), transform.position.z); // Clamps the x and y positions along the set maximum in the public variables
        // transform.position = newPos;
    }

    public void MoveCamera(Vector3 pos, float speed, float waitTime)
    {
        StartCoroutine(movecam(pos, speed, waitTime));
    }

    IEnumerator movecam(Vector3 pos, float speed, float waitTime)
    {
        while (Vector3.Distance(transform.position, pos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.x, pos.y, transform.position.z), speed);
            yield return new WaitForSeconds(waitTime);
        }
    }

    
}
