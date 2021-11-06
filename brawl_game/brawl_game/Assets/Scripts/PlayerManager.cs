using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject menuPlayerPrefab;

    private static int playerCounter;
    private GameManager gm;

    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    public Dictionary<int, InputDevice> devices = new Dictionary<int, InputDevice>();

    //[Header("Read Only")]
    public PlayerInputManager pim { get; private set; }

    private void Awake()
    {
        instance ??= this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        pim = GetComponent<PlayerInputManager>();
        gm = GameManager.instance;

        pim.onPlayerJoined += player => AddPlayer(player);
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
        pi.gameObject.GetComponent<Player>().SetId(pi.playerIndex);
        //playerCounter++;

        AddCharacterSelection();

        print($"Active players: {playerCounter}");
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

    public void ConvertToGameMap()
    {
        foreach(GameObject go in players.Values)
        {

        }
    }
}
