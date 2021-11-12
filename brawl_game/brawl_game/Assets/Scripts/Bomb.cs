using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    public float radius = 5f;
    public float delay = 3f;
    public float force = 10f;

    public Image visualTimer;

    private float timer;
    private bool hasExploded;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        timer = delay;
    }

    public void Launch(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (timer > 0f && !hasExploded)
        {
            timer -= Time.fixedDeltaTime;
            visualTimer.fillAmount = timer / delay;
        }
        else
        {
            Explode();
            hasExploded = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity *= 0.8f;
        }
        else
        {
            timer = 0f;
        }
    }

    public void Explode()
    {
        //effects and sound

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            Rigidbody2D rbhit = hit.GetComponent<Rigidbody2D>();

            Vector3 relativePosition = hit.transform.position - transform.position;

            rbhit.velocity = Vector2.zero;
            rbhit?.AddForce(relativePosition * force, ForceMode2D.Impulse);

            if (hit.gameObject.TryGetComponent(out PlayerState player))
            {
                //damage
                //player.Die();
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
