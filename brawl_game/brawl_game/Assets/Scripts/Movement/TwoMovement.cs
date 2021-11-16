using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TwoMovement : PlayerMovement
{
    [Header("Wreckingball related")]
    public float pushMultiplier = 1f;
    public float velocityThreshold = 50f;
    public float massWhileGrappled = 3f;
    public float gravityWhileGrappled = 1f;
    public float slamIgnoreCollisionTime = 0.25f;

    [Header("Read Only - Ball")]
    public bool grappled;
    public bool canPush;
    [SerializeField] private float originalMass;
    [SerializeField] private float originalGravity;
    public bool freezeBecauseSlam;
    public bool freezeBecauseThrow;
    private TwoCombat combat;
    private Trajectory trajectory;

    protected override void Awake()
    {
        base.Awake();

        originalMass = rb.mass;
        originalGravity = rb.gravityScale;
        trajectory = GetComponent<Trajectory>();
    }

    private void Start()
    {
        combat = (TwoCombat)pc.combat;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //print(rb.velocity.magnitude);

        if (grappled && rb.mass != massWhileGrappled && rb.gravityScale != gravityWhileGrappled)
        {
            rb.mass = massWhileGrappled;
            rb.gravityScale = gravityWhileGrappled;
        }
    }

    public override void Move(InputAction.CallbackContext ctx)
    {
        //is set in combat
        if (freezeBecauseSlam || freezeBecauseThrow) return;
        base.Move(ctx);
    }

    //public override void Jump(InputAction.CallbackContext ctx)
    //{
    //    base.Jump(ctx);

    //    if (ctx.performed && CanJump())
    //    {
    //        combat.DisableGrapple();
    //    }
    //}

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (canPush)
        {
            if (!collision.gameObject.CompareTag("Ground"))
            {
                if (collision.gameObject.TryGetComponent(out Rigidbody2D other))
                {
                    ContactPoint2D point = collision.GetContact(0);
                    Vector2 pushPoint = point.point - (Vector2)transform.position;
                    //Vector2 force = point.normalImpulse * (rb.velocity * rb.mass) * pushMultiplier;
                    Vector2 force = (point.normalImpulse * pushMultiplier) * pushPoint.normalized;

                    List<Collider2D> results = new List<Collider2D>();
                    if (other.GetAttachedColliders(results) > 0)
                    {
                        StartCoroutine(TemporaryIgnoreCollision(results, slamIgnoreCollisionTime));
                    }

                    Debug.Log($"Attempting to push {other.gameObject.name} by {force} at {pushPoint}!");
                    other.AddForce(force, ForceMode2D.Impulse);
                    rb.velocity = Vector2.zero;
                }
            }
        }

        GrappleCheck();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GrappleCheck();
    }

    private void GrappleCheck()
    {
        if (!grappled)
        {
            canPush = false;
            ResetRigidbody();
            freezeBecauseThrow = false;
            trajectory.ToggleTrail(false);
            //rb.sharedMaterial.bounciness = 0f;
        }
    }


    private IEnumerator TemporaryIgnoreCollision(List<Collider2D> colliders, float duration)
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            Physics2D.IgnoreCollision(col, colliders[i]);
        }

        float end = Time.time + duration;
        while(Time.time < end)
        {
            yield return null;
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            if(colliders[i] != null && Physics2D.GetIgnoreCollision(col, colliders[i]))
            {
                Physics2D.IgnoreCollision(col, colliders[i], false);
            }
        }
    }

    public void ResetRigidbody()
    {
        rb.mass = originalMass;
        rb.gravityScale = originalGravity;
    }
}
