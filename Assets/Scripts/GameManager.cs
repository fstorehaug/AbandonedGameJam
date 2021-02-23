using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    Player player;
    [SerializeField]
    public static SettingsScriptableObject settings;

    public static int AbandondIslands;

    public static void GameOver(int score)
    {
        settings.playerScore = score;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Start()
    {
        player = new Player(settings.name);
        AbandondIslands = 0;
    }

    private void addStartingResources(Player player)
    {
        player.reasourceManager.AddResource("people", 7);
        player.reasourceManager.AddResource("wood", 20);
        player.reasourceManager.AddResource("food", 40);
    }

}
