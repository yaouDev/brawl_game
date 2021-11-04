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
    public CharacterSelection characterSelection;
    public bool isSelecting;

    private void Awake()
    {
        isKeyboard = GetComponent<PlayerInput>().currentControlScheme == "Keyboard";
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            if(isKeyboard)
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

    public void Move(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();

        moving = ctx.performed;
    }

    public void Select(InputAction.CallbackContext ctx)
    {
        isSelecting = ctx.performed;
    }
}
