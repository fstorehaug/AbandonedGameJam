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

    Player owner;
    public void setupBuilding(BuildingData data, Player player)
    {
        owner = player;
        isProducing = false;
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
            productionTime++;
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
