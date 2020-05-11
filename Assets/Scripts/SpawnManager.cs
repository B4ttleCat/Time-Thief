using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Tilemap _arena;

    [SerializeField] private GameObject _enemyPrefab;

    [Header("Gameplay")]
    [SerializeField] private float _spawnInterval = 2f;

    private float _spawnTimer;
    private float _nextSpawn;
    private int _arenaWidth, _arenaHeight;
    private GameObject _enemyParent;

    void Start()
    {
        _spawnTimer = 0f;
        _nextSpawn = 0f;

        _arenaHeight = _arena.size.y;
        _arenaWidth = _arena.size.x;

        if (_enemyParent == null)
        {
            _enemyParent = new GameObject("Enemies");
        }
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
        Vector2 randomPos = GetRandomSpawnPos();
        GameObject newEnemy = Instantiate(_enemyPrefab, randomPos, quaternion.identity);
        newEnemy.transform.parent = _enemyParent.transform;
    }

    private Vector2 GetRandomSpawnPos()
    {
        float randomXPos = Random.Range((-_arenaWidth * 0.5f) + 2, (_arenaWidth * 0.5f) - 2);
        float randomYPos = Random.Range((-_arenaHeight * 0.5f) + 2, (_arenaHeight * 0.5f) - 2);
        Vector2 randomPos = new Vector2(randomXPos, randomYPos);
        return randomPos;
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