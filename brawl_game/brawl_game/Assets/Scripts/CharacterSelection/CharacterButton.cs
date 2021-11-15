using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterButton : MyButton
{
    public GameObject character;

    private void Start()
    {
        if(character != null)
        {
            TMP_Text text = GetComponentInChildren<TMP_Text>();
            text.text = character.name;
        }
    }
    public override void Trigger(GameObject go)
    {
        if (go.transform.parent.TryGetComponent(out CharacterSelection cs))
        {
            cs.selected = character;
        }
    }
}
