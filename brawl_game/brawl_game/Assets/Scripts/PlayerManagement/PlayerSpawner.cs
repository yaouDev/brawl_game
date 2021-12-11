using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
        //InputMaster controls = new InputMaster();
        //controls.Disable();
        //controls.Player.Enable();
        //pm.ToggleActionMap(pm.inputActions.Player);

        foreach (GameObject go in pm.players.Values)
        {
            //go.GetComponentInChildren<MenuPlayer>().gameObject.SetActive(false);
            go.GetComponent<Player>().menuPlayer.SetActive(false);

            GameObject character = Instantiate(go.GetComponent<CharacterSelection>().selected, go.transform);
            SetPlayerIndicator(character, go);

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

    private void SetPlayerIndicator(GameObject character, GameObject player)
    {
        TMP_Text text = character.GetComponentInChildren<TMP_Text>();

        if (text == null) return;

        int id = player.GetComponent<Player>().playerId;
        text.gameObject.GetComponent<UIFollowGameObject>().target = character.transform;
        text.color = pm.GetPlayerColor(id);
        text.text = $"Player {id + 1}";
    }
}
