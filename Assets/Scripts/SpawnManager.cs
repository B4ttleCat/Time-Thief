using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Tilemap _arena;

    [Header("Gameplay")]
    [SerializeField] private float _spawnInterval = 2f;

    private float _spawnTimer;
    private float _nextSpawn;
    private int _arenaWidth, _arenaHeight;

    void Start()
    {
        _spawnTimer = 0f;
        _nextSpawn = 0f;

        _arenaHeight = _arena.size.y;
        _arenaWidth = _arena.size.x;
    }

    void Update()
    {
        if (CanSpawn())
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        float randomXPos = Random.Range((-_arenaWidth * 0.5f) + 1, (_arenaWidth * 0.5f) - 1);
        float randomYPos = Random.Range((-_arenaHeight * 0.5f) + 1, (_arenaHeight * 0.5f) - 1);
        Vector2 randomPos = new Vector2(randomXPos, randomYPos);
        
        GameObject newEnemy = Instantiate(_enemyPrefab, randomPos, Quaternion.identity);
    }

    private bool CanSpawn()
    {
        _spawnTimer = Time.time;

        if (_spawnTimer > _nextSpawn)
        {
            _nextSpawn += _spawnInterval;
            return true;
        }

        return false;
    }
}