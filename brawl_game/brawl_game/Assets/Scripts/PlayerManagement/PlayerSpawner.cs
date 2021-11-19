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
        if(gm.state != GameState.game)
        {
            Debug.LogWarning("Trying to spawn players while not in-game!");
            return;
        }

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
            pc?.GetPlayerScripts();

            PlayerInput pi = go.GetComponent<PlayerInput>();
            pi.SwitchCurrentActionMap(pm.inputActions.Player.Get().name);

            //destroy things
            //Destroy(go.GetComponentInChildren<MenuPlayer>().gameObject);
            //Destroy(go.GetComponent<CharacterSelection>());

            go.GetComponent<PlayerState>().Refresh();
            go.transform.position = spawnPositions[spawnCounter].position;
            spawnCounter++;
        }

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Dummy"))
        {
            go.GetComponent<PlayerState>().Refresh();
        }
    }
}
