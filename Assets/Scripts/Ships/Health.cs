using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _maxHealth;

    private int _currentHealth;
    public event Action<float> OnTakeDamage;
    public event Action OnDeath;

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }

        OnTakeDamage?.Invoke((float)_currentHealth / _maxHealth);
    }
}
