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
        if (Extensions.CheckIfAlreadyExists(this))
        {
            Destroy(gameObject);
            return;
        }
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

    public void EndGame()
    {
        SpawnMenuPlayers();

        //take care of dummies
        GameObject[] dummies = GameObject.FindGameObjectsWithTag("Dummy");
        for (int i = 0; i < dummies.Length; i++)
        {
            Destroy(dummies[i]);
        }

        //change scene
        SetGameState(GameState.mainMenu);
        SceneManager.LoadScene(0);
    }

    public void SetGameState(GameState newState)
    {
        state = newState;
    }

    private void SpawnMenuPlayers()
    {
        //change actionmap
        //InputMaster controls = new InputMaster();
        //controls.Disable();
        //controls.MenuPlayer.Enable();

        foreach (GameObject go in pm.players.Values)
        {
            go.GetComponent<Player>().menuPlayer.SetActive(true);
            if (go.TryGetComponent(out PlayerCombat character)) Destroy(character.gameObject);

            PlayerInput pi = go.GetComponent<PlayerInput>();
            pi.SwitchCurrentActionMap(pm.inputActions.MenuPlayer.Get().name);
        }
    }
}

public enum GameState
{
    mainMenu, characterSelection, game
}
