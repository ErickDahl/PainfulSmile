using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Photon.Pun;

public abstract class ShipBasePhoton : MonoBehaviour
{
    [Header("Ship Configs")]
    [SerializeField]
    protected float _moveSpeed = 5f;

    [SerializeField]
    protected float _rotationSpeed = 5f;

    [Header("Bullet")]
    [SerializeField]
    protected BulletPhoton _bulletPrefab;

    [SerializeField]
    protected float _bulletSpeed = 10f;

    [SerializeField]
    protected float _fireRate = 0.5f;

    [SerializeField]
    protected int _bulletDamage = 10;

    protected Rigidbody2D _shipRB;
    protected Vector2 _moveDirection;
    protected bool _isShipDead;
    protected HealthPhoton _shipHealth;
    protected BoxCollider2D _shipCollider;
    protected PhotonView _photonView;

    private float _lastShootTime;
    private ObjectPool<BulletPhoton> _bulletPool;
    private GameObject _bulletPoolParent;
    private Transform _frontPoint;
    private Transform[] _sidePoints;

    protected abstract void MoveShip();

    protected abstract void RotateShip();

    protected abstract void OnShipDeath();

    private void Awake()
    {
        _shipHealth = GetComponent<HealthPhoton>();
        _shipRB = GetComponent<Rigidbody2D>();
        _shipCollider = GetComponent<BoxCollider2D>();
        _photonView = GetComponent<PhotonView>();
        _isShipDead = false;
        _bulletPool = new ObjectPool<BulletPhoton>(CreateBullet);
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

    private BulletPhoton CreateBullet()
    {
        GameObject bulletObject = PhotonNetwork.Instantiate(
            _bulletPrefab.name,
            Vector3.zero,
            Quaternion.identity
        );
        return bulletObject.GetComponent<BulletPhoton>();
    }

    protected void TryToShoot(ShootType shootType)
    {
        if (Time.time > _fireRate + _lastShootTime && !_isShipDead)
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

    [PunRPC]
    public void ActivateBullet(int bulletViewID, Vector3 position, Vector2 direction)
    {
        PhotonView bulletView = PhotonView.Find(bulletViewID);
        if (bulletView != null)
        {
            BulletPhoton bullet = bulletView.GetComponent<BulletPhoton>();
            bullet.gameObject.SetActive(true);
            bullet.transform.position = position;
            bullet.Spawn(direction * _bulletSpeed);
        }
    }

    private void SpawnBullet(Transform spawnPoint, Vector2 direction)
    {
        BulletPhoton bullet = _bulletPool.Get();
        bullet.gameObject.transform.SetParent(_bulletPoolParent.transform);
        bullet.OnCollisionEvent += HandleBulletCollision;

        Vector3 spawnPosition = spawnPoint.position + (Vector3)(direction.normalized * 0.1f);
        _photonView.RPC(
            "ActivateBullet",
            RpcTarget.AllBuffered,
            bullet.GetComponent<PhotonView>().ViewID,
            spawnPosition,
            direction
        );
    }

    [PunRPC]
    public void HideBullet(int bulletViewID)
    {
        PhotonView bulletView = PhotonView.Find(bulletViewID);
        if (bulletView != null)
        {
            bulletView.gameObject.SetActive(false);
        }
    }

    protected void HandleBulletCollision(BulletPhoton bullet, Collision2D collision)
    {
        _photonView.RPC(
            "HideBullet",
            RpcTarget.AllBuffered,
            bullet.GetComponent<PhotonView>().ViewID
        );

        _bulletPool.Release(bullet);

        if (collision != null)
        {
            if (collision.collider.TryGetComponent(out HealthPhoton damageable))
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
