using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBarrage : Skill
    {
    //const stats for the skill
    const float projectileSpeed = 20f;
    const float projectileDuration = 2f;
    const float baseAttackSpeed = 1f;
    const float baseProjectiles = 3;

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
            //start firing
            StartCoroutine(Shoot());

            //set wait time for next shot
            nextShotTime = Time.time + baseAttackSpeed / (1 + (PlayerStats.Stats["incAttackSpeed"].GetValue() / 100));
            }
        }
    IEnumerator Shoot()
        {
        float projCount = baseProjectiles + PlayerStats.Stats["addedProjectiles"].GetValue();

        float timeBetweenAttacks = baseAttackSpeed / (1 + (PlayerStats.Stats["incAttackSpeed"].GetValue() / 100));
        float timeBetweenShots = (timeBetweenAttacks / 2) / projCount;

        for (int i = 0; i < projCount; i++)
            {
            GameObject proj = Instantiate(projPrefab, firePoint.position, firePoint.rotation);
            proj.GetComponent<Projectile>().firedByPlayer = true;
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * projectileSpeed, ForceMode2D.Impulse);
            Destroy(proj, projectileDuration);

            //wait a duration between each shot
            yield return new WaitForSeconds(timeBetweenShots);
            }
        }
    }
