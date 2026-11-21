using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<int> OnBlocksCountUpdated;
    public static event Action OnBlocksCountEnded;
    public static event Action<int> OnAttemptsCountUpdated;
    public static event Action OnAttemptsEnded;
    public static event Action<int> OnLevelLoaded;

    private List<Block> _blocks = new List<Block>();
    private int _attemptsCount = 5;

    private void Start()
    {
        OnAttemptsCountUpdated?.Invoke(_attemptsCount);

        Ball.OnBallFalledDown += HandleAttemptsCount;

        StartCoroutine(CheckBlocksCount());
        OnBlocksCountUpdated?.Invoke(_blocks.Count);

        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        OnLevelLoaded?.Invoke(levelIndex);
    }

    private IEnumerator CheckBlocksCount()
    {
        while(_blocks.Count > 0)
        {
            if (PanelsManager.LevelEnded) StopAllCoroutines();

            for (int i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i] == null)
                {                    
                    _blocks.RemoveAt(i);
                    OnBlocksCountUpdated?.Invoke(_blocks.Count);
                }
            }
            yield return null;
        }

        if (_blocks.Count == 0)
        {
            LevelCompleteHandler.CompleteLevel();
            OnBlocksCountEnded?.Invoke();
        }
    }    

    public void SetBlocks(List<Block> blocks)
    {
        _blocks = blocks;
        OnBlocksCountUpdated?.Invoke(_blocks.Count);
    }

    private void HandleAttemptsCount()
    {
        _attemptsCount--;
        OnAttemptsCountUpdated?.Invoke(_attemptsCount);
        if(_attemptsCount == 0)
        {
            OnAttemptsEnded?.Invoke();
        }
    }

    private void OnDestroy()
    {
        Ball.OnBallFalledDown -= HandleAttemptsCount;
    }
}









/*
ref, in, out - это модификаторы параметров метода, которые определяют как передаются аргументы в методы^ по ссылке или по значению.

ref - позволяет методу как читать, так и записывать переданный аргумент, то есть иметь заданное значение.
out - указывает, что метод должен задать значение параметру. Должен быть инициализирован перед вызовом, то есть иметь заданное значение.
in - позволяет методу только читать переданный по ссылке аргумент, запрещая его изменение. 


 */
