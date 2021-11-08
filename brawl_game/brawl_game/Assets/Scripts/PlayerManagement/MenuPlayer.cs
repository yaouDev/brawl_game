using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPlayer : MonoBehaviour
{
    public float moveSpeed = 8f;
    private bool moving;
    private Vector2 direction;

    private bool isKeyboard;

    [Header("Read Only")]
    public bool isSelecting;
    public bool isBacking;

    //private InputMaster controls;

    //---

    //public InputMaster controls;

    //write a script that acts as a middle man on the player object that gives appropriate actions to an instansiated script?

    private void Awake()
    {
        isKeyboard = transform.parent.GetComponent<PlayerInput>().currentControlScheme == "Keyboard";

        //controls = new InputMaster();
        //controls.MenuPlayer.Select.performed += ctx => Select();
        //controls.MenuPlayer.Select.canceled += ctx => Select();
        //controls.MenuPlayer.Move.performed += ctx => Move(ctx.ReadValue<Vector2>(), true);
        //controls.MenuPlayer.Move.canceled += ctx => Move(ctx.ReadValue<Vector2>(), false);
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            if (isKeyboard)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(direction);
                transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
            }
            else
            {
                transform.position += (Vector3)direction * Time.deltaTime * moveSpeed;
            }
        }
    }

    public void MoveUE(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();

        moving = ctx.performed;
    }

    public void SelectUE(InputAction.CallbackContext ctx)
    {
        isSelecting = ctx.performed;
    }

    public void BackUE(InputAction.CallbackContext ctx)
    {
        isBacking = ctx.performed;
    }

    //public void Move(Vector2 dir, bool move)
    //{
    //    direction = dir;

    //    moving = move;
    //}

    //public void Select()
    //{
    //    isSelecting = !isSelecting;
    //}

    //private void OnEnable()
    //{
    //    //controls.MenuPlayer.Enable();
    //    controls.Enable();
    //}

    //private void OnDisable()
    //{
    //    //controls.MenuPlayer.Disable();
    //    controls.Disable();
    //}
}
