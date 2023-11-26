using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private EnemyController[] _enemysPrefabs;

    [SerializeField]
    private float spawnRange = 10f;

    [SerializeField]
    private float _enemySpawnTime = 5f;

    [SerializeField]
    private int _maxEnemysInGame = 8;

    [SerializeField]
    private float _delayedTimeToRelease = 4f;

    private ObjectPool<EnemyController> _enemyPool;
    private float _initialSpawnTime;
    private GameObject _enemyPoolParent;
    private int _enemysInGame = 0;

    void Start()
    {
        _initialSpawnTime = _enemySpawnTime;
        _enemyPool = new ObjectPool<EnemyController>(CreateEnemy);
        _enemyPoolParent = new GameObject("EnemyPool");
    }

    void Update()
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
        enemy.transform.SetParent(_enemyPoolParent.transform);
    }

    private EnemyController CreateEnemy()
    {
        return Instantiate(_enemysPrefabs[Random.Range(0, _enemysPrefabs.Length)]);
    }

    private void OnEnemyDestroyed(EnemyController enemy)
    {
        StartCoroutine(DelayedOnEnemyDestroyed(enemy));
    }

    private IEnumerator DelayedOnEnemyDestroyed(EnemyController enemy)
    {
        yield return new WaitForSeconds(_delayedTimeToRelease);

        // Check if the enemy is already inactive
        if (!enemy.gameObject.activeInHierarchy)
        {
            yield break;
        }

        enemy.gameObject.SetActive(false);
        _enemyPool.Release(enemy);
        _enemysInGame--;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = Camera.main.aspect * halfHeight;

        Vector3 cameraPos = Camera.main.transform.position;

        float x = Random.Range(
            cameraPos.x - halfWidth - spawnRange,
            cameraPos.x + halfWidth + spawnRange
        );
        float y = Random.Range(
            cameraPos.y - halfHeight - spawnRange,
            cameraPos.y + halfHeight + spawnRange
        );

        if (Random.value < 0.5f)
        {
            x =
                Random.value < 0.5f
                    ? cameraPos.x - halfWidth - spawnRange
                    : cameraPos.x + halfWidth + spawnRange;
        }
        else
        {
            y =
                Random.value < 0.5f
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
