using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeHit : MonoBehaviour
    {
    public float dmg;
    public bool firedByPlayer = false;
    private void Start()
        {
        Collider2D collider = gameObject.GetComponent<Collider2D>();

        //set the layermask to look for enemies in based on whether the skill was fired by a player or an enemy
        LayerMask lm;
        if (firedByPlayer)
            lm = LayerMask.GetMask("Enemy");
        else
            lm = LayerMask.GetMask("Player");
        ContactFilter2D hitFilter = new ContactFilter2D();
        hitFilter.SetLayerMask(lm);

        
        List<Collider2D> enemiesHit = new List<Collider2D>();
        collider.OverlapCollider(hitFilter, enemiesHit);

        for (int i = 0; i < enemiesHit.Count; i++)
            {
            if (firedByPlayer)
                enemiesHit[i].GetComponentInChildren<EnemyHealth>().damage(dmg);
            else
                enemiesHit[i].GetComponentInChildren<Health>().damage(dmg);
            }
        }
    }
