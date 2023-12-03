using UnityEngine;

public class PlayerController : ShipBase
{
    [SerializeField]
    private float _maxVelocity = 5f;

    [SerializeField]
    private InputReader _playerInputReader;

    void OnEnable()
    {
        _playerInputReader.OnMoveEvent += GetMove;
        _playerInputReader.OnShootFrontEvent += ShootFront;
        _playerInputReader.OnShootSideEvent += ShootSide;
        _shipHealth.OnDeath += OnShipDeath;
    }

    void OnDisable()
    {
        _playerInputReader.OnMoveEvent -= GetMove;
        _playerInputReader.OnShootFrontEvent -= ShootFront;
        _playerInputReader.OnShootSideEvent -= ShootSide;
        _shipHealth.OnDeath -= OnShipDeath;
    }

    void FixedUpdate()
    {
        if (!_isShipDead)
        {
            MoveShip();
            RotateShip();
        }
    }

    protected override void OnShipDeath()
    {
        _isShipDead = true;
        _shipCollider.enabled = false;
        // GameManager.Instance.GameOver();
    }

    protected override void MoveShip()
    {
        Vector2 force = transform.up * _moveDirection.y * _moveSpeed;
        _shipRB.AddForce(force);
        ClampVelocity();
    }

    protected override void RotateShip()
    {
        transform.Rotate(0, 0, _moveDirection.x * -_rotationSpeed);
    }

    private void GetMove(Vector2 direction)
    {
        direction = DisableBackwardMoviment(direction);

        _moveDirection = direction;
    }

    private void ShootFront(bool isShooting)
    {
        if (isShooting)
        {
            TryToShoot(ShootType.Front);
        }
    }

    private void ShootSide(bool isShooting)
    {
        if (isShooting)
        {
            TryToShoot(ShootType.Side);
        }
    }

    private Vector2 DisableBackwardMoviment(Vector2 moviment)
    {
        if (moviment.y < 0)
            moviment.y = 0;

        return moviment;
    }

    private void ClampVelocity()
    {
        float x = Mathf.Clamp(_shipRB.velocity.x, -_maxVelocity, _maxVelocity);
        float y = Mathf.Clamp(_shipRB.velocity.y, -_maxVelocity, _maxVelocity);

        _shipRB.velocity = new Vector2(x, y);
    }
}
