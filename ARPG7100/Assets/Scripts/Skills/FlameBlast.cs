using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBlast : Skill
    {
    const float baseAttackSpeed = 1f;
    const float castTime = 1f;
    const float damageMultiplier = 0.7f;

    //gameobject references
    GameObject aoePrefab;
    GameObject player;
    Rigidbody2D playerRB;
    Transform firePoint;

    LayerMask layersToRayCast;
    float nextShotTime;

    private void Awake()
        {
        //get references to gameobjects
        aoePrefab = Resources.Load("FlameBlastPrefab") as GameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();

        layersToRayCast = LayerMask.GetMask("Default");
        }
    public override void Fire()
        {
        if(Time.time > nextShotTime)
            {
            Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (mPos - playerRB.position).normalized;
            float distance = Vector2.Distance(playerRB.position, mPos);
            RaycastHit2D hit = Physics2D.Raycast(playerRB.position, dir, distance, layersToRayCast);
            Debug.DrawLine(player.transform.position, hit.point, Color.red);

            GameObject aoe;
            if (hit.collider == null)
                {
                aoe = Instantiate(aoePrefab, new Vector3(mPos.x, mPos.y, 0), new Quaternion(0, 0, 0, 0));
                Destroy(aoe, 2f);
                }
            else
                {
                aoe = Instantiate(aoePrefab, hit.point, new Quaternion(0, 0, 0, 0));
                Destroy(aoe, 2f);
                }
            aoe.GetComponentInChildren<AoeHit>().firedByPlayer = true;
            float minDmg = PlayerStats.Stats["addedMinDMG"].GetValue();
            float maxDmg = minDmg + PlayerStats.Stats["addedMaxDMG"].GetValue();
            aoe.GetComponentInChildren<AoeHit>().dmg = Random.Range(minDmg, maxDmg) * damageMultiplier;

            nextShotTime = Time.time + baseAttackSpeed / (1 + (PlayerStats.Stats["incAttackSpeed"].GetValue() / 100));
            }
        }
    }
