using System;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
    [SerializeField]
    private float _gameSessionTime = 1;

    private int _score = 0;
    private bool _canCountTimer;

    public int Score => _score;
    public float GameSessionTime => _gameSessionTime;
    public event Action OnGameOverEvent;

    void OnEnable()
    {
        SpawnManager.Instance.OnEnemyKilledEvent += OnEnemyKilled;
    }

    void OnDisable()
    {
        SpawnManager.Instance.OnEnemyKilledEvent -= OnEnemyKilled;
    }

    void Update()
    {
        if (_canCountTimer)
        {
            _gameSessionTime -= Time.deltaTime;

            if (_gameSessionTime <= 0)
            {
                GameOver();
            }
        }
    }

    public void EnableTimer()
    {
        _gameSessionTime = ConvertToMinutes(_gameSessionTime);
        _canCountTimer = true;
    }

    public void IncreaseGameTime()
    {
        if (_gameSessionTime == 3)
            return;

        _gameSessionTime++;
    }

    public void DecreaseGameTime()
    {
        if (_gameSessionTime > 1)
        {
            _gameSessionTime--;
        }
    }

    public void GameOver()
    {
        OnGameOverEvent?.Invoke();
        _canCountTimer = false;
        _gameSessionTime = 1;
    }

    private void OnEnemyKilled()
    {
        _score++;
    }

    private float ConvertToMinutes(float value)
    {
        return value * 60;
    }
}
