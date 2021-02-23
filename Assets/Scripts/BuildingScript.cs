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

    public bool isDepleted;

    Player owner;
    MapTile tile;
    public void setupBuilding(BuildingData data, Player player, MapTile tile)
    {
        owner = player;
        isProducing = false;
        isDepleted = false;
        assignedWorkers = 0;

        this.productionresname = data.productionResourseType;
        this.consumtionresname = data.consumtionResourseType;
        this.productionValue = data.productionValue;
        this.consumtionValue = data.consumtionValue;

        this.productionDelay = data.productionDelay;
        this.requiredWorkers = data.workerCost;

        this.tile = tile;
        TimeManager.onTick += ProduceResources;
    }

    public void ProduceResources() 
    {

        if (productionTime >= productionDelay)
        {
            if (productionresname == "people" || productionresname == "boats")
            {
                productionTime = 0;
                isProducing = false;
                owner.reasourceManager.AddResource(productionresname, productionValue);
                return;
            }

            productionTime = 0;
            isProducing = false;
            int extractedvalue = 0;
            try
            {
               extractedvalue = tile.extractResource(productionresname, productionValue);
            }
            catch
            {
                extractedvalue = 0;
                Debug.Log("productioresoursename: " + productionresname);
            }
            if(extractedvalue < productionValue)
            {
                TimeManager.onTick -= ProduceResources;
            }
            owner.reasourceManager.AddResource(productionresname, extractedvalue);
            return;
        }

        if (isProducing)
        {
            if (true /*TODO: assignedWorkers == requiredWorkers*/)
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
        TimeManager.onTick -= ProduceResources;
    }
}
