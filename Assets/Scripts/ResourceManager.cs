using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public Dictionary<string, int> CurrentResources;
    public static readonly string[] resourses =
    {
        "wood",
        "food",
        "stone",
        "iron",
        "people",
        "boats"
    };

    public ResourceManager()
    {
        CurrentResources = new Dictionary<string, int>();
        foreach (string resourcename in resourses)
        {
            CurrentResources.Add(resourcename, 0);
        }
    }

    public void AddResource(string type, int value)
    {
        CurrentResources[type] += value;
    }

    public void SpendResource(string type, int value)
    {
        if (CurrentResources[type] >= value)
        {
            CurrentResources[type] -= value;
        }
    }

}

