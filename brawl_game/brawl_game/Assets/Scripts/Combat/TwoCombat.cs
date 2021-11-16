using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TwoCombat : PlayerCombat
{
    [Header("Platform related")]
    [SerializeField] private GameObject spawnablePlatform;
    [SerializeField] private Vector3 offset;

    [Header("Grapple related")]
    [SerializeField] private LineRenderer grappleLineRenderer;

    [Header("Slam related")]
    public float maxSlamTime = 1f;
    public float slamForce = 50f;
    private float slamHold;
    private bool isHolding;
    private bool canSlam;
    //public float bounciness = 6f;

    [Header("Read Only")]
    [SerializeField] private GameObject shotHook;
    private Rigidbody2D rb;
    private TwoMovement movement;

    private Trajectory trajectory;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        trajectory = GetComponent<Trajectory>();
    }

    private void Start()
    {
        movement = (TwoMovement)pc.movement;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(shotHook != null && grappleLineRenderer.enabled)
        {
            grappleLineRenderer.SetPosition(0, transform.position);
            grappleLineRenderer.SetPosition(1, shotHook.transform.position);
        }
        else if(shotHook == null && grappleLineRenderer.enabled)
        {
            DisableGrapple();
            movement.freezeBecauseThrow = true;
        }

        if(slamHold < maxSlamTime && isHolding)
        {
            slamHold += Time.fixedDeltaTime;
        }
    }

    public void DisableGrapple()
    {
        grappleLineRenderer.enabled = false;
        grappleLineRenderer.SetPosition(1, transform.position);
        GetComponent<SpringJoint2D>().enabled = false;
    }

    public override void LightAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canSlam)
        {
            isHolding = true;
            StartCoroutine(Slam());
            canSlam = false;
            movement.freezeBecauseSlam = true;
        }

        if (ctx.canceled)
        {
            isHolding = false;
            slamHold = 0f;
            canSlam = true;
            movement.freezeBecauseSlam = false;
        }
    }

    private IEnumerator Slam()
    {
        movement.ResetRigidbody();
        Vector3 initPos = transform.position;
        rb.velocity = Vector2.zero;

        float end = Time.time + maxSlamTime;
        while(Time.time < end && isHolding)
        {
            transform.position = initPos;
            yield return null;
        }

        movement.canPush = true;
        rb.AddForce(Vector2.down * slamForce * (slamHold + 1f), ForceMode2D.Impulse);
    }

    public override void HeavyAttack(InputAction.CallbackContext ctx)
    {
        //hook

        if (ctx.performed && CanAttack(ctx, AttackType.heavy) && shotHook == null)
        {
            shotHook = Attack_Instantiate().gameObject;
            grappleLineRenderer.enabled = true;
            //---
            //rb.sharedMaterial.bounciness = bounciness;
            //---

            trajectory.FreezePoints(false);
            trajectory.ToggleTrail(true);
        }

        if (ctx.canceled && shotHook != null)
        {
            AttackCooldown(AttackType.heavy);

            trajectory.FreezePoints(true);

            if (shotHook != null)
            {
                shotHook.GetComponent<Hook>().Kill();
                shotHook = null;
                grappleLineRenderer.enabled = false;
            }
        }

        //if (ctx.canceled)
        //{
        //    AttackCooldown(AttackType.heavy);

        //    if (shotHook != null)
        //    {
        //        shotHook.GetComponent<Hook>().Kill();
        //        shotHook = null;
        //        grappleLineRenderer.enabled = false;
        //    }
        //}
    }

    public override void SpecialAttack(InputAction.CallbackContext ctx)
    {
        //spawn platform

        if(CanAttack(ctx, AttackType.special))
        {
            AttackCooldown(AttackType.special);
            SpawnPlatform();
        }
    }

    private void SpawnPlatform()
    {
        Instantiate(spawnablePlatform, transform.position + offset, Quaternion.identity);
    }
}
