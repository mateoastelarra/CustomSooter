using UnityEngine;

public interface IDamageable: IHitable
{
    void TakeDamage(Vector2 damageSourceDirection, int damageAmount, float knockbackTrhust = 0);
}
