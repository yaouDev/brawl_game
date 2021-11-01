using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Character character;
    private PlayerMovement movement;
    public int id { get; private set; }

    public void SetId(int playerId)
    {
        id = playerId;
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void SetMovement()
    {
        movement = GetComponent<PlayerMovement>();

        if(character == null)
        {
            Debug.LogWarning("No character set for player " + id);
            return;
        }

        movement.movementSpeed = character.movementSpeed;
        movement.jumpForce = character.jumpForce;
        movement.jumpAmount = character.jumpAmount;
        movement.crouchDivider = character.crouchDivider;
    }
}
