using System;
using UnityEngine;
using Photon.Pun;

public class HealthPhoton : MonoBehaviourPunCallbacks, IDamageable
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

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        photonView.RPC("UpdateHealthRPC", RpcTarget.AllBuffered, (float)_currentHealth / _maxHealth);
    }

    public void Kill()
    {
        _currentHealth = 0;
        photonView.RPC("UpdateHealthRPC", RpcTarget.AllBuffered, (float)_currentHealth / _maxHealth);
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

        photonView.RPC("UpdateHealthRPC", RpcTarget.AllBuffered, (float)_currentHealth / _maxHealth);
    }

    [PunRPC]
    public void UpdateHealthRPC(float value)
    {
        OnTakeDamage?.Invoke(value);
    }
}
