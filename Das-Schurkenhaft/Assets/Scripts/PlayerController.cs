using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;

    //====Stats=====//
    //Health (Vitality?)
    public int maxHealth = 10;
    int currentHealth;
    //speed (Dexterity?)
    public float speed = 3.0f;
    public int strength = 1;
    public int defense = 1;

    
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

    }

    void FixedUpdate()
<<<<<<< Updated upstream
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
=======
    { 
        if (_currentState == PlayerState.MOVE)
        {
            Vector2 velocity = move * speed;
            rigidbody2d.linearVelocity = velocity;
        }
        else
        {
            rigidbody2d.linearVelocity = Vector2.zero;
        }
>>>>>>> Stashed changes
    }

    void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}
