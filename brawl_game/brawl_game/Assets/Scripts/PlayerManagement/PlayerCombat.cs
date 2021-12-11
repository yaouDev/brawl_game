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
    [SerializeField] protected float lineTime = 0.25f;
    [SerializeField] protected float lightAttackSpeed = 1f;
    [SerializeField] protected float heavyAttackSpeed = 2f;
    [SerializeField] protected float specialAttackSpeed = 3f;
    [SerializeField] protected float enemyGroundLift = 0.1f;
    protected float lightAttackTimer;
    protected float heavyAttackTimer;
    protected float specialAttackTimer;
    protected CharacterSound cs;

    [Header("Projectile combat")]
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected GameObject bomb;
    public float bombLaunchForce = 50f;

    protected virtual void Awake()
    {
        pc = transform.parent.gameObject.GetComponent<PlayerControls>();
    }

    private void Start()
    {
        cs = GetComponent<CharacterSound>();
    }

    protected virtual void FixedUpdate()
    {
        Timers();
    }

    protected void Timers()
    {
        if (lightAttackTimer > 0f) lightAttackTimer -= Time.fixedDeltaTime;
        if (heavyAttackTimer > 0f) heavyAttackTimer -= Time.fixedDeltaTime;
        if (specialAttackTimer > 0f) specialAttackTimer -= Time.fixedDeltaTime;
    }

    public virtual void LightAttack(InputAction.CallbackContext ctx) { NotImplementedMessage(ctx); }

    public virtual void HeavyAttack(InputAction.CallbackContext ctx) { NotImplementedMessage(ctx); }

    public virtual void SpecialAttack(InputAction.CallbackContext ctx) { NotImplementedMessage(ctx); }

    private void NotImplementedMessage(InputAction.CallbackContext ctx) { Debug.LogWarning(ctx.action.name + " is not implemented"); }

    protected void Attack_Raycast(float range, float hitForce, bool draw)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, pc.aim, range);

        Vector2 endPos = hit.collider ? hit.point : (Vector2)transform.position + (pc.aim * range);
        if (draw) StartCoroutine(ShootLineRenderer(endPos));

        GameObject parent = hit.collider?.gameObject.transform.parent?.gameObject;

        if (parent != null)
        {
            if (parent.TryGetComponent(out PlayerState player))
            {
                Transform child = player.transform.GetChild(0);
                child.position = new Vector3(child.position.x, child.position.y + enemyGroundLift);

                Vector2 force = -hit.normal * hitForce;
                player.TakeHit(force);
            }
        }

        Debug.Log(hit.collider?.gameObject.name + " was hit by " + gameObject.name);
    }

    protected void Attack_Raycast(float range, float hitForce)
    {
        Attack_Raycast(range, hitForce, true);
    }

    protected IEnumerator ShootLineRenderer(Vector3 endPos)
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

    protected virtual Projectile Attack_Instantiate()
    {
        Projectile projectileInstance = Instantiate(projectile, transform.position + ((Vector3)pc.aim * 3f), Quaternion.identity).GetComponent<Projectile>();
        projectileInstance.shootDirection = pc.aim;

        projectileInstance.SetOwner(gameObject);

        return projectileInstance;
    }

    protected void Attack_Bomb()
    {
        Bomb bombInstance = Instantiate(bomb, transform.position + ((Vector3)pc.aim * 3f), Quaternion.identity).GetComponent<Bomb>();

        bombInstance.Launch(pc.aim * bombLaunchForce);
    }

    protected void AttackCooldown(AttackType attackType)
    {
        //casting an attack doesnt take time. there is cooldown before being able to use an individual ability again.
        //lightAttackSpeed is effectively a "default" cooldown
        switch (attackType)
        {
            case AttackType.light:
                heavyAttackTimer = lightAttackSpeed;
                specialAttackTimer = lightAttackSpeed;
                break;
            case AttackType.heavy:
                heavyAttackTimer = heavyAttackSpeed;
                specialAttackTimer = lightAttackSpeed;
                break;
            case AttackType.special:
                heavyAttackTimer = lightAttackSpeed;
                specialAttackTimer = specialAttackSpeed;
                break;
            default:
                break;
        }
        //lightAttack always has the same cooldown
        lightAttackTimer = lightAttackSpeed;
    }

    protected void PlayAttackSound(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.light: AudioManager.instance.Play("LightAttack", false, transform.position, cs.characterSounds); break;
            case AttackType.heavy: AudioManager.instance.Play("HeavyAttack", false, transform.position, cs.characterSounds); break;
            case AttackType.special: AudioManager.instance.Play("SpecialAttack", false, transform.position, cs.characterSounds); break;
            default: break;
        }
    }

    protected bool CanAttack(InputAction.CallbackContext ctx, AttackType attacktype)
    {
        switch (attacktype)
        {
            case AttackType.light: return ctx.performed && lightAttackTimer <= 0f;
            case AttackType.heavy: return ctx.performed && heavyAttackTimer <= 0f;
            case AttackType.special: return ctx.performed && specialAttackTimer <= 0f;
            default: return true;
        }
    }
}

public enum AttackType
{
    light, heavy, special
}
