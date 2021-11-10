using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MyButton
{
    public GameObject character;

    public override void Trigger(GameObject go)
    {
        if(go.transform.parent.TryGetComponent(out CharacterSelection cs))
        {
            cs.selected = character;
        }
    }
}
