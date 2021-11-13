using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private void Awake()
    {
        instance ??= this;
    }

    public event Action test;
    public event Action onPlayerDeath;

    public void Test()
    {
        if (test != null) test.Invoke();
    }

    public void OnPlayerDeath_Action()
    {
        if (onPlayerDeath != null) onPlayerDeath.Invoke();
    }
}
