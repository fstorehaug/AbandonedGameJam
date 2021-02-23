using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static Player player;
    [SerializeField]
    public SettingsScriptableObject settings;

    public static SettingsScriptableObject staticSettings;
    public static int AbandondIslands;

    public static void GameOver(int score)
    {
        staticSettings.playerScore = score;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Awake()
    {
        player = new Player(settings.name);
        AbandondIslands = 0;
        staticSettings = settings;
        addStartingResources(player);
    }

    private void addStartingResources(Player player)
    {
        player.reasourceManager.AddResource("people", 7);
        player.reasourceManager.AddResource("wood", 20);
        player.reasourceManager.AddResource("food", 40);
    }

}
