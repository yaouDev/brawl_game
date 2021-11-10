using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //[Header("Read Only")]
    //wipe upon start
    public Vector2 aim { get; private set; }
    private float lookAngle;
    //--


    private PlayerMovement movement;
    private Player playerInfo;

    private void Start()
    {
        playerInfo = GetComponent<Player>();
    }

    public void GetPlayerMovement()
    {
        movement ??= GetComponentInChildren<PlayerMovement>();

        if(movement == null)
        {
            Debug.LogWarning($"No movement for player {playerInfo.playerId}");
        }
    }

    public void ReadDirection(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            aim = ctx.ReadValue<Vector2>();
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        movement?.Move(ctx);
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        movement?.Jump(ctx);
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        movement?.Crouch(ctx);
    }

    public void Dodge(InputAction.CallbackContext ctx)
    {
        movement?.Dodge(ctx);
    }

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
