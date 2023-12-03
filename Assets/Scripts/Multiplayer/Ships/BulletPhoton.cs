using System.Collections;
using System;
using UnityEngine;

public class BulletPhoton : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 2f;
    private float _timer;

    private Rigidbody2D rb;
    public event Action<BulletPhoton, Collision2D> OnCollisionEvent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Spawn(Vector3 spawnForce)
    {
        transform.forward = spawnForce.normalized;
        transform.rotation = Quaternion.identity;
        rb.AddForce(spawnForce);
        _timer = _lifeTime;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            OnCollisionEnter2D(null);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        OnCollisionEvent?.Invoke(this, other);
    }

    void OnDisable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        OnCollisionEvent = null;
    }
}
