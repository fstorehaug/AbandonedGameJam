using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public bool isOccupied;
    public bool isCoast;

    private Dictionary<string, int> availableresources = new Dictionary<string, int>();

    public int extractResource(string resourceName, int value)
    {
        if (availableresources[resourceName] >= value)
        {
            availableresources[resourceName] -= value;
            return value;
        }
        else
        {
            int lastres = availableresources[resourceName];
            availableresources[resourceName] = 0;
            return lastres;
        }
    }

    public int getAvailableResources(string resourceName)
    {
        if(availableresources.ContainsKey(resourceName))
            return availableresources[resourceName];
        else
            return 0;
    }

    public void addAvailableResource(string resourceName, int count)
    {
        if(!availableresources.ContainsKey(resourceName))
            availableresources.Add(resourceName, 0);
        else
            availableresources[resourceName] += count;
    }
}
