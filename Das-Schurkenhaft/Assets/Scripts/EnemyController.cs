using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    //stats
    public int maxHealth;
    int currentHealth;
    public int strength;
    public int defense;
    public float speed;

    Rigidbody2D rigidbody2d;
    float timer;
    int direction = 1;
    bool vertical = true;
    public float changeTime = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }

    void FixedUpdate() {
        Vector2 position = rigidbody2d.position;
        
        if (vertical) {
           position.y = position.y + speed * Time.deltaTime;
        } else {
           position.x = position.x + speed * Time.deltaTime;
        }
        rigidbody2d.MovePosition(position);
    }
    // Update is called once per frame
    void Update() 
    {
        timer  -= Time.deltaTime;

        if (timer < 0) {
            direction = -direction;
            timer = changeTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered with: " + other.name);
        if(other.name == "Player")
        {
            Debug.Log("Loading Combat Scene...");
            SceneManager.LoadScene("CombatScene");
        }
    }
}
