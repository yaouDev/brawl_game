using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    protected PlayerControls pc;

    //public event Action onAttack;

    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected int baseDamage = 1;
    [SerializeField] protected float shortRange = 1f;
    [SerializeField] protected float longRange = 20f;
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected float lineTime = 0.25f;
    protected float attackTimer;

    [Header("Projectile combat")]
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected GameObject bomb;
    public float bombLaunchForce = 50f;

    private void Awake()
    {
        pc = transform.parent.gameObject.GetComponent<PlayerControls>();
    }

    private void FixedUpdate()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.fixedDeltaTime;
        }
    }

    public virtual void LightAttack(InputAction.CallbackContext ctx)
    {
        if (CanAttack(ctx))
        {
            Attack_Raycast(longRange);
        }
    }

    public virtual void HeavyAttack(InputAction.CallbackContext ctx)
    {
        if (CanAttack(ctx))
        {
            Attack_Bomb();
        }
    }

    public virtual void SpecialAttack(InputAction.CallbackContext ctx)
    {
        if (CanAttack(ctx))
        {
            Attack_Instantiate();
        }
    }

    protected virtual void Attack_Raycast(float range)
    {
        attackTimer = attackSpeed;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, pc.aim, range);

        //if hit, shoots at center point !fix!
        Vector3 endPos = hit.transform?.position ?? transform.position + ((Vector3)pc.aim * range);
        StartCoroutine(ShootLineRenderer(endPos));

        GameObject parent = hit.collider?.gameObject.transform.parent?.gameObject;

        if (parent != null)
        {
            if(parent.TryGetComponent(out PlayerState player)) player.Die();
        }

        Debug.Log(hit.collider?.gameObject.name + " was hit by " + gameObject.name);
    }

    private IEnumerator ShootLineRenderer(Vector3 endPos)
    {
        lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, transform.position + (Vector3)pc.aim);
        lineRenderer.SetPosition(1, endPos);

        float end = Time.time + lineTime;
        while (Time.time < end)
        {
            //fade and stuff maybe
            yield return null;
        }

        lineRenderer.enabled = false;
    }

    protected virtual void Attack_Instantiate()
    {
        attackTimer = attackSpeed;
        Projectile projectileInstance = Instantiate(projectile, transform.position + ((Vector3)pc.aim * 3f), Quaternion.identity).GetComponent<Projectile>();
        projectileInstance.shootDirection = pc.aim;
    }

    protected virtual void Attack_Bomb()
    {
        attackTimer = attackSpeed;
        Bomb bombInstance = Instantiate(bomb, transform.position + ((Vector3)pc.aim * 3f), Quaternion.identity).GetComponent<Bomb>();

        bombInstance.Launch(pc.aim * bombLaunchForce);
    }

    protected bool CanAttack(InputAction.CallbackContext ctx)
    {
        return ctx.performed && attackTimer <= 0f;
    }
}
