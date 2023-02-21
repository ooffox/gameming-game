using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehaviour : MonoBehaviour
{
    private Transform camPos;
    // Start is called before the first frame update
    void Start()
    {
        camPos = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(camPos.position.x, camPos.position.y, transform.position.z);
    }
}
