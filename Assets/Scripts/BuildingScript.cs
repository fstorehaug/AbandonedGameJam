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
    public void setupBuilding(BuildingData data, Player player)
    {
        owner = player;
        isProducing = false;

        this.productionresname = data.productionResourseType;
        this.consumtionresname = data.consumtionResourseType;
        this.productionValue = data.productionValue;
        this.consumtionValue = data.consumtionValue;

        this.productionDelay = data.productionDelay;
    }

    public void ProduceResources() 
    {
        if (productionTime <= productionDelay)
        {
            productionTime = 0;
            isProducing = false;
            owner.reasourceManager.AddResource(productionresname, productionValue);
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

    private void OnDestroy()
    {
        
    }
}
