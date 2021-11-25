using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    private void CheckCollision(Collider2D collision)
    {
        if (collision.gameObject.transform.parent == null || collision.gameObject.TryGetComponent(out Deathbox buddy)) return;

        print(collision.gameObject);

        if (collision.gameObject.transform.parent.gameObject.TryGetComponent(out PlayerState player))
        {
            player.Die();
        }
        else if (collision.gameObject.TryGetComponent(out Bomb bomb))
        {
            //doesnt work because bomb is dynamic rb and deathbox is kinematic rb?
            bomb.Explode();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
