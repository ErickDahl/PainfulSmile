using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : GenericSingleton<SpawnManager>
{
    [SerializeField]
    private EnemyController[] _enemysPrefabs;

    [SerializeField]
    private float spawnRange = 10f;

    [SerializeField]
    private float _enemySpawnTime = 1f;

    [SerializeField]
    private int _maxEnemysInGame = 8;

    [SerializeField]
    private float _delayedTimeToRelease = 4f;

    private ObjectPool<EnemyController> _enemyPool;
    private float _initialSpawnTime;
    private int _enemysInGame = 0;
    private bool _canCountTimer;

    public event Action OnEnemyKilledEvent;
    public float EnemySpawnTime => _enemySpawnTime;

    void Start()
    {
        _enemyPool = new ObjectPool<EnemyController>(CreateEnemy);
    }

    void OnEnable()
    {
        GameManager.Instance.OnGameOverEvent += OnGameOver;
    }

    void OnDisable()
    {
        GameManager.Instance.OnGameOverEvent -= OnGameOver;
    }

    void Update()
    {
        if (_canCountTimer)
        {
            _enemySpawnTime -= Time.deltaTime;

            if (_enemySpawnTime <= 0 && _enemysInGame < _maxEnemysInGame)
            {
                _enemysInGame++;
                SpawnEnemy();
                _enemySpawnTime = _initialSpawnTime;
            }
            else if (_enemySpawnTime <= 0)
            {
                _enemySpawnTime = _initialSpawnTime;
            }
        }
    }

    private void OnGameOver()
    {
        _canCountTimer = false;
        _enemySpawnTime = 1;
    }

    public void EnableTimer()
    {
        _initialSpawnTime = _enemySpawnTime;
        _canCountTimer = true;
    }

    public void IncreaseEnemySpawnTime()
    {
        _enemySpawnTime++;
    }

    public void DecreaseEnemySpawnTime()
    {
        if (_enemySpawnTime > 1)
        {
            _enemySpawnTime--;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        while (Physics2D.OverlapCircle(spawnPosition, 1f))
        {
            spawnPosition = GetRandomSpawnPosition();
        }

        EnemyController enemy = _enemyPool.Get();
        enemy.transform.position = spawnPosition;
        enemy.gameObject.SetActive(true);
        enemy.ResetEnemy();
        enemy.OnEnemyDestroyedEvent += OnEnemyDestroyed;
    }

    private EnemyController CreateEnemy()
    {
        return Instantiate(_enemysPrefabs[UnityEngine.Random.Range(0, _enemysPrefabs.Length)]);
    }

    private void OnEnemyDestroyed(EnemyController enemy)
    {
        OnEnemyKilledEvent?.Invoke();
        StartCoroutine(DelayedOnEnemyDestroyed(enemy));
    }

    private IEnumerator DelayedOnEnemyDestroyed(EnemyController enemy)
    {
        yield return new WaitForSeconds(_delayedTimeToRelease);
        enemy.gameObject.SetActive(false);
        _enemyPool.Release(enemy);
        _enemysInGame--;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = Camera.main.aspect * halfHeight;

        Vector3 cameraPos = Camera.main.transform.position;

        float x = UnityEngine.Random.Range(
            cameraPos.x - halfWidth - spawnRange,
            cameraPos.x + halfWidth + spawnRange
        );
        float y = UnityEngine.Random.Range(
            cameraPos.y - halfHeight - spawnRange,
            cameraPos.y + halfHeight + spawnRange
        );

        if (UnityEngine.Random.value < 0.5f)
        {
            x =
                UnityEngine.Random.value < 0.5f
                    ? cameraPos.x - halfWidth - spawnRange
                    : cameraPos.x + halfWidth + spawnRange;
        }
        else
        {
            y =
                UnityEngine.Random.value < 0.5f
                    ? cameraPos.y - halfHeight - spawnRange
                    : cameraPos.y + halfHeight + spawnRange;
        }

        return new Vector3(x, y, 0);
    }

    void OnDrawGizmos()
    {
        if (!Camera.main)
            return;

        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = Camera.main.aspect * halfHeight;

        Vector3 cameraPos = Camera.main.transform.position;

        Vector3 topLeft = new Vector3(
            cameraPos.x - halfWidth - spawnRange,
            cameraPos.y + halfHeight + spawnRange,
            0
        );
        Vector3 topRight = new Vector3(
            cameraPos.x + halfWidth + spawnRange,
            cameraPos.y + halfHeight + spawnRange,
            0
        );
        Vector3 bottomLeft = new Vector3(
            cameraPos.x - halfWidth - spawnRange,
            cameraPos.y - halfHeight - spawnRange,
            0
        );
        Vector3 bottomRight = new Vector3(
            cameraPos.x + halfWidth + spawnRange,
            cameraPos.y - halfHeight - spawnRange,
            0
        );

        Gizmos.color = Color.red;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
