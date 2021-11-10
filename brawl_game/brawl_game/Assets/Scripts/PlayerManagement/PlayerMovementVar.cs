using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementVar : PlayerMovement
{
    public override void Dodge(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("PlayerMovementVar is working!");
        }
    }
}
