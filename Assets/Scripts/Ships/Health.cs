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

    public void Kill()
    {
        _currentHealth = 0;
        OnTakeDamage?.Invoke((float)_currentHealth / _maxHealth);
        OnDeath?.Invoke();
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
