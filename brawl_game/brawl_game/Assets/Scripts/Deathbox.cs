using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent == null || collision.gameObject.TryGetComponent(out Deathbox buddy)) return;

        if(collision.gameObject.transform.parent.gameObject.TryGetComponent(out PlayerState player))
        {
            player.Die();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
