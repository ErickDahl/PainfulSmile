using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private Animator _animator;
    private HealthPhoton _health;

    void OnEnable()
    {
        _health.OnDeath += PlayDeathExplosion;
        _health.OnTakeDamage += PlayExplosion;
    }

    void OnDisable()
    {
        _health.OnDeath -= PlayDeathExplosion;
        _health.OnTakeDamage -= PlayExplosion;
    }

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _health = GetComponent<HealthPhoton>();
    }

    private void PlayDeathExplosion()
    {
        _animator.SetTrigger("Death");
    }

    private void PlayExplosion(float value)
    {
        _animator.SetTrigger("Explode");
    }
}
