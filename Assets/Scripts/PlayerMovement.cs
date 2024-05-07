using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f; // tốc độ di chuyển 
    [SerializeField] float jumpSpeed = 20f; // tốc độ nhảy
    Vector2 moveInput; // hướng di chuyển x,y
    Rigidbody2D myRigidbody; // khai báo đối tượng Rigid của unity

    Animator myAnimator;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>(); // sử dụng component của Rigidbody
    }

    void Update()
    {
        Run();
        FlipSprite();
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }
    void Run()
    {
        // giữ nguyên trục y, trục x sẽ thay đổi theo runSpeed cho người dùng thiết lập
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        Debug.Log(playerHasHorizontalSpeed);
        myAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);
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
}
