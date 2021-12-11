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
    public GameObject shotgunBlast;

    [Header("Sniper")]
    public float sniperMaxCharge = 2f;
    private float sniperCharge;
    private bool sniperHold;
    public float sniperForce = 10f;
    public float maxLineWidth = 1f;

    public override void LightAttack(InputAction.CallbackContext ctx)
    {
        if (!CanAttack(ctx, AttackType.light) || sniperHold)
        {
            return;
        }

        Shotgun();
        PlayAttackSound(AttackType.light);
        AttackCooldown(AttackType.light);
    }

    public override void HeavyAttack(InputAction.CallbackContext ctx)
    {
        if (!CanAttack(ctx, AttackType.heavy))
        {
            print("on cooldown");
            return;
        }

        if (!sniperHold)
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

    public override void SpecialAttack(InputAction.CallbackContext ctx)
    {
        if (!CanAttack(ctx, AttackType.special) || sniperHold)
        {
            return;
        }

        base.SpecialAttack(ctx);
    }

    private void Shotgun()
    {
        Instantiate(shotgunBlast, (Vector2)transform.position + pc.aim,  pc.GetAngle() * Quaternion.Euler(0f, 0f, -90f));

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

        LRData startData = SetSniperColor();

        Color targetColor = Color.red;

        float end = Time.time + sniperMaxCharge;
        while (Time.time < end && sniperHold)
        {
            sniperCharge += Time.deltaTime;

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, (Vector2)transform.position + pc.aim * longRange);
            lineRenderer.startWidth += Time.deltaTime * (sniperCharge / sniperMaxCharge) * maxLineWidth;
            lineRenderer.endWidth += Time.deltaTime * (sniperCharge / sniperMaxCharge) * maxLineWidth;

            lineRenderer.startColor = Color.Lerp(startData.cx, targetColor, sniperCharge / sniperMaxCharge);
            lineRenderer.endColor = Color.Lerp(startData.cy, targetColor, sniperCharge / sniperMaxCharge);

            yield return null;
        }

        if (sniperCharge >= sniperMaxCharge)
        {
            Attack_Raycast(longRange, sniperForce, false);
            AttackCooldown(AttackType.heavy);
            PlayAttackSound(AttackType.heavy);
            sniperCharge = 0f;
            sniperHold = false;
        }

        //reset linerenderer
        lineRenderer.startColor = startData.cx;
        lineRenderer.endColor = startData.cy;

        lineRenderer.startWidth = startData.wx;
        lineRenderer.endWidth = startData.wy;

        lineRenderer.enabled = false;
    }

    private LRData SetSniperColor()
    {
        LRData startSpecs = new LRData(lineRenderer.startColor, lineRenderer.endColor, lineRenderer.startWidth, lineRenderer.endWidth);
        lineRenderer.startColor = new Color(255, 255, 255, 0);
        lineRenderer.endColor = new Color(255, 255, 255, 0);
        return startSpecs;
    }
}

public struct LRData
{
    public readonly Color cx;
    public readonly Color cy;

    public readonly float wx;
    public readonly float wy;

    public LRData(Color x, Color y, float a, float b)
    {
        cx = x;
        cy = y;
        wx = a;
        wy = b;
    }
}
