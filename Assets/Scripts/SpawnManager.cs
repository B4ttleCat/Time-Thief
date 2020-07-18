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
    private Vector2 _arenaSize;

    // spawn checking variables
    private const int MaxSpawnTries = 20;
    private Vector2 _min = new Vector2(); // I think this is for the OverlapArea
    private Vector2 _max = new Vector2(); // I think this is for the OverlapArea
    private float _enemyColliderHalfWidth;
    private float _enemyColliderHalfHeight;

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
        _arenaSize = new Vector2(_arena.Size.x * 0.5f, _arena.Size.y * 0.5f);

        if (_enemyParent == null)
        {
            _enemyParent = new GameObject("Enemies");

            // spawn temp enemy to get collider values
            BoxCollider2D enemyBoxCollider2D = _enemyPrefab.GetComponent<BoxCollider2D>();
            _enemyColliderHalfWidth = enemyBoxCollider2D.size.x / 2;
            _enemyColliderHalfHeight = enemyBoxCollider2D.size.y / 2;
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
        SetMinAndMax(randomPos);

        int spawnTries = 1;

        // If it detects another collider in the randomPos and we haven't tried the max number of times...
        while (Physics2D.OverlapArea(_min, _max) != null && spawnTries < MaxSpawnTries)
        {
            // Choose another spot
            randomPos = GetRandomSpawnPos();
            // SetMinAndMax(randomPos);

            spawnTries++;
            Debug.Log(spawnTries);
        }

        // if there's no collider detected, it's a safe spot to spawn
        if (Physics2D.OverlapArea(_min, _max) == null)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, randomPos, quaternion.identity);
            newEnemy.transform.parent = _enemyParent.transform;
        }
    }

    private void SetMinAndMax(Vector3 location)
    {
        _min.x = location.x - _enemyColliderHalfWidth;
        _min.y = location.y - _enemyColliderHalfHeight;
        _max.x = location.x + _enemyColliderHalfWidth;
        _max.y = location.y + _enemyColliderHalfHeight;
    }

    private Vector2 GetRandomSpawnPos()
    {
        float randomXPos = Random.Range((-_arenaSize.x) + 2, (_arenaSize.x * 0.5f) - 2);
        float randomYPos = Random.Range((-_arenaSize.y * 0.5f) + 2, (_arenaSize.y * 0.5f) - 2);
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