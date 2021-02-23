using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{

    public static List<GameObject> buildings = new List<GameObject>();
    public static Vector3 buildingOffsetVector;

    public static bool CanBuild(Player player, BuildingData building, MapTile tile)
    {
        if (building.needwater && !tile.isCoast)
        {
            return false;
        }

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

    public static void Build(Player player, BuildingData buidling, MapTile tile)
    {
        foreach (string resourcename in buidling.cost.Keys)
        {
            player.reasourceManager.SpendResource(resourcename, buidling.cost[resourcename]);
        }

        GameObject buildingmodel = Instantiate(buidling.model);
        buildingmodel.transform.parent = tile.transform;
        buildingmodel.transform.localPosition = buildingOffsetVector;
        BuildingScript buildingScript  = buildingmodel.AddComponent<BuildingScript>();

        buildingScript.setupBuilding(buidling, player);
    }

}