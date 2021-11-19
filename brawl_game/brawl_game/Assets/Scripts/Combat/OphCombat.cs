using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OphCombat : PlayerCombat
{
    //make a character with an aim that goes further away the longer the player presses

    [Header("Shotgun")]
    public float shotgunForce = 30f;
    public ShotgunHitbox shotgun;

    [Header("Sniper")]
    public float sniperMaxCharge = 2f;
    private float sniperCharge;
    private bool sniperHold;
    public float sniperForce = 10f;
    public float maxLineWidth = 1f;

    public override void LightAttack(InputAction.CallbackContext ctx)
    {
        //shotgun!
        if (CanAttack(ctx, AttackType.light))
        {
            Shotgun();
        }
    }

    public override void HeavyAttack(InputAction.CallbackContext ctx)
    {
        if (CanAttack(ctx, AttackType.heavy) && !sniperHold)
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
        foreach (GameObject go in shotgun.entered)
        {
            if (go.TryGetComponent(out PlayerCombat player))
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, go.transform.position - transform.position, Mathf.Infinity);
                Debug.DrawLine(transform.position, hit.point, Color.red, 2f);

                if (hit.transform.gameObject.TryGetComponent(out PlayerCombat character))
                {
                    float distance = Vector2.Distance(transform.position, hit.point);

                    character.transform.position += enemyGroundLift * Vector3.one;

                    GameObject parent = player.transform.parent.gameObject;
                    parent.GetComponentInParent<PlayerState>().TakeHit(-hit.normal * (shotgunForce / distance));
                }
            }
        }
    }

    private IEnumerator SniperShot()
    {
        //shoot projectile?

        //linerenderer
        lineRenderer.enabled = true;

        float lrws = lineRenderer.startWidth;
        float lrwe = lineRenderer.endWidth;
        lineRenderer.startWidth = 0f;
        lineRenderer.endWidth = 0f;
        Color lrsc = lineRenderer.startColor;
        Color lrec = lineRenderer.endColor;
        lineRenderer.startColor = new Color(255, 255, 255, 0);
        lineRenderer.endColor = new Color(255, 255, 255, 0);

        Color targetColor = Color.red;

        float end = Time.time + sniperMaxCharge;
        while (Time.time < end && sniperHold)
        {
            sniperCharge += Time.deltaTime;

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, (Vector2)transform.position + pc.aim * longRange);
            lineRenderer.startWidth += Time.deltaTime * (sniperCharge / sniperMaxCharge) * maxLineWidth;
            lineRenderer.endWidth += Time.deltaTime * (sniperCharge / sniperMaxCharge) * maxLineWidth;

            lineRenderer.startColor = Color.Lerp(lrsc, targetColor, sniperCharge / sniperMaxCharge);
            lineRenderer.endColor = Color.Lerp(lrec, targetColor, sniperCharge / sniperMaxCharge);

            yield return null;
        }

        if (sniperCharge >= sniperMaxCharge)
        {
            Attack_Raycast(longRange, sniperForce, false);
            AttackCooldown(AttackType.heavy);
        }

        //reset linerenderer
        lineRenderer.startColor = lrsc;
        lineRenderer.endColor = lrec;

        lineRenderer.startWidth = lrws;
        lineRenderer.endWidth = lrwe;

        lineRenderer.enabled = false;
    }
}
