using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> tempCharacters;

    public static GameManager instance;

    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();

    [Header("Read Only")]
    public int activePlayers;
    public List<CharacterSelection> selections = new List<CharacterSelection>();

    private void Awake()
    {
        instance ??= this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayer(CharacterSelection cs)
    {
        selections.Add(cs);
        activePlayers++;
    }

    public void StartGame()
    {
        if(activePlayers == 0)
        {
            Debug.LogWarning("No one is playing!");
            return;
        }

        if(selections.Count != activePlayers)
        {
            Debug.LogWarning("Unassigned Character Selections");
            return;
        }

        foreach(CharacterSelection selection in selections)
        {
            if(selection.selected == null)
            {
                Debug.Log("Not all players are ready");
                return;
            }
        }

        for (int i = 0; i < activePlayers; i++)
        {
            players?.Add(i, selections?[i].selected);
        }

        SceneManager.LoadScene(1);
    }

}
