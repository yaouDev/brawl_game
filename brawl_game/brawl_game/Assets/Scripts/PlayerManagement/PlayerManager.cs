using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject menuPlayerPrefab;

    private static int playerCounter;
    public InputMaster inputActions;
    //public event Action<InputActionMap> actionMapChange;
    private GameManager gm;

    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    public Dictionary<int, InputDevice> devices = new Dictionary<int, InputDevice>();

    [Header("Player Colors")]
    [SerializeField] public Color player1Color = Color.red;
    [SerializeField] public Color player2Color = Color.blue;
    [SerializeField] public Color player3Color = Color.yellow;
    [SerializeField] public Color player4Color = Color.green;

    [Header("Fill in the character tag")]
    [SerializeField] private string playerTag;

    //[Header("Read Only")]
    public PlayerInputManager pim { get; private set; }

    private void Awake()
    {
        instance ??= this;

        DontDestroyOnLoad(gameObject);
        inputActions = new InputMaster();
    }

    private void Start()
    {
        pim = GetComponent<PlayerInputManager>();
        gm = GameManager.instance;

        //ToggleActionMap(inputActions.MenuPlayer);

        pim.onPlayerJoined += player => AddPlayer(player);
        EventManager.instance.onPlayerElimination += CheckForLivingPlayers;
    }

    public void AddPlayer(PlayerInput pi)
    {
        if (gm.state != GameState.characterSelection)
        {
            Debug.LogWarning("Trying to add players while not in Character Selection Mode!");
            return;
        }

        //if in char select
        //Instantiate(menuPlayerPrefab, pi.gameObject.transform);

        //change back to playerCounter if something weird happens
        players.Add(pi.playerIndex, pi.gameObject);
        Player playerIteration = pi.gameObject.GetComponent<Player>();
        playerIteration.SetId(pi.playerIndex);
        //playerCounter++;

        AddCharacterSelection();

        print($"Active players: {players.Count}");
    }

    public void AddCharacterSelection()
    {
        foreach (GameObject go in players.Values)
        {
            if (go?.GetComponent<CharacterSelection>() == null)
            {
                go.AddComponent<CharacterSelection>();
            }
        }
    }

    public void RemovePlayer(int id)
    {
        players.Remove(id);
    }

    public Color GetPlayerColor(int id)
    {
        switch (id)
        {
            case 0: return player1Color;
            case 1: return player2Color;
            case 2: return player3Color;
            case 3: return player4Color;
            default: return Color.black;
        }
    }

    public void CheckForLivingPlayers()
    {
        if(playerTag.Length < 0)
        {
            Debug.LogWarning("Player Tag is 0");
            return;
        }

        GameObject[] livingPlayers = GameObject.FindGameObjectsWithTag(playerTag);

        if(livingPlayers.Length <= 1)
        {
            gm.EndGame();
        }

        foreach (var go in livingPlayers)
        {
            Debug.Log(go.name + " is still living!");
        }
    }

    //public void ToggleActionMap(InputActionMap actionMap)
    //{
    //    if (actionMap.enabled)
    //    {
    //        return;
    //    }

    //    inputActions.Disable();
    //    actionMapChange?.Invoke(actionMap);
    //    actionMap.Enable();
    //}
}
