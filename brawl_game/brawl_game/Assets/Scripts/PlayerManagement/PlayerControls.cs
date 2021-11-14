using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    
    //wipe upon start
    public Vector2 aim { get; private set; }
    //--

    //[Header("Read Only")]
    public PlayerCombat combat;
    public PlayerMovement movement;
    private Player playerInfo;

    private void Start()
    {
        playerInfo = GetComponent<Player>();
    }

    public void GetPlayerScripts()
    {
        movement ??= GetComponentInChildren<PlayerMovement>();

        if(movement == null)
        {
            Debug.LogWarning($"No movement for player {playerInfo.playerId}");
        }

        combat ??= GetComponentInChildren<PlayerCombat>();

        if (combat == null)
        {
            Debug.LogWarning($"No combat for player {playerInfo.playerId}");
        }
    }

    public void ReadDirection(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            aim = ctx.ReadValue<Vector2>();
        }
    }

    //Movement
    public void Move(InputAction.CallbackContext ctx)
    {
        movement?.Move(ctx);
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        movement?.Jump(ctx);
    }

    public void Down(InputAction.CallbackContext ctx)
    {
        movement?.Down(ctx);
    }

    public void Dodge(InputAction.CallbackContext ctx)
    {
        movement?.Dodge(ctx);
    }
    //--

    //Combat
    public void LightAttack(InputAction.CallbackContext ctx)
    {
        combat?.LightAttack(ctx);
    }

    public void HeavyAttack(InputAction.CallbackContext ctx)
    {
        combat?.HeavyAttack(ctx);
    }

    public void SpecialAttack(InputAction.CallbackContext ctx)
    {
        combat?.SpecialAttack(ctx);
    }
    //-- 

    public Quaternion GetAngle()
    {
        float angle = Mathf.Atan2(aim.y, aim.x);
        angle *= Mathf.Rad2Deg;

        return Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, angle + 90f);
    }

    public void OnDeath()
    {
        movement = null;
        aim = Vector2.zero;
    }
}
