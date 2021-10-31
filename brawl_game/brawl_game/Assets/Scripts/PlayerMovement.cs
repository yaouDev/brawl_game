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
        controls.Player.Movement.performed += context => MoveTest();
    }

    private void Update()
    {
        
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        Debug.Log("Move: " + ctx.ReadValue<Vector2>());
    }

    void Test()
    {
        Debug.Log("test");
    }

    void MoveTest()
    {
        Debug.Log("yo");
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
