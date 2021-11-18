using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OphCombat : PlayerCombat
{
    //make a character with an aim that goes further away the longer the player presses

    [Header("Shotgun")]
    public float firstHitForce = 15f;
    public float subHitForce = 3f;
    public float rangeOffset = 0.5f;
    public int numberOfShots = 5;

    [Header("Sniper")]
    private float sniperCharge;
    public float sniperMaxCharge = 2f;
    private bool sniperHold;
    public float sniperForce = 10f;

    public override void LightAttack(InputAction.CallbackContext ctx)
    {
        //shotgun!
        if(CanAttack(ctx, AttackType.light))
        {
            Shotgun();
        }
    }

    public override void HeavyAttack(InputAction.CallbackContext ctx)
    {
        if(CanAttack(ctx, AttackType.heavy) && !sniperHold)
        {
            sniperHold = true;
            StartCoroutine(SniperShot());
        }

        if (ctx.canceled)
        {
            sniperCharge = 0f;
            sniperHold = false;
        }
    }

    private void Shotgun()
    {
        //raycast 5 rays on varying degrees

        //1
        //1.25
        //0.75
        //1.5
        //0.5

        //make the shots appropriate to the aim
        //the middle ray is not centered.

        float spread1 = pc.aim.y + rangeOffset;
        float spread2 = pc.aim.y - rangeOffset;
        //float spread1 = pc.aim.x - rangeOffset;
        //float spread2 = pc.aim.x + rangeOffset;
        float difference = spread1 - spread2;
        float increment = difference / numberOfShots;

        float currentSpread = spread1;

        bool firstHit = true;

        for (int i = 0; i < numberOfShots; i++)
        {
            Vector2 direction = new Vector2(pc.aim.x, currentSpread);
            //Vector2 direction = new Vector2(currentSpread, pc.aim.y);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, shortRange);

            Debug.DrawRay(transform.position, direction * shortRange, Color.red, 1f);

            GameObject parent = hit.collider?.gameObject.transform.parent?.gameObject;
            if (parent != null && parent.TryGetComponent(out PlayerState player))
            {
                float hitForce = firstHit ? firstHitForce : subHitForce;
                Transform child = player.transform.GetChild(0);
                child.position = new Vector3(child.position.x, child.position.y + enemyGroundLift);

                Vector2 force = new Vector2(-hit.normal.x, hit.normal.y) * hitForce;
                player.TakeHit(force);

                firstHit = false;
            }

            currentSpread -= increment;
        }
    }

    private IEnumerator SniperShot()
    {
        //shoot projectile?

        lineRenderer.enabled = true;

        float lrws = lineRenderer.startWidth;
        float lrwe = lineRenderer.endWidth;
        lineRenderer.startWidth = 0f;
        lineRenderer.endWidth = 0f;
        

        float end = Time.time + sniperMaxCharge;
        while(Time.time < end && sniperHold)
        {
            sniperCharge += Time.deltaTime;

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, (Vector2)transform.position + pc.aim * longRange);
            lineRenderer.startWidth += Time.deltaTime * sniperCharge;
            lineRenderer.endWidth += Time.deltaTime * sniperCharge;
            
            yield return null;
        }

        if(sniperCharge >= sniperMaxCharge)
        {
            Attack_Raycast(longRange, sniperForce, false);
            AttackCooldown(AttackType.heavy);
        }

        lineRenderer.startWidth = lrws;
        lineRenderer.endWidth = lrwe;

        lineRenderer.enabled = false;
    }
}
