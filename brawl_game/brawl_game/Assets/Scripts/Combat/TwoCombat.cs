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

    [Header("Read Only")]
    [SerializeField] private GameObject shotHook;
    public bool grappled;

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
            grappleLineRenderer.enabled = false;
            grappleLineRenderer.SetPosition(1, transform.position);
        }
    }

    public override void HeavyAttack(InputAction.CallbackContext ctx)
    {

        //MAKE HIM LIKE THAT HAMSTER FROM OVERWATCH?!?!?
        //hook

        if (ctx.performed && CanAttack(ctx, AttackType.heavy) && shotHook == null)
        {
            shotHook = Attack_Instantiate().gameObject;
            grappleLineRenderer.enabled = true;
        }

        if (ctx.canceled && shotHook != null)
        {
            AttackCooldown(AttackType.heavy);

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
