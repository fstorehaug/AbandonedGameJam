using System.ComponentModel;
using UnityEngine;
﻿using UnityWeld.Binding;

[Binding]
public class BuildingListItemViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private BuildingData buildingData;

    public BuildingListItemViewModel(BuildingData building)
    {
        buildingData = building;

        TimeManager.onTick += OnTick;
    }

    [Binding]
    public string buildingName
    {
        get
        {
            return buildingData.buildingName;
        }
    }

    [Binding]
    public bool hasSufficientResources
    {
        get
        {
            return BuildingManager.canAfford(Player.mainPlayer.reasourceManager, buildingData);
        }
    }

    [Binding]
    public void StartPlacement()
    {
        if(BuildingManager.canAfford(Player.mainPlayer.reasourceManager, buildingData))
        {
            BuildingPlacementManager.instance.StartPlacement(buildingData);
        }
        else
        {
            Debug.LogError("Not enough resources!");
        }
    }

    private void OnTick()
    {
        OnPropertyChanged("hasSufficientResources");
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
