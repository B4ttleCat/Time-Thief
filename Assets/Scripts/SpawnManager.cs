using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("Gameplay")]
    [SerializeField] private float _spawnInterval = 2f;

    private float _spawnTimer;
    private float _nextSpawn;
    private Arena _arena;
    private GameObject _enemyParent;
    private bool _spawnDisabled;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerOutOfBounds += DisableSpawning;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerOutOfBounds -= DisableSpawning;
    }
    
    private void Awake()
    {
        _arena = FindObjectOfType<Arena>();
    }

    void Start()
    {
        _spawnTimer = 0f;
        _nextSpawn = 0f;
        
        if (_enemyParent == null)
        {
            _enemyParent = new GameObject("Enemies");
        }

    }

    void Update()
    {
        if (_spawnDisabled) return;
        
        if (IsAllowedToSpawn())
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
        float randomXPos = Random.Range((-_arena.Size.x * 0.5f) + 2, (_arena.Size.x * 0.5f) - 2);
        float randomYPos = Random.Range((-_arena.Size.y * 0.5f) + 2, (_arena.Size.y * 0.5f) - 2);
        Vector2 randomPos = new Vector2(randomXPos, randomYPos);
        return randomPos;
    }

    private bool IsAllowedToSpawn()
    {
        _spawnTimer = Time.timeSinceLevelLoad;

        if (_spawnTimer > _nextSpawn)
        {
            _nextSpawn += _spawnInterval;
            return true;
        }

        return false;
    }

    private void DisableSpawning(float delay)
    {
        StartCoroutine(DelayTimer(delay));
    }
    
    private IEnumerator DelayTimer(float delay)
    {
        _spawnDisabled = true;
        yield return new WaitForSecondsRealtime(delay);
        _spawnDisabled = false;
    }
}