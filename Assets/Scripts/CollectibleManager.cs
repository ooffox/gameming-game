using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager
{
    private static List<Vector3> collected = new List<Vector3>();

    public static void addCollected(Vector3 pos)
    {
        collected.Add(pos);
    }

    public static List<Vector3> getCollected()
    {
        return collected;
    }
}
