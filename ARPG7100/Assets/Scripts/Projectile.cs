using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float dmg;
    public bool firedByPlayer = false;
    void Start()
    {
        //if (firedByPlayer) Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());

        if (firedByPlayer) gameObject.layer = LayerMask.NameToLayer("Projectiles");
        else gameObject.layer = LayerMask.NameToLayer("EnemyProjectiles");
    }
    private void OnCollisionEnter2D(Collision2D collision)
        {
        //get the health component of the collision and deal damage to it
        if (!firedByPlayer && collision.gameObject.CompareTag("Player")) collision.gameObject.GetComponent<Health>().damage(dmg);
        else if (firedByPlayer && collision.gameObject.CompareTag("Enemy")) collision.gameObject.GetComponent<EnemyHealth>().damage(dmg);
        Destroy(gameObject);
        }
    }
