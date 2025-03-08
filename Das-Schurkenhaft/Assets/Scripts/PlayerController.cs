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
    int currentHealth = 5;
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
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        move = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount) {
    Debug.Log("Current Health: " + currentHealth);
    currentHealth = Mathf.Clamp(currentHealth - 9, 0, maxHealth);
    Debug.Log("Current Health: " + currentHealth);
    currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    Debug.Log("Current Health: " + currentHealth);
    }

    // Add other stat modification methods if needed
    public void ChangeSpeed(float amount) {
        speed += amount; // Modify the player's speed
        Debug.Log("New Speed: " + speed);
    }

    public void ChangeStrength(int amount) {
        strength += amount; // Modify strength
        Debug.Log("New Strength: " + strength);
    }

    public void ChangeDefense(int amount) {
        defense += amount; // Modify defense
        Debug.Log("New Defense: " + defense);
    }

}