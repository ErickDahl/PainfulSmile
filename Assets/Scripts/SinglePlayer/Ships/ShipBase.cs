using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class ShipBase : MonoBehaviour
{
    [Header("Ship Configs")]
    [SerializeField]
    protected float _moveSpeed = 5f;

    [SerializeField]
    protected float _rotationSpeed = 5f;

    [Header("Bullet")]
    [SerializeField]
    protected Bullet _bulletPrefab;

    [SerializeField]
    protected float _bulletSpeed = 10f;

    [SerializeField]
    protected float _fireRate = 0.5f;

    [SerializeField]
    protected int _bulletDamage = 10;

    protected Rigidbody2D _shipRB;
    protected Vector2 _moveDirection;
    protected bool _isShipDead;
    protected Health _shipHealth;
    protected BoxCollider2D _shipCollider;

    private float _lastShootTime;
    private ObjectPool<Bullet> _bulletPool;
    private GameObject _bulletPoolParent;
    private Transform _frontPoint;
    private Transform[] _sidePoints;

    protected abstract void MoveShip();
    protected abstract void RotateShip();
    protected abstract void OnShipDeath();

    private void Awake()
    {
        _shipHealth = GetComponent<Health>();
        _shipRB = GetComponent<Rigidbody2D>();
        _shipCollider = GetComponent<BoxCollider2D>();
        _isShipDead = false;
        _bulletPool = new ObjectPool<Bullet>(CreateBullet);
        _bulletPoolParent = new GameObject("BulletPool");
        _frontPoint = transform.Find("FrontBulletPoint");
        PopulateSidePoints();
    }

    private void PopulateSidePoints()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        List<Transform> sidePoints = new List<Transform>();

        foreach (Transform child in allChildren)
        {
            if (child.CompareTag("SideBulletPoint"))
            {
                sidePoints.Add(child);
            }
        }

        _sidePoints = sidePoints.ToArray();
    }

    private Bullet CreateBullet()
    {
        return Instantiate(_bulletPrefab);
    }

    protected void TryToShoot(ShootType shootType)
    {
        if (Time.time > _fireRate + _lastShootTime)
        {
            _lastShootTime = Time.time;

            Vector2 direction;
            Transform[] spawnPoints;

            switch (shootType)
            {
                case ShootType.Front:
                    direction = _frontPoint.transform.up;
                    spawnPoints = new Transform[] { _frontPoint };
                    break;
                case ShootType.Side:
                    direction = _frontPoint.transform.right;
                    spawnPoints = _sidePoints;
                    break;
                default:
                    direction = _frontPoint.transform.up;
                    spawnPoints = new Transform[] { _frontPoint };
                    break;
            }

            foreach (var spawnPoint in spawnPoints)
            {
                SpawnBullet(spawnPoint, direction);
            }
        }
    }

    private void SpawnBullet(Transform spawnPoint, Vector2 direction)
    {
        Bullet bullet = _bulletPool.Get();
        bullet.gameObject.transform.SetParent(_bulletPoolParent.transform);
        bullet.gameObject.SetActive(true);
        bullet.OnCollisionEvent += HandleBulletCollision;

        Vector3 spawnPosition = spawnPoint.position + (Vector3)(direction.normalized * 0.1f);
        bullet.transform.position = spawnPosition;
        bullet.Spawn(direction * _bulletSpeed);
    }

    protected void HandleBulletCollision(Bullet bullet, Collision2D collision)
    {
        bullet.gameObject.SetActive(false);
        _bulletPool.Release(bullet);

        if (collision != null)
        {
            if (collision.collider.TryGetComponent(out Health damageable))
            {
                damageable.TakeDamage(_bulletDamage);
            }
        }
    }

    protected enum ShootType
    {
        Front,
        Side
    }
}
