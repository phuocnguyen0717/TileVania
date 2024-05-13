using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRGB;
    void Start()
    {
        myRGB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        myRGB.velocity = new Vector2(moveSpeed, 0f);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-Mathf.Sign(myRGB.velocity.x), 1f);
    }
}
