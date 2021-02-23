using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{

    public static List<GameObject> buildings = new List<GameObject>();
    public static Vector3 buildingOffsetVector;


    public static bool canBuildWater(Player player, BuildingData building, Tile tile)
    {
        if (tile.neighbors.Count < 8)
        {
            return false;
        }

        return canBuildLand(player, building, tile);
    }

    public static bool canBuildLand(Player player, BuildingData building, Tile tile)
    {
        if (tile.isOccupied)
            return false;

        if (!canAfford(player.reasourceManager, building))
        {
            return false;
        }

        return true;
    }
   
    public static bool canAfford(ResourceManager resourceManager, BuildingData building)
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

    public static void Build(Player player, BuildingData buidling, Tile tile)
    {
        foreach (string resourcename in buidling.cost.Keys)
        {
            player.reasourceManager.SpendResource(resourcename, buidling.cost[resourcename]);
        }

        GameObject buildingmodel = Instantiate(buidling.model);
        buildingmodel.transform.parent = tile.transform;
        buildingmodel.transform.localPosition = buildingOffsetVector;
        Building buildingScript  = buildingmodel.AddComponent<Building>();

        buildingScript.setupBuilding(buidling, player);
    }

}



public class BuildingData
{
    public Dictionary<string, int> cost;
    public GameObject model;
    public int production;
    public string resourceType;
}

public class Building : MonoBehaviour
{
    public void setupBuilding(BuildingData data, Player player)
    {

    }

}

public class ResourceManager
{
    public Dictionary<string, int> CurrentResources;
    public static readonly string[] resourses =
    {
        "wood",
        "food",
        "stone",
        "iron"
    };

    public ResourceManager()
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

    public readonly ResourceManager reasourceManager;

    public Player(string name)
    {
        reasourceManager = new ResourceManager();
        color = new Color(Random.Range(0, 264), Random.Range(0, 264), Random.Range(0, 264));
        this.name = name;
    }

}

public class Tile : MonoBehaviour
{
    public bool isOccupied;
    public List<Tile> neighbors;
}