using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SPUM_Prefabs _prefabs;
    public float speed = 3.0f;
    public Dictionary<PlayerState, int> IndexPair = new Dictionary<PlayerState, int>();
    private PlayerState _currentState = PlayerState.IDLE;
    private Rigidbody2D rigidbody2d;
    private Vector2 move;

    public InputAction MoveAction;

    public int maxHealth = 10;
    private int currentHealth;
    public int strength = 1;
    public int defense = 1;

    private Vector3 originalScale;

    void Start()
    {
        if (_prefabs == null)
        {
            _prefabs = transform.GetChild(0).GetComponent<SPUM_Prefabs>();
            if (!_prefabs.allListsHaveItemsExist())
            {
                _prefabs.PopulateAnimationLists();
            }
        }
        _prefabs.OverrideControllerInit();

        originalScale = _prefabs.transform.localScale;

        foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
        {
            IndexPair[state] = 0;
        }

        rigidbody2d = GetComponent<Rigidbody2D>();

        if (MoveAction != null)
            MoveAction.Enable();

        currentHealth = maxHealth;

        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.bodyType = RigidbodyType2D.Dynamic;

        if (MoveAction != null)
            MoveAction.Enable();

        currentHealth = maxHealth;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        move = new Vector2(moveX, moveY).normalized;

        if (move.sqrMagnitude > 0.01f)
        {
            _currentState = PlayerState.MOVE;

            if (move.x > 0)
                _prefabs.transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            else if (move.x < 0)
                _prefabs.transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            _currentState = PlayerState.IDLE;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.localPosition.y * 0.01f);

        PlayStateAnimation(_currentState);
    }

    void FixedUpdate()
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
    }

    public void SetStateAnimationIndex(PlayerState state, int index = 0)
    {
        IndexPair[state] = index;
    }

    public void PlayStateAnimation(PlayerState state)
    {
        _prefabs.PlayAnimation(state, IndexPair[state]);
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log("Current Health: " + currentHealth);
    }

    public void ChangeSpeed(float amount)
    {
        speed += amount;
        Debug.Log("New Speed: " + speed);
    }

    public void ChangeStrength(int amount)
    {
        strength += amount;
        Debug.Log("New Strength: " + strength);
    }

    public void ChangeDefense(int amount)
    {
        defense += amount;
        Debug.Log("New Defense: " + defense);
    }
    
    public Vector2 GetPosition()
    {
        return rigidbody2d.position;
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
