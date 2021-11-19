using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonkiMonkMovement : PlayerMovement
{
    private MonkiMonkCombat monkiCombat;

    private void Start()
    {
        monkiCombat = GetComponent<MonkiMonkCombat>();
    }

    public override void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            moving = false;
        }

        if (monkiCombat.isPraying) return;
        base.Move(ctx);
    }

    public override void Dodge(InputAction.CallbackContext ctx)
    {
        if (monkiCombat.isPraying) return;
        base.Dodge(ctx);
    }

    public override void Jump(InputAction.CallbackContext ctx)
    {
        if (monkiCombat.isPraying) return;
        base.Jump(ctx);
    }

    public void StopMoving()
    {
        moving = false;
    }
}
