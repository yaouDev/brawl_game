using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Player playerInfo;
    public int startingLives = 3;
    public float invulnerableTime = 2f;
    private float respawnTimer;

    [Header("Read Only")]
    public GameObject childPlayer;
    private Rigidbody2D childRb;
    public int currentLives;
    [SerializeField] private bool invulnerable;

    public void Refresh()
    {
        playerInfo = GetComponent<Player>();
        childRb = GetComponentInChildren<Rigidbody2D>();
        childPlayer = childRb.gameObject;
        currentLives = startingLives;
    }

    public void TakeHit(Vector2 hitForce)
    {
        if (invulnerable) return;

        Debug.Log($"{gameObject.name} was hit by {hitForce}");
        childRb?.AddForce(hitForce, ForceMode2D.Impulse);
    }

    public void Die()
    {
        if (invulnerable) return;

        Debug.Log($"Player {playerInfo?.playerId} was killed!");

        currentLives--;

        EventManager.instance.OnPlayerDeath_Action();

        if (currentLives <= 0)
        {
            Eliminate();
            return;
        }

        Respawn();
    }

    private IEnumerator RespawnInvulnerable(Vector3 freezePos)
    {
        invulnerable = true;

        float end = Time.time + invulnerableTime;
        while(Time.time < end)
        {
            if (childRb != null) childRb.velocity = Vector2.zero;
            childPlayer.transform.position = freezePos;
            yield return null;
        }

        invulnerable = false;
    }

    private void Respawn()
    {
        if (childRb != null) childRb.velocity = Vector2.zero;
        int randomNumber = Random.Range(0, PlayerSpawner.instance.spawnPositions.Length - 1);
        Vector3 spawnPos = PlayerSpawner.instance.spawnPositions[randomNumber].position;

        StartCoroutine(RespawnInvulnerable(spawnPos));
    }

    private void Eliminate()
    {
        GetComponent<PlayerControls>().OnDeath();

        //wait for animations/particles/sound to play out
        Debug.Log($"Player {GetComponent<Player>().playerId} was eliminated!!!");
        Destroy(childPlayer);
    }
}
