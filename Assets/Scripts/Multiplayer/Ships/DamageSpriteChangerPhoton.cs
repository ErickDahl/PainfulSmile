using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpriteChangerPhoton : MonoBehaviour
{
    [SerializeField]
    private ShipDamageStatesSO _damageStates;

    private HealthPhoton _health;
    private SpriteRenderer _spriteRenderer;

    void OnEnable()
    {
        _health.OnTakeDamage += ChangeSprite;
    }

    void OnDisable()
    {
        _health.OnTakeDamage -= ChangeSprite;
    }

    void Awake()
    {
        _health = GetComponent<HealthPhoton>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartWithHealthySprite();
    }

    private void StartWithHealthySprite()
    {
        _spriteRenderer.sprite = _damageStates.HealthySprite;
    }

    private void ChangeSprite(float healthPercent)
    {
        if (healthPercent > 0.6)
        {
            _spriteRenderer.sprite = _damageStates.HealthySprite;
        }
        else if (healthPercent > 0.4)
        {
            _spriteRenderer.sprite = _damageStates.DamagedSprite;
        }
        else if (healthPercent > 0)
        {
            _spriteRenderer.sprite = _damageStates.BadlyDamagedSprite;
        }
        else
        {
            _spriteRenderer.sprite = _damageStates.DeadSprite;
        }
    }
}
