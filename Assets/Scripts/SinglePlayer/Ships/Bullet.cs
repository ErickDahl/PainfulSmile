using System.Collections;
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 2f;

    private Rigidbody2D rb;
    public event Action<Bullet, Collision2D> OnCollisionEvent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Spawn(Vector3 spawnForce)
    {
        transform.forward = spawnForce.normalized;
        transform.rotation = Quaternion.identity;
        rb.AddForce(spawnForce);
        StartCoroutine(DisableBullet(_lifeTime));
    }

    IEnumerator DisableBullet(float time)
    {
        yield return new WaitForSeconds(time);
        OnCollisionEnter2D(null);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        OnCollisionEvent?.Invoke(this, other);
    }

    void OnDisable()
    {
        StopAllCoroutines();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        OnCollisionEvent = null;
    }
}
