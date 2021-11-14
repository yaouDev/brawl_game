using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Projectile
{
    [Header("Read Only")]
    [SerializeField] private GameObject grappledObject;

    protected override void FixedUpdate()
    {
        if (grappledObject == null)
        {
            base.FixedUpdate();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        grappledObject = collision.gameObject;
        rb.velocity = Vector2.zero;

        if(owner.TryGetComponent(out TwoCombat twoCombat))
        {
            twoCombat.grappled = true;
        }

        if (owner.TryGetComponent(out SpringJoint2D sj))
        {
            sj.enabled = true;
            //sj.connectedAnchor = transform.position;
            sj.connectedBody = rb;
        }
    }

    public void Kill()
    {
        

        //sound and effects

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(owner != null)
        {
            if (owner.TryGetComponent(out SpringJoint2D sj))
            {
                //sj.connectedAnchor = Vector2.zero;
                sj.connectedBody = null;
                sj.enabled = false;
            }

            if (owner.TryGetComponent(out TwoCombat twoCombat))
            {
                twoCombat.grappled = false;
            }
        }
    }
}
