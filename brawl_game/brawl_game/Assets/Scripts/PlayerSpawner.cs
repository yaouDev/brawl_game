using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance;

    public Transform[] spawnPositions;

    private GameManager gm;

    //[Header("Read Only")]


    private void Awake()
    {
        instance ??= this;

        gm = GameManager.instance;
    }

    public void SpawnPlayers()
    {
        Debug.Log($"Attempting to spawn {gm.activePlayers} players");

        for (int i = 0; i < gm.activePlayers; i++)
        {
            Vector3 spawn = spawnPositions[i].position;
            if (gm.players.TryGetValue(i, out GameObject player))
            {
                Player playerInstance = Instantiate(player, spawn, Quaternion.identity).GetComponent<Player>();
            }
            else
            {
                Debug.LogWarning("No player spawned for id " + i);
            }

        }
    }
}
