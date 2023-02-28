using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBehaviour : MonoBehaviour
{
    public GameObject[] ObjectsToHide;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            foreach (GameObject obj in ObjectsToHide)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
    }
}
