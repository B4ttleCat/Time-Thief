using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("Gameplay")]
    [SerializeField] private float _spawnInterval = 2f;

    private float _spawnTimer;
    private float _nextSpawn;

    void Start()
    {
        _spawnTimer = 0f;
        _nextSpawn = 0f;
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
        GameObject newEnemy = Instantiate(_enemyPrefab, new Vector2(0, 0), Quaternion.identity);
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