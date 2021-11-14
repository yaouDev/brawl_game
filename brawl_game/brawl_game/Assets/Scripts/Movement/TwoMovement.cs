using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoMovement : PlayerMovement
{
    [Header("Wreckingball related")]
    public float pushForce = 1f;
    public float velocityThreshold = 50f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //print(rb.velocity.magnitude);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        TwoCombat twoCombat = (TwoCombat)pc.combat;

        if (twoCombat.grappled || rb.velocity.magnitude > velocityThreshold)
        {
            if (!collision.gameObject.CompareTag("Ground"))
            {
                if (collision.gameObject.TryGetComponent(out Rigidbody2D other))
                {
                    Vector2 force = pc.aim * rb.velocity.magnitude * pushForce;
                    Debug.Log($"Attempting to push {other.gameObject.name} by {force}!");
                    other.AddForce(force);
                }
            }
        }
    }
}
