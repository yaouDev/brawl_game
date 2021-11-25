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
    public GameObject explosion;

    private float timer;
    private bool hasExploded;
    private bool isGrounded;
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
        isGrounded = true;

        //stop rolling
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity *= 0.8f;
        }
        else
        {
            timer = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    public void Explode()
    {
        //effects and sound
        Instantiate(explosion, transform.position, Quaternion.identity);
        AudioManager.instance.Play("Explosion");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);


        //doesnt really affect player if grounded?
        foreach (Collider2D hit in hits)
        {
            Rigidbody2D rbhit = hit.GetComponent<Rigidbody2D>();

            if(rbhit != null)
            {
                Vector3 relativePosition = hit.transform.position - transform.position;

                //check if relativePosition.y negative and (player?) is grounded, if yes invert
                if(relativePosition.y <= 0f && isGrounded)
                {
                    relativePosition = new Vector3(relativePosition.x, -relativePosition.y);
                }

                rbhit.velocity = Vector2.zero;
                rbhit?.AddForce(relativePosition.normalized * force, ForceMode2D.Impulse);

                GameObject parent = hit.gameObject.transform.parent?.gameObject;

                if (parent == null) continue;

                if (parent.TryGetComponent(out PlayerState player))
                {
                    //damage
                    //player.Die();
                }
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
