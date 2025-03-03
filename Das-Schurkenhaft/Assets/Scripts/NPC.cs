using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Vector3 directionVector;
    private Transform myTransform;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    public Collider2D bounds;
    public bool playerInRange = false;
    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        ChangeDirection();
    }

    // Update is called once per frame for timer logic
    void Update()
    {
        if(!playerInRange){
            Move();
        }
    
    }


    private void Move()
    {
        // Calculate the new position using FixedDeltaTime.
        Vector3 newPosition = myTransform.position + directionVector * speed * Time.fixedDeltaTime;
        // Optional debug log to see where we're trying to move.
        // Debug.Log("Attempting move to: " + newPosition);

        // Check if the new position is within the allowed bounds.
        if (bounds.bounds.Contains(newPosition))
        {
            myRigidbody.MovePosition(newPosition);
        }
        else
        {
            // If out of bounds, change direction.
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                directionVector = Vector3.right;
                break;
            case 1:
                directionVector = Vector3.up;
                break;
            case 2:
                directionVector = Vector3.left;
                break;
            case 3:
                directionVector = Vector3.down;
                break;
            default:
                break;
        }
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (anim != null)
        {
            anim.SetFloat("MoveX", directionVector.x);
            anim.SetFloat("MoveY", directionVector.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector3 temp = directionVector;
        ChangeDirection();
        int loops = 0;
        while(temp == directionVector && loops < 100){
            loops++;
            ChangeDirection();
        }
    }
} 