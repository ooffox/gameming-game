using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    private const int _numberOfSpanners = 0;
    private const float _gravity = 4.0f;
    private const bool _hasBoots = true;
    private const bool _hasGoldenBoots = false;
    private static Vector2 c_SpawnPosition;

    public static int numberOfSpanners = _numberOfSpanners;
    public static float gravity = _gravity;
    public static bool hasBoots = _hasBoots;
    public static bool hasGoldenBoots = _hasGoldenBoots;
    public static Vector2 s_SpawnPosition = c_SpawnPosition;

    public static void resetStats()
    {
        numberOfSpanners = _numberOfSpanners;
        gravity = _gravity;
        hasBoots = _hasBoots;
        hasGoldenBoots = _hasGoldenBoots;
        s_SpawnPosition = c_SpawnPosition;
    }
}
