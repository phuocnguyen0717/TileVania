using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f; // tốc độ di chuyển 
    [SerializeField] float jumpSpeed = 12f; // tốc độ nhảy
    [SerializeField] float climbSpeed = 5f; // tốc độ leo 
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f);
    Vector2 moveInput; // hướng di chuyển x,y
    Rigidbody2D myRigidbody; // khai báo đối tượng Rigid của unity

    Animator myAnimator;
    BoxCollider2D myFoot;
    CapsuleCollider2D myCapsule2d;
    float myGravicityScaleStart;
    bool isAlive = true;
    void Start()
    {
        myCapsule2d = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        myFoot = GetComponent<BoxCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>(); // sử dụng component của Rigidbody
        myGravicityScaleStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (!myFoot.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }
    void Run()
    {
        if (!isAlive) { return; }

        // giữ nguyên trục y, trục x sẽ thay đổi theo runSpeed cho người dùng thiết lập
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);
    }
    void ClimbLadder()
    {
        if (!myCapsule2d.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = myGravicityScaleStart;
            myAnimator.SetBool("IsClimbing", false);
            return;
        }

        myRigidbody.gravityScale = 0f;

        Vector2 playerVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("IsClimbing", playerHasHorizontalSpeed);
    }
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        //nếu nhân vật có di chuyển
        if (playerHasHorizontalSpeed)
        {
            // nhân vật quay trái(-1)/phải(1)
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }
    void Die()
    {
        if (myCapsule2d.IsTouchingLayers(LayerMask.GetMask("Enemies", "Trap")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
        }
    }
}
