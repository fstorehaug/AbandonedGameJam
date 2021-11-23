using System.ComponentModel;
using UnityEngine;
﻿using UnityWeld.Binding;

[Binding]
public class BuildingListItemViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private BuildingData buildingData;
    private BuildingListViewModel buildingListVM;

    public BuildingListItemViewModel(BuildingData building, BuildingListViewModel listVM)
    {
        buildingData = building;
        buildingListVM = listVM;

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
            return BuildingManager.canAfford(GameManager.player.reasourceManager, buildingData);
        }
    }

    [Binding]
    public void StartPlacement()
    {
        if(BuildingManager.canAfford(GameManager.player.reasourceManager, buildingData))
        {
            BuildingPlacementManager.instance.StartPlacement(buildingData);
        }
        else
        {
            Debug.LogError("Not enough resources!");
        }
    }

    [Binding]
    public void HoverBuilding()
    {
        buildingListVM.StartHoverBuilding(buildingData);
    }

    [Binding]
    public void UnHoverBuilding()
    {
        buildingListVM.EndHoverBuilding(buildingData);
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
