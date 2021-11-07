using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance;

    public Transform[] spawnPositions;

    private GameManager gm;
    private PlayerManager pm;

    //[Header("Read Only")]


    private void Awake()
    {
        instance ??= this;

        gm = GameManager.instance;
        pm = PlayerManager.instance;
    }

    public void SpawnPlayers()
    {
        Debug.Log($"Attempting to spawn {pm.players.Count} players");

        int spawnCounter = 0;

        //change actionmap
        InputMaster controls = new InputMaster();
        controls.Disable();
        controls.Player.Enable();
        //pm.ToggleActionMap(pm.inputActions.Player);

        foreach (GameObject go in pm.players.Values)
        {
            go.GetComponentInChildren<MenuPlayer>().gameObject.SetActive(false);

            Instantiate(go.GetComponent<CharacterSelection>().selected, go.transform);

            PlayerControls pc = go.GetComponent<PlayerControls>();
            pc?.GetPlayerMovement();

            PlayerInput pi = go.GetComponent<PlayerInput>();

            pi.defaultActionMap = pm.inputActions.Player.Get().name;
            pi.currentActionMap = pm.inputActions.Player;

            //dev use
            print(pi.currentActionMap);
            print(pm.inputActions.Player.enabled);
            print(pi.defaultActionMap);

            //destroy things
            //Destroy(go.GetComponentInChildren<MenuPlayer>().gameObject);
            //Destroy(go.GetComponent<CharacterSelection>());

            go.transform.position = spawnPositions[spawnCounter].position;
            spawnCounter++;
        }
    }
}
