using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance;

    public GameObject player;
    public Transform[] spawnPositions;

    private GameManager gm;
    private Dictionary<int, Player> livingPlayers;

    //[Header("Read Only")]


    private void Awake()
    {
        instance ??= this;

        gm = GameManager.instance;
    }

    public void SpawnPlayers()
    {


        for (int i = 0; i < gm.activePlayers; i++)
        {
            Vector3 spawn = spawnPositions[i].position;
            Player playerInstance = Instantiate(player, spawn, Quaternion.identity).GetComponent<Player>();

            playerInstance.SetId(i + 1);

            if (gm.players.TryGetValue(i + 1, out Character character))
            {
                playerInstance.character = character;

                playerInstance.SetMovement();
                playerInstance.SetColor(character.color);
            }
        }
    }
}
