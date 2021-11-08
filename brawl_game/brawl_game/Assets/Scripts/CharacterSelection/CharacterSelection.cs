using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelection : MonoBehaviour
{
    [Header("Read Only")]
    public GameObject selected;

    public void Select(GameObject character)
    {
        selected = character;
    }
}
