using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
﻿using System.ComponentModel;

[Binding]
public class PlayerResourceViewModel: MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private ResourceManager resourceManager;

    [Binding]
    public int resourceCountPeople
    {
        get
        {
            return resourceManager.CurrentResources["people"];
        }
    }

    [Binding]
    public int resourceCountBoats
    {
        get
        {
            return resourceManager.CurrentResources["boats"];
        }
    }

    [Binding]
    public int resourceCountWood
    {
        get
        {
            return resourceManager.CurrentResources["wood"];
        }
    }

    [Binding]
    public int resourceCountFood
    {
        get
        {
            return resourceManager.CurrentResources["food"];
        }
    }

    [Binding]
    public int resourceCountStone
    {
        get
        {
            return resourceManager.CurrentResources["stone"];
        }
    }

    [Binding]
    public int resourceCountIron
    {
        get
        {
            return resourceManager.CurrentResources["iron"];
        }
    }

    public void SetResourceManager(ResourceManager manager)
    {
        this.resourceManager = manager;
    }

    private void Awake()
    {
        SetResourceManager(Player.mainPlayer.reasourceManager);
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
