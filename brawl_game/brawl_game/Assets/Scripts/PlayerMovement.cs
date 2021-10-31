using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputMaster controls;
    public float movementSpeed = 10f;
    public float jumpForce = 5f;
    public int jumpAmount = 2;

    private float direction;
    private bool moving;
    private bool isGrounded;
    private int jumpCount;

    private Rigidbody2D rb;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Test.performed += ctx => Test();

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            Vector3 movement = new Vector3(direction, 0f);
            transform.position += movement * Time.deltaTime * movementSpeed;
        }
    }

    //callback calls started, performed, cancelled
    public void Move(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<float>();

        if (ctx.performed)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (isGrounded && jumpCount == 0 || jumpCount < jumpAmount && jumpCount != 0)
        {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

                jumpCount++;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("enter");
        isGrounded = GroundedCollision(collision);
        jumpCount = 0;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (GroundedCollision(collision))
        {
            isGrounded = false;
        }
    }

    private bool GroundedCollision(Collision2D collision)
    {
        bool correctTag = collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player") ? true : false;
        return collision.gameObject.transform.position.y < transform.position.y && correctTag ? true : false;
    }

    void Test()
    {
        Debug.Log("test");
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
