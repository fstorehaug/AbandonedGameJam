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
        Debug.Log("TODO!");
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
