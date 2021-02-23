using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
	public static ResourceManager instance = null;

	public int RunningPoints { get; private set; }

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


	const int MaxPeoplePerBoat = 60;
	const int BestCasePeoplePerBoat = 10;
	public int DoAbandonmentPointCalculation()
	{
		if (CurrentResources["boats"] == 0 || CurrentResources["people"] == 0)
			return 0;

		//determine points by multiplying the number of survivors with the amount of resources and then multiplying that by the traveling comfort factor (crowding per boat)
		float travelingComfortFactor = (float)MaxPeoplePerBoat / Mathf.Clamp((float)CurrentResources["people"] / (float)CurrentResources["boats"], (float)BestCasePeoplePerBoat, (float)MaxPeoplePerBoat);
		float totalPoints = (float)CurrentResources["people"] * (float)(CurrentResources["food"] + CurrentResources["wood"] + 1) * travelingComfortFactor;

		RunningPoints += (int)totalPoints;
		return (int)totalPoints;
	}

	public void CarryOverReasourses()
    {
		int boats = CurrentResources["boats"];
		int people = CurrentResources["people"];
		int food = CurrentResources["food"];
    }

	public void ResetResources()
	{
		CurrentResources = new Dictionary<string, int>();
		foreach (string resourcename in resourses)
		{
			CurrentResources.Add(resourcename, 0);
		}
	}

    public ResourceManager()
    {
		ResetResources();
		instance = this;
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

