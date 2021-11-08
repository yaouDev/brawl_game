using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("Read Only")]
    private PlayerMovement movement;
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void GetPlayerMovement()
    {
        movement ??= GetComponentInChildren<PlayerMovement>();

        if(movement == null)
        {
            Debug.LogWarning($"No movement for player {player.playerId}");
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

    public void OnDeath()
    {
        movement = null;
    }
}
