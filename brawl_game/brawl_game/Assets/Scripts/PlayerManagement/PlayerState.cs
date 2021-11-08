using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Player playerInfo;
    public int startingLives = 3;

    [Header("Read Only")]
    public GameObject childPlayer;
    public int currentLives;

    public void Refresh()
    {
        playerInfo = GetComponent<Player>();
        childPlayer = GetComponentInChildren<PlayerMovement>().gameObject;
        currentLives = startingLives;
    }

    public void Die()
    {
        Debug.Log($"Player {playerInfo?.playerId} was killed!");

        currentLives--;

        if (currentLives <= 0)
        {
            Eliminate();
        }

        Respawn();
    }

    private void Respawn()
    {
        int randomNumber = Random.Range(0, PlayerSpawner.instance.spawnPositions.Length - 1);
        Vector3 spawnPos = PlayerSpawner.instance.spawnPositions[randomNumber].position;

        childPlayer.transform.position = spawnPos;
    }

    private void Eliminate()
    {
        GetComponent<PlayerControls>().OnDeath();

        //wait for animations/particles/sound to play out
        Debug.Log($"Player {GetComponent<Player>().playerId} was eliminated!!!");
        Destroy(childPlayer);
    }
}