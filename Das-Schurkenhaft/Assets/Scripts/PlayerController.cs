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
    private Shield currentShield; // Keeps track of if shield is equipped
    
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
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
    
    public void ChangeDefense(int amount)
    {
        defense += amount;
        Debug.Log("Player's defense updated to " + defense);
    }
    public void EquipShield(Shield shield)
    {
        if (currentShield == null)
        {
            currentShield = shield; // Equip the shield
            ChangeDefense(currentShield.defenseBoost);
            Debug.Log("Equipped shield: " + currentShield.name + ". Defense increased to " + defense);
        }
        else
        {
            Debug.Log("Already equipped with a shield: " + currentShield.name);
        }
    }
    public void DestroyShield()
    {
        if (currentShield != null)
        {
            ChangeDefense(-currentShield.defenseBoost);
            Debug.Log("Shield destroyed: " + currentShield.name + ". Defense decreased to " + defense);
            currentShield = null; // Get rid of shield
        }
    }

}

