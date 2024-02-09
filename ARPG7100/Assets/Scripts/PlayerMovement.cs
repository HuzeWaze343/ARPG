using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
    {
    const float baseMovementSpeed = 5f;
    float movementSpeed;

    public Rigidbody2D rigidBody;
    public Animator animator;
    public Rigidbody2D firingAngle;

    Vector2 destination;
    bool isMoving;
    bool isInteracting;

    private void Start()
        {
        Equipment.instance.onEquipmentChangedCallback += UpdateMovementSpeed;
        movementSpeed = baseMovementSpeed + PlayerStats.Stats["incMovementSpeed"].GetValue();
        }

    private void Update()
        {
        //get pointer position and store in var
        Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //set firing angle to mouseposition
        Vector2 lookDir = mPos - firingAngle.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        firingAngle.rotation = angle - 90f;

        //if the mouse pointer is over UI, end the script
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //if lmb is clicked, check for any interactable objects on the clicked location and interact with them
        if (Input.GetMouseButtonDown(0))
            {
            //generate a ray on mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            //if the ray hits something
            if (hit.collider != null)
                {
                //check that the hit object is interactable
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                //if hit object is interactable and within range
                if(interactable != null && Vector2.Distance(gameObject.transform.position, hit.collider.transform.position) < 2f)
                    {
                    //interact with the object and exit the script
                    interactable.Interact();
                    isInteracting = true;
                    }
                }
            }

        if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0)) isInteracting = false;

        //if lmb is pressed or held, set character move destination to mouse point
        else if (!isInteracting && Input.GetMouseButton(0))
            {
            destination = mPos;
            isMoving = true;
            }

        }
    private void FixedUpdate()
        {
        if (isMoving & rigidBody.position != destination)
            {
            //set player movement vector
            float step = movementSpeed * Time.deltaTime;
            Vector2 movement = Vector2.MoveTowards(rigidBody.position, destination, step);

            //get movement direction (for anim)
            Vector2 dir = destination - rigidBody.position;

            //set player position to the movement vector
            rigidBody.position = movement;

            //set floats in animator
            animator.SetFloat("Speed", dir.magnitude);
            animator.SetFloat("Horizontal", dir.normalized.x);
            animator.SetFloat("Vertical", dir.normalized.y);
            }
        else
            {
            //disable all movement
            animator.SetFloat("Speed", 0f);
            isMoving = false;
            }
        }
    private void UpdateMovementSpeed()
        {
        movementSpeed = baseMovementSpeed * (1 + (PlayerStats.Stats["incMovementSpeed"].GetValue() / 100));
        }
    }
