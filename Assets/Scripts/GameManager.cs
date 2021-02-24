using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{

    public static Player player;
    [SerializeField]
    public SettingsScriptableObject settings;

    public static SettingsScriptableObject staticSettings;
    public static int AbandondIslands;

    public static UnityAction onAbandond;

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

    public static void AbandondIsland()
    {
        if (GameManager.player.reasourceManager.CurrentResources["boats"] == 0)
        {
            GameManager.GameOver(ResourceManager.instance.RunningPoints);
            return;
        }
        
        int pointsEarnedOnThisIsland = ResourceManager.instance.DoAbandonmentPointCalculation(); //TODO: display points earned on this island during transition

        if (pointsEarnedOnThisIsland == 0)
        {
            GameManager.GameOver(ResourceManager.instance.RunningPoints);
            return;
        }

        ResourceManager.instance.CarryOverReasourses();
        TileMapGenerator.instance.DeleteAllTiles();
        BuildingManager.DeleteAllBuildings();
        GameManager.AbandondIslands++;

        onAbandond?.Invoke();
        //TODO: do some transition stuff here

        //create the next island!
        TileMapGenerator.instance.GenerateTileMap(Mathf.Clamp(Random.Range(4, 6) - GameManager.AbandondIslands, 2, 5));
    }

    private void addStartingResources(Player player)
    {
        player.reasourceManager.AddResource("people", 7);
        player.reasourceManager.AddResource("wood", 20);
        player.reasourceManager.AddResource("food", 40);
    }

}
