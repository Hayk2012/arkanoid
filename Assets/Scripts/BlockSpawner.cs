using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameManager GameManager;

    [Header("Grid Spawner")]
    [SerializeField] private Vector2 _startSpawnGridPosition;
    [SerializeField] private float _rows;
    [SerializeField] private float _columns;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;
    [SerializeField] private Block _block;

    [Space(20), Header("Spiral Spawner")]
    [SerializeField] private int _numberOfSpiralBlocks;
    [SerializeField] private float _startRadius;
    [SerializeField, Range(1f, 360f)] private float _angleStep;
    [SerializeField, Range(0.1f, 10f)] private float _radiusStep;
    [SerializeField] private Vector2 _startSpawnSpiralPosition;

    [Space(20), Header("Circle Spawner")]
    [SerializeField] private int _numberOfCircleBlocks;
    [SerializeField] private float _radiusCircle;
    [SerializeField] private Vector2 _startSpawnCirclePosition1;
    [SerializeField] private Vector2 _startSpawnCirclePosition2;
    [SerializeField] private Vector2 _startSpawnCirclePosition3;

    [Space(20), Header("Square Spawner")]
    [SerializeField] private int _numberOfSquareBlocks;
    [SerializeField] private float _sideLength;
    [SerializeField] private Vector2 _startSpawnSquarePosition1;
    [SerializeField] private Vector2 _startSpawnSquarePosition2;
    [SerializeField] private Vector2 _startSpawnSquarePosition3;

    private List<Block> _blocksCount = new List<Block>();

    public void Behaviour1()
    {
        for (int column = 0; column < _columns; column++)
        {
            for (int row = 0; row < _rows; row++)
            {
                Vector2 spawnPosition = _startSpawnGridPosition + new Vector2(column * _offsetX, row * _offsetY);
                var block = Instantiate(_block, spawnPosition, Quaternion.identity);
                _blocksCount.Add(block);
                GameManager.SetBlocks(_blocksCount);
            }
        }
    }

    public void Behaviour2()
    {
        float currentAngle = 0;
        float currentRadius = _startRadius;
        for (int i = 0; i < _numberOfSpiralBlocks; i++)
        {
            float angleRad = currentAngle * Mathf.Deg2Rad;
            float xPosition = currentRadius * Mathf.Cos(angleRad);
            float yPosition = currentRadius * Mathf.Sin(angleRad);
            Vector2 spawnPosition = _startSpawnSpiralPosition + new Vector2(xPosition, yPosition);
            var block = Instantiate(_block, spawnPosition, Quaternion.identity);
            _blocksCount.Add(block);
            GameManager.SetBlocks(_blocksCount);
            currentAngle += _angleStep;
            currentRadius += _radiusStep;
        }
    }

    public void Behaviour3()
    {
        void Spawn(Vector3 position)
        {
            for (int i = 0; i < _numberOfCircleBlocks; i++)
            {
                float angle = 360 / _numberOfCircleBlocks;
                float angleRad = i * angle * Mathf.Deg2Rad;
                float x = _radiusCircle * Mathf.Cos(angleRad);
                float y = _radiusCircle * Mathf.Sin(angleRad);
                Vector3 spawnPosition = position + new Vector3(x, y);
                var block = Instantiate(_block, spawnPosition, Quaternion.identity);
                _blocksCount.Add(block);
                GameManager.SetBlocks(_blocksCount);
            }
        }

        Spawn(_startSpawnCirclePosition1);
        Spawn(_startSpawnCirclePosition2);
        Spawn(_startSpawnCirclePosition3);
    }

    public void Behaviour4()
    {
        void SpawnPosition(Vector2 positionSpawn)
        {
            float spacing = _sideLength / (_numberOfSquareBlocks - 1);
            Vector2 startPosition = positionSpawn - new Vector2(_sideLength / 2, _sideLength / 2);
            for (int i = 0; i < _numberOfSquareBlocks; i++)
            {
                Vector2 position = startPosition + new Vector2(i * spacing, 0);
                var block = Instantiate(_block, position, Quaternion.identity);
                _blocksCount.Add(block);
                GameManager.SetBlocks(_blocksCount);
            }
            for (int i = 1; i < _numberOfSquareBlocks - 1; i++)
            {
                Vector2 position = startPosition + new Vector2(_sideLength, i * spacing);
                var block = Instantiate(_block, position, Quaternion.identity);
                _blocksCount.Add(block);
                GameManager.SetBlocks(_blocksCount);
            }
            for (int i = 0; i < _numberOfSquareBlocks; i++)
            {
                Vector2 position = startPosition + new Vector2(_sideLength - i * spacing, _sideLength);
                var block = Instantiate(_block, position, Quaternion.identity);
                _blocksCount.Add(block);
                GameManager.SetBlocks(_blocksCount);
            }
            for (int i = 1; i < _numberOfSquareBlocks - 1; i++)
            {
                Vector2 position = startPosition + new Vector2(0, _sideLength - i * spacing);
                var block = Instantiate(_block, position, Quaternion.identity);
                _blocksCount.Add(block);
                GameManager.SetBlocks(_blocksCount);
            }
        }

        SpawnPosition(_startSpawnSquarePosition1);
        SpawnPosition(_startSpawnSquarePosition2);
        SpawnPosition(_startSpawnSquarePosition3);

    }
}
