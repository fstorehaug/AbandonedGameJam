using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
﻿using System.ComponentModel;

[Binding]
public class HUDViewModel: MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [Binding]
    public void StartPlacingTestModel()
    {
        BuildingPlacementManager.instance.StartPlacement(BuildingDatabase.instance.buildings[0]);
    }

	[Binding]
	public static void AbandonIsland()
	{
        GameManager.AbandondIsland();
    }

	private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
