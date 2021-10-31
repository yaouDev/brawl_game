using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputMaster controls;
    [SerializeField] private float movementSpeed;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Test.performed += ctx => Test();
    }

    private void Update()
    {
        
    }

    void Test()
    {
        Debug.Log("test");
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
