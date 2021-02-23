using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
﻿using System.ComponentModel;

[Binding]
public class PlayerResourceViewModel: MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;


    [Binding]
    public int resourceCountPeople
    {
        get
        {
            return GameManager.player == null? 0 : GameManager.player.reasourceManager.CurrentResources["people"];
        }
    }

    [Binding]
    public int resourceCountBoats
    {
        get
        {
            return GameManager.player == null ? 0 : GameManager.player.reasourceManager.CurrentResources["boats"];
        }
    }

    [Binding]
    public int resourceCountWood
    {
        get
        {
            return GameManager.player == null ? 0 : GameManager.player.reasourceManager.CurrentResources["wood"];
        }
    }

    [Binding]
    public int resourceCountFood
    {
        get
        {
            return GameManager.player == null ? 0 : GameManager.player.reasourceManager.CurrentResources["food"];
        }
    }

    [Binding]
    public int resourceCountStone
    {
        get
        {
            return GameManager.player == null ? 0 :GameManager.player.reasourceManager.CurrentResources["stone"];
        }
    }

    [Binding]
    public int resourceCountIron
    {
        get
        {
            return GameManager.player == null ? 0 : GameManager.player.reasourceManager.CurrentResources["iron"];
        }
    }

    private void Update()
    {
        // TODO: subscribe to resource changes
        OnPropertyChanged("resourceCountPeople");
        OnPropertyChanged("resourceCountBoats");
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
