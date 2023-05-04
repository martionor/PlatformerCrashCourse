using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;

    public Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //See if target is valid to hit
        Damageable damagable = collision.GetComponent<Damageable>();

        if (damagable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            //Hit the target
            bool gotHit = damagable.Hit(attackDamage, deliveredKnockback);

            if (gotHit)
            {
                Debug.Log(collision.name + " hit for "+ attackDamage);
            }
        }
    }
}
