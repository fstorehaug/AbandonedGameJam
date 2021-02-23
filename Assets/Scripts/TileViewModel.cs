using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
﻿using System.ComponentModel;

[Binding]
public class TileViewModel: MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private MapTile tile;

    [Binding]
    public bool hasTile
    {
        get
        {
            return tile != null;
        }
    }

    [Binding]
    public int resourceCountWood
    {
        get
        {
            return tile.getAvailableResources("wood");
        }
    }

    [Binding]
    public int resourceCountFood
    {
        get
        {
            return tile.getAvailableResources("food");
        }
    }

    [Binding]
    public int resourceCountStone
    {
        get
        {
            return tile.getAvailableResources("stone");
        }
    }

    [Binding]
    public int resourceCountIron
    {
        get
        {
            return tile.getAvailableResources("iron");
        }
    }

    public void SetMapTile(MapTile tile)
    {
        this.tile = tile;
    }

    private void Start()
    {
        InteractionManager.instance.onTileHover += OnTileHover;
    }

    private void OnTileHover(int x, int y)
    {
        MapTile tile = TileMapGenerator.instance.GetTile(x, y);
        SetMapTile(tile);

        OnPropertyChanged("hasTile");
        OnPropertyChanged("resourceCountWood");
        OnPropertyChanged("resourceCountFood");
        OnPropertyChanged("resourceCountStone");
        OnPropertyChanged("resourceCountIron");
    }

    private void Update()
    {
        // TODO: subscribe to resource changes on tile
        OnPropertyChanged("hasTile");
        OnPropertyChanged("resourceCountWood");
        OnPropertyChanged("resourceCountFood");
        OnPropertyChanged("resourceCountStone");
        OnPropertyChanged("resourceCountIron");
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
