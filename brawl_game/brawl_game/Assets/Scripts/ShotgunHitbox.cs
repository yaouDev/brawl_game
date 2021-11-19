using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunHitbox : MonoBehaviour
{
    public List<GameObject> entered = new List<GameObject>();
    private PlayerControls pc;
    private void Start()
    {
        pc = transform.parent?.parent?.GetComponent<PlayerControls>();
    }

    private void FixedUpdate()
    {
        gameObject.transform.localRotation = pc.GetAngle();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        entered.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(entered.Contains(collision.gameObject)) entered.Remove(collision.gameObject);
    }
}
