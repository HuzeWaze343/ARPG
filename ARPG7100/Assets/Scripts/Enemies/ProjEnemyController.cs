using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjEnemyController : MonoBehaviour
    {
    public float viewDistance; //how far the enemy can detect the player from
    public float attackRange; //the minimum distance the enemy must be within to attack
    public float minimumDistance; //the closest that an enemy can be to the player, if they get any closer they will stop approaching
    public float movementSpeed; //the speed that the enemy is able to move
    public float baseAttackSpeed;
    public LayerMask lmPlayer;
    public LayerMask layersToRayCast;

    Animator animator;
    Rigidbody2D rb;
    GameObject target;
    Rigidbody2D targetRb;
    GameObject projPrefab;

    Vector2 destination;
    bool playerInSightRange;
    bool playerInSight;
    bool isMoving = false;
    float nextShotTime;
    
    private void Awake()
        {
        projPrefab = Resources.Load("WaterBoltPrefab") as GameObject;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        targetRb = target.GetComponent<Rigidbody2D>();
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

            nextShotTime = Time.time + baseAttackSpeed;
            }
        }
    }
