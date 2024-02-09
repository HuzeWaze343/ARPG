using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
    {
    public float viewDistance; //how far the enemy can detect the player from
    public float attackRange; //the minimum distance the enemy must be within to attack
    public float minimumDistance; //the closest that an enemy can be to the player, if they get any closer they will stop approaching
    public float movementSpeed; //the speed that the enemy is able to move
    public float baseAttackSpeed;
    public float baseDMG;
    public LayerMask lmPlayer;
    public LayerMask layersToRayCast;
    private string rarity;

    Animator animator;
    Rigidbody2D rb;
    GameObject target;
    Rigidbody2D targetRb;
    GameObject aoePrefab;
    EnemyHealth health;


    Vector2 destination;
    bool playerInSightRange;
    bool playerInSight;
    bool isMoving = false;
    float nextShotTime;
    
    private void Awake()
        {
        aoePrefab = Resources.Load("ClawSlashPrefab") as GameObject;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        targetRb = target.GetComponent<Rigidbody2D>();

        int rng = Random.Range(0, 100);
        if (rng > 95) rarity = "Rare";
        else if (rng > 75) rarity = "Magic";
        else rarity = "Normal";

        int baseHealth = 100;
        int startingHealth;

        if (rarity == "Rare")
            {
            startingHealth = baseHealth * 10;
            baseDMG = baseDMG * 3;
            gameObject.transform.localScale = new Vector3(1.9f, 1.9f, 1f);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.15f, 0 ,0);
            }
        else if (rarity == "Magic")
            {
            startingHealth = baseHealth * 2;
            baseDMG = baseDMG * 2;
            gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1);
            }
        else
            {
            startingHealth = baseHealth;
            }

        health = gameObject.GetComponent<EnemyHealth>();
        health.maxHp = startingHealth;
        }
    private void Update()
        {
        playerInSightRange = Physics2D.OverlapCircle(transform.position, viewDistance, lmPlayer);

        if (playerInSightRange)
            {
            Vector2 dir = (target.GetComponent<Rigidbody2D>().position - rb.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(rb.position, dir, viewDistance, layersToRayCast);
            if (hit.collider != null)
                {
                if (hit.collider.tag == "Player")
                    {
                    destination = targetRb.position;
                    isMoving = true;
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                    playerInSight = true;
                    }
                else
                    {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    playerInSight = false;
                    }
                }
            }
        else playerInSight = false;

        //movement
        if (rb.position != destination && isMoving == true)
            {
            float dist = Vector2.Distance(destination, rb.position);
            if ( dist > minimumDistance) //dont approach if the enemy is already within the minimum distance
                {
                float step = movementSpeed * Time.deltaTime;
                Vector2 movement = Vector2.MoveTowards(rb.position, destination, step);

                Vector2 dir = destination - rb.position;
                animator.SetFloat("Speed", dir.magnitude);
                animator.SetFloat("Horizontal", dir.x);
                animator.SetFloat("Vertical", dir.y);

                rb.position = movement;
                }
            }

        //attacking
        if (playerInSight == true)
            {
            float dist = Vector2.Distance(destination, rb.position);
            if (dist < attackRange)
                {
                Fire();
                }
            }

        else isMoving = false;
        }
    private void Fire()
        {
        if (nextShotTime < Time.time)
            {
            GameObject aoe = Instantiate(aoePrefab, target.transform.position, Quaternion.identity);
            aoe.GetComponentInChildren<AoeHit>().dmg = baseDMG;
            Destroy(aoe, 0.5f);
            nextShotTime = Time.time + baseAttackSpeed;
            }
        }
    }
