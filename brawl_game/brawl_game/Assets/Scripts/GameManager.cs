using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state { get; private set; }

    private PlayerManager pm;

    //[Header("Read Only")]
    //public int activePlayers;

    private void Awake()
    {
        instance ??= this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        pm = PlayerManager.instance;

        //dev use
        SetGameState(GameState.characterSelection);
    }

    public void StartGame()
    {
        if(pm.players.Count == 0)
        {
            Debug.LogWarning("No one is playing!");
            return;
        }

        foreach(GameObject player in pm.players.Values)
        {
            if(player.TryGetComponent(out CharacterSelection cs))
            {
                if(cs.selected == null)
                {
                    Debug.Log("Not all players are ready!");
                    return;
                }
            }
            else
            {
                Debug.LogWarning("Player without CharacterSelection present");
                return;
            }
        }

        //on success

        SetGameState(GameState.game);
        SceneManager.LoadScene(1);
    }

    public void SetGameState(GameState newState)
    {
        state = newState;
    }
}

public enum GameState
{
    mainMenu, characterSelection, game
}
