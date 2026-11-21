using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Block _block;
    [SerializeField] private BlockSpawner _blockSpawner;

    private List<(int behaviourIndex, int spawnIndex)> _usedCombinations = new List<(int, int)>();
    private delegate void _blockBehaviour();
    private delegate void _spawnShape();

    private void Awake()
    {
        _block.gameObject.SetActive(false);
        
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SetupLevel(levelIndex);
        ChangeBackground();
    }

    private void SetupLevel(int levelIndex)
    {       
        _blockBehaviour[] behaviours = new _blockBehaviour[]
        {
            _block.Behaviour1,
            _block.Behaviour2,
            _block.Behaviour3,
            _block.Behaviour4
        };
        _spawnShape[] spawns = new _spawnShape[]
        {
            _blockSpawner.Behaviour1,
            _blockSpawner.Behaviour2,
            _blockSpawner.Behaviour3,
            _blockSpawner.Behaviour4
        };

        LevelCombination levelCombination;        

        if (LevelCombinationStorage.Combinations.ContainsKey(levelIndex))
        {
            levelCombination = LevelCombinationStorage.Combinations[levelIndex];
        }
        else
        {
            levelCombination = GenerateUniqueCombination();
            LevelCombinationStorage.Combinations.Add(levelIndex, levelCombination);
        }

        spawns[levelCombination.SpawnIndex]();
    }

    private LevelCombination GenerateUniqueCombination()
    {
        int maxCombinations = 4 * 4;
        if (LevelCombinationStorage.Combinations.Count >= maxCombinations)
        {
            return new LevelCombination(0, 0);
        }
        int spawn, behaviour;
        do
        {
            spawn = Random.Range(0, 4);
            behaviour = Random.Range(0, 4);
        }
        while (IsCombinationUsed(spawn, behaviour));
        return new LevelCombination(spawn, behaviour);
    }

    private bool IsCombinationUsed(int spawn, int behaviour)
    {
        foreach (var combo in LevelCombinationStorage.Combinations.Values)
        {
            if (combo.SpawnIndex == spawn && combo.BehaviourIndex == behaviour)
            {
                return true;
            }
        }
        return false;
    }    

    private void ChangeBackground()
    {
        float min = 50f / 255f;
        float max = 150f / 255f;
        Color backgroundColor = new Color(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        Camera.main.backgroundColor = backgroundColor;
    }
}
