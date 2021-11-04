using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    public GameObject character;

    [Header("Read Only")]
    public List<GameObject> entered = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        entered.Add(go);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach(GameObject go in entered)
        {
            if (go.TryGetComponent(out MenuPlayer player))
            {
                if (go.TryGetComponent(out CharacterSelection cs) && player.isSelecting)
                {
                    cs.selected = character;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        if (entered.Contains(go))
        {
            entered.Remove(go);
        }
    }
}
