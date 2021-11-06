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
        foreach (GameObject go in pm.players.Values)
        {
            go.GetComponentInChildren<MenuPlayer>().gameObject.SetActive(false);

            Instantiate(go.GetComponentInChildren<CharacterSelection>().selected, go.transform);
            go.transform.position = spawnPositions[spawnCounter].position;
            spawnCounter++;
        }
    }

    private void ChangeActionMap(GameObject go)
    {
        InputMaster im = new InputMaster();
        //go.GetComponent<PlayerInput>().currentActionMap = im.Player;
    }
}
