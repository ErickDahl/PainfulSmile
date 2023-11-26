using System;

public interface IDamageable
{
    public event Action<float> OnTakeDamage;
    public event Action OnDeath;

    public void TakeDamage(int damage);
}
