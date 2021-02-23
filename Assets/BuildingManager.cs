using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{

    public static bool canBuildWater(Player player, Building building, Tile tile)
    {
        if (tile.neighbors.Count < 8)
        {
            return false;
        }

        return canBuildLand(player, building, tile);
    }

    public static bool canBuildLand(Player player, Building building, Tile tile)
    {
        if (tile.isOccupied)
            return false;

        if (!canAfford(player.reasourceManager, building))
        {
            return false;
        }

        return true;
    }
   
    public static bool canAfford(resourceManager resourceManager, Building building)
    {
        foreach (string reasourcename in building.cost.Keys)
        {
            if (resourceManager.CurrentResources[reasourcename] < building.cost[reasourcename])
            {
                return false;
            }
        }

        return true;
    }

    public static void Build(Player player, Building buidling, Tile tile)
    {
        foreach (string resourcename in buidling.cost.Keys)
        {
            player.reasourceManager.SpendResource(resourcename, buidling.cost[resourcename]);
        }

        buidling.build(tile);
    }

}



public class Building
{
    public Player owner;
    public Dictionary<string, int> cost;
    public GameObject model;
    public int production;
    public string resourceType;

    public PlayerControlls plauercontroll = new PlayerControlls();

    public void build(Tile tile)
    {
        //instatnitate an instance of the correct building on the map
    }



}

public class BuildingInstance : MonoBehaviour
{

}

public class resourceManager
{
    public Dictionary<string, int> CurrentResources;
    public static readonly string[] resourses =
    {
        "wood",
        "food",
        "stone",
        "iron"
    };

    public resourceManager()
    {
        CurrentResources = new Dictionary<string, int>();
        foreach (string resourcename in resourses)
        {
            CurrentResources.Add(resourcename, 0);
        }
    }

    public void SpendResource(string type, int value)
    {
        if (CurrentResources[type] >= value)
        {
            CurrentResources[type] -= value; 
        }
    }

}

public class Player
{
    public Color color;
    public string name;

    public readonly resourceManager reasourceManager;

    public Player(string name)
    {
        reasourceManager = new resourceManager();
        color = new Color(Random.Range(0, 264), Random.Range(0, 264), Random.Range(0, 264));
        this.name = name;
    }

}

public class Tile : MonoBehaviour
{
    public bool isOccupied;
    public List<Tile> neighbors;
}