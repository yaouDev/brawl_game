using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //disable collider while dodging?

    public AnimationCurve moveCurve;
    [SerializeField] protected float dashFailTime = 3f;
    [SerializeField] protected float groundedOffset = 0.2f;
    protected float direction;
    protected bool moving;
    protected bool isGrounded;
    protected int jumpCount;

    protected Rigidbody2D rb;
    protected CapsuleCollider2D col;
    protected PlayerControls pc;

    public float movementSpeed = 20f;
    public float jumpForce = 22f;
    public int jumpAmount = 2;
    public float dashSpeed = 2f;
    public float dashDistance = 5f;

    [Header("Read Only")]
    public GameObject currentGround;
    //--

    //---
    protected float moveTime;
    public float timeToMaxSpeed = 1f;

    //---

    //private PlayerInput pi;

    protected virtual void Awake()
    {
        //controls = new InputMaster();
        //pi = transform.parent.gameObject.GetComponent<PlayerInput>();

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        pc = transform.parent.GetComponent<PlayerControls>();
    }

    //private void ReadAction(InputAction.CallbackContext ctx)
    //{
    //    //read action and trigger appropriate method
    //    Debug.Log(pi.currentActionMap);

    //    if(ctx.action == controls.MenuPlayer.Move)
    //    {
    //        print("ligma");
    //    }

    //    if(ctx.action == controls.Player.Jump)
    //    {
    //        print("yahoo");
    //    }

    //    //controls.Player.Test.performed += ctx => Test(pi);
    //    //controls.Player.Movement.performed += ctx => MoveCE(ctx.ReadValue<float>(), true);
    //    //controls.Player.Movement.canceled += ctx => MoveCE(ctx.ReadValue<float>(), false);
    //    //controls.Player.Jump.performed += _ => JumpCE();
    //    //controls.Player.Crouch.performed += _ => CrouchCE(1);
    //    //controls.Player.Crouch.canceled += _ => CrouchCE(-1);
    //}

    protected virtual void FixedUpdate()
    {
        if (moving)
        {
            if (moveTime < timeToMaxSpeed)
            {
                moveTime += Time.fixedDeltaTime;
            }

            //fix so that external forces dont maim movement ability
            Vector2 movement = new Vector2(direction, 0f);

            //add curve relation
            float modifiedSpeed = movementSpeed * moveCurve.Evaluate(moveTime / timeToMaxSpeed);

            //transform.position += movement * Time.deltaTime * modifiedSpeed;
            rb.position += movement * Time.deltaTime * modifiedSpeed;
        }
    }

    //public virtual void MoveCE(float dir, bool active)
    //{
    //    direction = dir;

    //    moving = active;
    //}

    //public virtual void JumpCE()
    //{
    //    //cant jump if havent jumped as grounded
    //    if (isGrounded && jumpCount == 0 || jumpCount < jumpAmount && jumpCount != 0)
    //    {
    //        rb.velocity = new Vector2(rb.velocity.x, 0);
    //        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

    //        jumpCount++;
    //    }
    //}

    //public virtual void CrouchCE(int performed)
    //{
    //    if (performed > 0)
    //    {
    //        col.size = new Vector2(col.size.x, col.size.y / crouchDivider);
    //        col.offset = new Vector2(0f, -col.size.y / crouchDivider);

    //        transform.localScale *= 0.5f;
    //    }
    //    else if (performed < 0)
    //    {
    //        col.size = new Vector2(col.size.x, col.size.y * crouchDivider);
    //        col.offset = Vector2.zero;

    //        transform.localScale *= 2f;
    //    }
    //}

    //------------

    //callback calls started, performed, canceled
    public virtual void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            direction = ctx.ReadValue<float>();
            moving = true;
        }
        else
        {
            moveTime = 0f;
            moving = false;
        }
    }

    public virtual void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && CanJump())
        {
            //cant jump if havent jumped as grounded

            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            jumpCount++;
        }
    }

    public virtual bool CanJump()
    {
        return isGrounded && jumpCount == 0 || jumpCount < jumpAmount && jumpCount != 0;
    }

    public virtual void Down(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded && currentGround != null)
        {
            if (currentGround.TryGetComponent(out PlatformEffector2D platform))
            {
                StartCoroutine(TemporaryNonCollision(platform?.gameObject.GetComponent<BoxCollider2D>(), col, 0.5f));
            }
        }
    }

    private IEnumerator TemporaryNonCollision(Collider2D col1, Collider2D col2, float duration)
    {
        Physics2D.IgnoreCollision(col1, col2);

        float end = Time.time + duration;
        while (Time.time < end)
        {
            if (col1 == null || col2 == null)
            {
                yield break;
            }

            yield return null;
        }

        if (Physics2D.GetIgnoreCollision(col1, col2))
        {
            Physics2D.IgnoreCollision(col1, col2, false);
        }
    }

    public virtual void Dodge(InputAction.CallbackContext ctx)
    {
        //you can dash through platforms...
        if (ctx.performed)
        {
            StartCoroutine(Dash());
        }
    }

    public virtual IEnumerator Dash()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (Vector3)(pc.aim * dashDistance);
        float end = Time.time + dashFailTime;
        while (Time.time < end || transform.position != endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, dashSpeed);
            if (Vector3.Distance(transform.position, endPos) < 0.0025f)
            {
                break;
            }
            yield return null;
        }

        rb.velocity = Vector2.zero;
        rb.AddForce(pc.aim * dashSpeed, ForceMode2D.Impulse);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = GroundedCollision(collision);
        currentGround = collision.gameObject;

        if (isGrounded)
        {
            jumpCount = 0;
            rb.velocity *= 0.8f;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        currentGround = null;

        if (GroundedCollision(collision))
        {
            isGrounded = false;
        }

        if (jumpCount == 0)
        {
            jumpCount++;
        }
    }

    protected virtual bool GroundedCollision(Collision2D collision)
    {
        bool correctTag = collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player") ? true : false;
        return (collision.gameObject.transform.position.y - groundedOffset < transform.position.y && correctTag) ? true : false;
    }

    //void Test(PlayerInput pi)
    //{
    //    Debug.Log(pi.playerIndex);
    //}

    //private void OnEnable()
    //{
    //    //controls.Player.Enable();
    //    controls.Enable();
    //}

    //private void OnDisable()
    //{
    //    //controls.Player.Disable();
    //    controls.Disable();
    //}
}
