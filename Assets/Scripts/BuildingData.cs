using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuildingResourceCost
{
    public string resource;
    public int cost;
}

[System.Serializable]
public struct BuildingData
{
    public string buildingName;
    public List<BuildingResourceCost> cost;
    public GameObject model;
    
    public string consumtionResourseType;
    public string productionResourseType;
    public int consumtionValue;
    public int productionValue;

    public int productionDelay;
    public int workerCost;

    public bool needwater;
}
