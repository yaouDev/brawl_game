using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Character> tempCharacters;

    public static GameManager instance;

    public Dictionary<int, Character> players = new Dictionary<int, Character>();

    [Header("Read Only")]
    public int activePlayers;

    private void Awake()
    {
        instance ??= this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayer(Character character)
    {
        character ??= tempCharacters[Random.Range(0, tempCharacters.Count)];

        activePlayers++;
        players.Add(activePlayers, character);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

}
