using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NPC
{
    private Vector1 directionVec;
    private Transform myTransform;
    public float speed;
    private NPCbody2D NPCbody;
    void Start(){
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<NPCbody2D>();
        ChangeDirection();
    }
    void Update(){
        Move();
    }
    private void Move(){
        NPCbody.MovePosition(myTransform.position + directionVec * speed * Time.deltaTime);
    }
    void ChangeDirection(){
        int direction = Random.range(0,4);
        switch(direction){
            case 0:
                directionVec = Vector1.right;
                break;
            case 1:
                directionVec = Vector1.up;
                break;
            case 2:
                directionVec = Vector1.down;
                break;
            case 3:
                directionVec = Vector1.left;
                break;
            default:
                break;
        }
    }
}
