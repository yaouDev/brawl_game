using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private static int playerCounter;

    public Dictionary<int, GameObject> menuPlayers = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> gamePlayers = new Dictionary<int, GameObject>();

    //[Header("Read Only")]
    //public int activePlayers;

    private void Awake()
    {
        instance ??= this;
        DontDestroyOnLoad(gameObject);
    }

    public void Participate()
    {
        PlayerInput[] users = FindObjectsOfType<PlayerInput>();
        foreach(PlayerInput pi in users)
        {
            menuPlayers.Add(playerCounter, pi.gameObject);
            CharacterSelection selection = pi.gameObject.AddComponent<CharacterSelection>();
            pi.gameObject.GetComponent<MenuPlayer>().characterSelection = selection;

            playerCounter++;
        }
    }

    public void StartGame()
    {
        if(menuPlayers.Count == 0)
        {
            Debug.LogWarning("No one is playing!");
            return;
        }

        foreach(GameObject player in menuPlayers.Values)
        {
            if(player.TryGetComponent(out CharacterSelection cs))
            {
                if(cs.selected == null)
                {
                    Debug.Log("Not all players are ready!");
                    return;
                }
            }
        }

        //overwrite playerprefab
        foreach(KeyValuePair<int, GameObject> entry in menuPlayers)
        {
            GameObject character = entry.Value?.GetComponent<CharacterSelection>()?.selected;
            gamePlayers.Add(entry.Key, character);
        }

        SceneManager.LoadScene(1);
    }

}
