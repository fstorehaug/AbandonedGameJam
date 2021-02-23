using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
﻿using System.ComponentModel;

[Binding]
public class BuildingListViewModel: MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private ObservableList<BuildingListItemViewModel> observableList = new ObservableList<BuildingListItemViewModel>();

    [Binding]
    public ObservableList<BuildingListItemViewModel> buildingList
    {
        get
        {
            return observableList;
        }
    }

    private void Start()
    {
        foreach(BuildingData building in BuildingDatabase.instance.buildings)
        {
            BuildingListItemViewModel buildingVM = new BuildingListItemViewModel(building);
            observableList.Add(buildingVM);
        }

        OnPropertyChanged("buildingList");
    }

	private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
