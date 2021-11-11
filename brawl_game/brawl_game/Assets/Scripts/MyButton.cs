using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class MyButton : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick = new UnityEvent();

    [SerializeField] private bool colliderFitToSize;

    private BoxCollider2D col;

    [Header("Read Only")]
    public List<GameObject> entered = new List<GameObject>();

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();

        if (!col.isTrigger)
        {
            col.isTrigger = true;
        }

        if (colliderFitToSize)
        {
            RectTransform rt = GetComponent<RectTransform>();
            col.size = new Vector2(rt.rect.width, rt.rect.height);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        entered.Add(go);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (GameObject go in entered)
        {
            if (go.TryGetComponent(out MenuPlayer player))
            {
                if (player.isSelecting)
                {
                    //do action
                    //new script that overrides trigger to access hovered objects, otherwise use unity events
                    Trigger(go);
                    onClick?.Invoke();
                }
            }
        }
    }

    public virtual void Trigger(GameObject go) { }

    private void OnTriggerExit2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        if (entered.Contains(go))
        {
            entered.Remove(go);
        }
    }
}
