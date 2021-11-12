using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileVelocity = 5f;
    public float deathTimer = 10f;

    [Header("Read Only")]
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public CapsuleCollider2D col;
    public Vector3 shootDirection;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        Destroy(gameObject, deathTimer);
    }

    private void FixedUpdate()
    {
        rb.velocity = shootDirection * projectileVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerState player))
        {
            //add damage
            player.Die();
        }

        Destroy(gameObject);
    }
}
