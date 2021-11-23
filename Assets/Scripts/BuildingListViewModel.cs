using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
﻿using System.ComponentModel;
using System;

[Binding]
public class BuildingListViewModel: MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private ObservableList<BuildingListItemViewModel> observableList = new ObservableList<BuildingListItemViewModel>();

    private string selectedBuildingCostTextInternal;
    private BuildingData selectedBuildingData;

    [Binding]
    public ObservableList<BuildingListItemViewModel> buildingList
    {
        get
        {
            return observableList;
        }
    }

    [Binding]
    public string selectedBuildingCostText
    {
        get
        {
            return selectedBuildingCostTextInternal;
        }
    }

    private void Start()
    {
        foreach(BuildingData building in BuildingDatabase.instance.buildings)
        {
            BuildingListItemViewModel buildingVM = new BuildingListItemViewModel(building, this);
            observableList.Add(buildingVM);
        }

        BuildingPlacementManager.instance.onPlacementStateChanged += OnBuildingPlacement;

        OnPropertyChanged("buildingList");
    }

    private void OnDestroy()
    {
        BuildingPlacementManager.instance.onPlacementStateChanged -= OnBuildingPlacement;
    }

    private void SetTooltipBuilding(Nullable<BuildingData> building)
    {
        string costString = "";
        if (building != null)
        {
            costString += $"{building.Value.buildingName} - Cost: ";
            for (int iCost = 0; iCost < building.Value.cost.Count; iCost++)
            {
                BuildingResourceCost cost = building.Value.cost[iCost];
                costString += $"{cost.cost} {cost.resource}";
                if (iCost < building.Value.cost.Count - 1)
                    costString += ", ";
            }
        }
        selectedBuildingCostTextInternal = costString;
        OnPropertyChanged("selectedBuildingCostText");
    }

    private void OnBuildingPlacement(bool isPlacing)
    {
        if (isPlacing)
            SetTooltipBuilding(BuildingPlacementManager.instance.GetBuildingData());
        else
            SetTooltipBuilding(null);
    }

    public void StartHoverBuilding(BuildingData buildingData)
    {
        selectedBuildingData = buildingData;
        SetTooltipBuilding(buildingData);
    }

    public void EndHoverBuilding(BuildingData buildingData)
    {
        if (selectedBuildingData.buildingName == buildingData.buildingName)
        {
            if (BuildingPlacementManager.instance.IsPlacing())
                SetTooltipBuilding(BuildingPlacementManager.instance.GetBuildingData());
            else
                SetTooltipBuilding(null);
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
