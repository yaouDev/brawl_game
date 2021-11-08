using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int playerId { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetId(int id)
    {
        playerId = id;
    }
}
