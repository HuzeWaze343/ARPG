using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanOfIce : Skill
    {
    //const stats for the skill
    const float projectileSpeed = 20f;
    const float projectileDuration = 2f;
    const float baseAttackSpeed = 1f;
    const float baseProjectiles = 7;

    //gameobject references
    Transform firePoint;
    GameObject projPrefab;

    float nextShotTime;

    private void Awake()
        {
        //get references to gameobjects
        projPrefab = Resources.Load("WaterBoltPrefab") as GameObject;
        firePoint = GameObject.Find("FirePoint").GetComponent<Transform>();
        }
    public override void Fire()
        {
        if (Time.time > nextShotTime)
            {
            float projCount = baseProjectiles + PlayerStats.Stats["addedProjectiles"].GetValue();

            Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(firePoint.position, mPos);

            float totalAngle = 100 / distance;
            if (totalAngle > 100) totalAngle = 100;
            float spread = totalAngle / projCount;
            float offset = totalAngle / 2;

            for (int i = 0; i < projCount; i++)
                {
                //creating the angle to offset projectile by
                //default rotation of the firepoint + (angle between shots * number of projectile) - half of the total spread of all shots - constant offset
                Quaternion target = Quaternion.Euler(0, 0, (firePoint.rotation.z + (spread * i) - offset + (spread / 2)));
                Vector2 aim = target * firePoint.up;

                GameObject proj = Instantiate(projPrefab, firePoint.position, target * firePoint.rotation);
                proj.GetComponent<Projectile>().firedByPlayer = true;
                Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
                rb.AddForce(aim * projectileSpeed, ForceMode2D.Impulse);
                Destroy(proj, projectileDuration);
                }

            //set wait time for next shot
            nextShotTime = Time.time + baseAttackSpeed / (1 + (PlayerStats.Stats["incAttackSpeed"].GetValue() / 100));
            }
        }
    }
