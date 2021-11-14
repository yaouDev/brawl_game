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
    [SerializeField] protected GameObject owner;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        Destroy(gameObject, deathTimer);
    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = shootDirection * projectileVelocity;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject parent = collision.gameObject.transform.parent.gameObject;

        if(parent != null)
        {
            if (parent.TryGetComponent(out PlayerState player))
            {
                //add damage
                player.Die();
            }
        }

        //sound and effects

        Destroy(gameObject);
    }

    public void SetOwner(GameObject go)
    {
        owner = go;
    }
}
