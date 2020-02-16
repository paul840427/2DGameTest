using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo
{
    public static string id = "root";
    public static bool isRunning = true;
    public static int game_stage = 1;

    public static Dictionary<DetectSkeleton, int> movement_count;
    public static Dictionary<int, int> game_count;
}
