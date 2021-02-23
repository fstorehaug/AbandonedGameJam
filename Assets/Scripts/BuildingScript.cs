using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingScript : MonoBehaviour
{

    private string productionresname;
    private int productionValue;
    private string consumtionresname;
    private int consumtionValue;

    private int productionDelay;
    private int productionTime;

    public bool isProducing;

    public int requiredWorkers;
    public int assignedWorkers;

    Player owner;
    MapTile tile;
    public void setupBuilding(BuildingData data, Player player, MapTile tile)
    {
        owner = player;
        isProducing = false;
        assignedWorkers = 0;

        this.productionresname = data.productionResourseType;
        this.consumtionresname = data.consumtionResourseType;
        this.productionValue = data.productionValue;
        this.consumtionValue = data.consumtionValue;

        this.productionDelay = data.productionDelay;
        this.requiredWorkers = data.workerCost;

        this.tile = tile;
    }

    public void ProduceResources() 
    {
        

        if (productionTime <= productionDelay)
        {
            productionTime = 0;
            isProducing = false;
            owner.reasourceManager.AddResource(productionresname, tile.extractResource(productionresname, productionValue));
            return;
        }

        if (isProducing)
        {
            if (assignedWorkers == requiredWorkers)
            {
                productionTime++;
            }
        }
        else
        {
            if (owner.reasourceManager.CurrentResources[consumtionresname] >= consumtionValue)
            {
                owner.reasourceManager.SpendResource(consumtionresname, consumtionValue);
                isProducing = true;
            }
        }
    }

    public void addWorker()
    {
        assignedWorkers++;
    }

    public void removeWorker()
    {
        assignedWorkers--;
    }

    private void OnDestroy()
    {
        
    }
}
