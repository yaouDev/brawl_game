using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputMaster controls;


    private float direction;
    private bool moving;
    private bool isGrounded;
    private int jumpCount;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;

    [Header("Read Only")]
    public float movementSpeed = 10f;
    public float jumpForce = 5f;
    public int jumpAmount = 2;
    public int crouchDivider = 2;

    //--

    private PlayerInput pi;


    //cannot subscribe controls to master, will trigger from anywhere. must subscribe directly from onActionTriggered
    private void Awake()
    {
        controls = new InputMaster();
        pi = transform.parent.gameObject.GetComponent<PlayerInput>();
        pi.onActionTriggered += ctx => ReadAction(ctx);

        switch (transform.parent.gameObject.GetComponent<Player>().playerId)
        {
            case 0:
                controls.Player1.Test.performed += ctx => Test(pi);
                controls.Player1.Movement.performed += ctx => MoveCE(ctx.ReadValue<float>(), true);
                controls.Player1.Movement.canceled += ctx => MoveCE(ctx.ReadValue<float>(), false);
                controls.Player1.Jump.performed += _ => JumpCE();
                controls.Player1.Crouch.performed += _ => CrouchCE(1);
                controls.Player1.Crouch.canceled += _ => CrouchCE(-1);
                break;
            case 1:
                controls.Player2.Test.performed += ctx => Test(pi);
                controls.Player2.Movement.performed += ctx => MoveCE(ctx.ReadValue<float>(), true);
                controls.Player2.Movement.canceled += ctx => MoveCE(ctx.ReadValue<float>(), false);
                controls.Player2.Jump.performed += _ => JumpCE();
                controls.Player2.Crouch.performed += _ => CrouchCE(1);
                controls.Player2.Crouch.canceled += _ => CrouchCE(-1);
                break;
            default:
                break;
        }

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }

    private void ReadAction(InputAction.CallbackContext ctx)
    {
        //read action and trigger appropriate method
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            Vector3 movement = new Vector3(direction, 0f);
            transform.position += movement * Time.deltaTime * movementSpeed;
        }
    }

    public virtual void MoveCE(float dir, bool active)
    {
        direction = dir;

        moving = active;
    }

    public virtual void JumpCE()
    {
        //cant jump if havent jumped as grounded
        if (isGrounded && jumpCount == 0 || jumpCount < jumpAmount && jumpCount != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            jumpCount++;
        }
    }

    public virtual void CrouchCE(int performed)
    {
        if (performed > 0)
        {
            col.size = new Vector2(col.size.x, col.size.y / crouchDivider);
            col.offset = new Vector2(0f, -col.size.y / crouchDivider);

            transform.localScale *= 0.5f;
        }
        else if (performed < 0)
        {
            col.size = new Vector2(col.size.x, col.size.y * crouchDivider);
            col.offset = Vector2.zero;

            transform.localScale *= 2f;
        }
    }

    //------------

    //callback calls started, performed, canceled
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
            //cant jump if havent jumped as grounded
            if (isGrounded && jumpCount == 0 || jumpCount < jumpAmount && jumpCount != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

                jumpCount++;
            }
        }
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        //add visual representation
        if (ctx.performed)
        {
            col.size = new Vector2(col.size.x, col.size.y / crouchDivider);
            col.offset = new Vector2(0f, -col.size.y / crouchDivider);

            transform.localScale *= 0.5f;
        }
        else if (ctx.canceled)
        {
            col.size = new Vector2(col.size.x, col.size.y * crouchDivider);
            col.offset = Vector2.zero;

            transform.localScale *= 2f;
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

    void Test(PlayerInput pi)
    {
        Debug.Log(pi.playerIndex);
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
