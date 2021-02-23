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
	public void AbandonIsland()
	{
		int pointsEarnedOnThisIsland = ResourceManager.instance.DoAbandonmentPointCalculation(); //TODO: display points earned on this island during transition
		ResourceManager.instance.ResetResources();
		TileMapGenerator.instance.DeleteAllTiles();
		BuildingManager.DeleteAllBuildings();

		//TODO: do some transition stuff here

		//create the next island!
		TileMapGenerator.instance.GenerateTileMap(Random.Range(16, 48), Random.Range(16, 48));
	}

	private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}