using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
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
        print("move");

        movement.Move(ctx);
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        print("jump");

        movement.Jump(ctx);
    }
}
