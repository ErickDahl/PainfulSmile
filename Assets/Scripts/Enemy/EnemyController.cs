using UnityEngine;
using UnityEngine.AI;

public class EnemyController : ShipBase
{
    [SerializeField]
    private float _attackRange = 3f;

    [SerializeField]
    private int _chaserDamage = 50;

    [SerializeField]
    private float _bufferRange = 5f;

    [SerializeField]
    private EnemyType _enemyType = EnemyType.Shooter;

    private Transform _playerTransform;
    private NavMeshAgent _agent;
    private bool _isShooter;
    private bool _isChaser;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _moveSpeed;
        SetEnemyBehavior();
    }

    void Update()
    {
        if (!_isShipDead)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (_isShooter)
            {
                ShooterBehavior();
            }
            else if (_isChaser)
            {
                ChaserBehavior();
            }
        }
    }

    private void SetEnemyBehavior()
    {
        switch (_enemyType)
        {
            case EnemyType.Shooter:
                _isShooter = true;
                break;
            case EnemyType.Chaser:
                _isChaser = true;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other != null)
        {
            if (other.collider.TryGetComponent(out Health player))
            {
                player.TakeDamage(_chaserDamage);
            }

            if (
                other.otherCollider.TryGetComponent(out Health enemy)
                && !other.collider.CompareTag("Bullet")
            )
            {
                enemy.Kill();
                _shipCollider.enabled = false;
            }
        }
    }

    private void ChaserBehavior()
    {
        RotateShip();
        _agent.SetDestination(_playerTransform.position);
    }

    private void ShooterBehavior()
    {
        float distance = Vector3.Distance(_playerTransform.position, transform.position);
        RotateShip();

        if (distance <= _attackRange)
        {
            _agent.isStopped = true;
            TryToShoot(ShootType.Front);
        }
        else if (distance > _bufferRange)
        {
            MoveShip();
        }
    }

    protected override void MoveShip()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_playerTransform.position);
    }

    protected override void RotateShip()
    {
        Vector3 direction = _playerTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotation,
            _rotationSpeed * Time.deltaTime
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _bufferRange);
    }
}

public enum EnemyType
{
    Shooter,
    Chaser
}