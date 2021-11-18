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

    public void TakeHit(Vector2 hitForce)
    {
        Debug.Log($"{gameObject.name} was hit by {hitForce}");
        childPlayer?.GetComponent<Rigidbody2D>().AddForce(hitForce, ForceMode2D.Impulse);
    }

    public void Die()
    {
        Debug.Log($"Player {playerInfo?.playerId} was killed!");

        currentLives--;

        EventManager.instance.OnPlayerDeath_Action();

        if (currentLives <= 0)
        {
            Eliminate();
        }

        childPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
