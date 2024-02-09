using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projPrefab;

    public float projSpeed = 20f;
    public float projDuration = 2f;
    public float addedProj = 0f;
    public float incAttackSpeed;

    float baseAttackSpeed = 0.3f;
    float nextShotTime;

    // Update is called once per frame
    void Update()
        {
        if(Input.GetMouseButton(1))
            {
            if(Time.time > nextShotTime)
                {
                Fire();
                nextShotTime = Time.time + baseAttackSpeed / (1 + (PlayerStats.Stats["incAttackSpeed"].GetValue() / 100));
                }
            }
        }
    void Fire()
        {
        float projectileCount = 1 + addedProj + PlayerStats.Stats["addedProjectiles"].GetValue();
        if (projectileCount > 1)
            {
            Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(firePoint.position, mPos);
            Debug.Log(distance);

            float totalAngle = 100 / distance;
            if (totalAngle > 100) totalAngle = 100;
            float shotSpread = totalAngle / projectileCount;
            float offset = totalAngle / 2;

            for (int x = 0; x < projectileCount; x++)
                {

                //creates angle to offset projectile by
                //default rotation of firepoints + (angle between shots * number of projectile) - half of the total angle of all projectiles - constant offset
                Quaternion target = Quaternion.Euler(0, 0, (firePoint.rotation.z + (shotSpread * x) - offset + (shotSpread / 2)));
                Vector2 aim = target * firePoint.up;
                GameObject proj = Instantiate(projPrefab, firePoint.position, target * firePoint.rotation);
                proj.GetComponent<Projectile>().firedByPlayer = true;
                Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();

                rb.AddForce(aim * projSpeed, ForceMode2D.Impulse);
                Destroy(proj, projDuration);
                }

            }
        else
            {
            GameObject proj = Instantiate(projPrefab, firePoint.position, firePoint.rotation);
            proj.GetComponent<Projectile>().firedByPlayer = true;
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * projSpeed, ForceMode2D.Impulse);
            Destroy(proj, projDuration);
            }
        }
}
