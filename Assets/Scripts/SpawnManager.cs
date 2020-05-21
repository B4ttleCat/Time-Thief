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
    private Vector2 _arenaSize;

    public Vector2 ArenaSize
    {
        get { return _arenaSize; }
        private set { _arenaSize = value; }
    }

    private GameObject _enemyParent;

    private void Awake()
    {
        Destroy(_enemyParent);
        Debug.Log(_enemyParent);
    }

    void Start()
    {
        _spawnTimer = 0f;
        _nextSpawn = 0f;

        ArenaSize = new Vector2(_arena.cellBounds.x, _arena.cellBounds.y);
            

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
        float randomXPos = Random.Range((-_arenaSize.x * 0.5f) + 2, (_arenaSize.x * 0.5f) - 2);
        float randomYPos = Random.Range((-_arenaSize.y * 0.5f) + 2, (_arenaSize.y * 0.5f) - 2);
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