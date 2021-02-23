using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
﻿using System.ComponentModel;

[Binding]
public class TileSelectionViewModel: MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
