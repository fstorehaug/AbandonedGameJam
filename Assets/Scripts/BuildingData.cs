using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingData
{
    public Dictionary<string, int> cost;
    public GameObject model;
    
    public string consumtionResourseType;
    public string productionResourseType;
    public int consumtionValue;
    public int productionValue;

    public int productionDelay;
    public int workerCost;

    public bool needwater;
}

