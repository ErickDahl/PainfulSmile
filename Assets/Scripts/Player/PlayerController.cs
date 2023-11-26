using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputReader _playerInputReader;

    [Header("Player Configs")]
    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private float _rotationSpeed = 5f;

    [SerializeField]
    private float _maxVelocity = 5f;

    private Rigidbody2D _playerRB;
    private Vector2 _moveDirection;
    private bool _isShootingFront;
    private bool _isShootingSide;
    private bool _isPlayerDead;
    private Health _playerHealth;

    void OnEnable()
    {
        _playerInputReader.OnMoveEvent += GetMove;
        _playerInputReader.OnShootFrontEvent += GetShootFront;
        _playerInputReader.OnShootSideEvent += GetShootSide;
        _playerHealth.OnDeath += OnPlayerDeath;
    }

    void OnDisable()
    {
        _playerInputReader.OnMoveEvent -= GetMove;
        _playerInputReader.OnShootFrontEvent -= GetShootFront;
        _playerInputReader.OnShootSideEvent -= GetShootSide;
        _playerHealth.OnDeath -= OnPlayerDeath;
    }

    void Awake()
    {
        _playerHealth = GetComponent<Health>();
        _playerRB = GetComponent<Rigidbody2D>();
        _isPlayerDead = false;
    }

    void FixedUpdate()
    {
        if (!_isPlayerDead)
        {
            MovePlayer();
            RotatePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector2 force = transform.up * _moveDirection.y * _moveSpeed;
        _playerRB.AddForce(force);
        ClampVelocity();
    }

    private void ClampVelocity()
    {
        float x = Mathf.Clamp(_playerRB.velocity.x, -_maxVelocity, _maxVelocity);
        float y = Mathf.Clamp(_playerRB.velocity.y, -_maxVelocity, _maxVelocity);

        _playerRB.velocity = new Vector2(x, y);
    }

    private void RotatePlayer()
    {
        transform.Rotate(0, 0, _moveDirection.x * -_rotationSpeed);
    }

    private void OnPlayerDeath()
    {
        _isPlayerDead = true;
    }

    private void GetMove(Vector2 direction)
    {
        direction = DisableBackwardMoviment(direction);

        _moveDirection = direction;
    }

    private void GetShootFront(bool isShooting)
    {
        _isShootingFront = isShooting;
    }

    private void GetShootSide(bool isShooting)
    {
        _isShootingSide = isShooting;
    }

    private Vector2 DisableBackwardMoviment(Vector2 moviment)
    {
        if (moviment.y < 0)
            moviment.y = 0;

        return moviment;
    }
}
