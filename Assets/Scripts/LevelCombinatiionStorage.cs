using System.Collections.Generic;
using UnityEngine;

public static class LevelCombinationStorage
{
    public static Dictionary<int, LevelCombination> Combinations = new Dictionary<int, LevelCombination>();
}


public struct LevelCombination
{
    public int BehaviourIndex;
    public int SpawnIndex;

    public LevelCombination(int spawn, int behaviour)
    {
        BehaviourIndex = behaviour;
        SpawnIndex = spawn;
    }
}