using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    //stats
    
    public float speed;

    public SPUM_Prefabs _prefabs;
    private Vector3 originalScale;
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
        if (_prefabs == null)
        {
            
            _prefabs = transform.GetComponent<SPUM_Prefabs>();
            if (!_prefabs.allListsHaveItemsExist())
            {
                _prefabs.PopulateAnimationLists();
            }
        }
        originalScale = _prefabs.transform.localScale;
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
        if (direction == 1) {
            _prefabs.transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        } else {
            _prefabs.transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        //Debug.Log("Timer: " + timer);
        if (timer < 0) {
            direction = -direction;
            vertical = !vertical;
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
