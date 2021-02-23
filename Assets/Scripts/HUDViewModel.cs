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
		
        if (pointsEarnedOnThisIsland == 0)
        {
            GameManager.GameOver(ResourceManager.instance.RunningPoints);
        }

        ResourceManager.instance.CarryOverReasourses();
		TileMapGenerator.instance.DeleteAllTiles();
		BuildingManager.DeleteAllBuildings();
        GameManager.AbandondIslands++;

		//TODO: do some transition stuff here

		//create the next island!
		TileMapGenerator.instance.GenerateTileMap(Mathf.Clamp( Random.Range(7, 12) - GameManager.AbandondIslands, 3, 10), Mathf.Clamp( Random.Range(7, 12) - GameManager.AbandondIslands, 3, 10));
	}

	private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
