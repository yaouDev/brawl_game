using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnablePlatform : MonoBehaviour
{
    public float killtime = 5f;

    private void Awake()
    {
        Invoke("Remove", killtime);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Remove();
    }

    public void Remove()
    {
        //sound and effects

        Destroy(gameObject);
    }
}
