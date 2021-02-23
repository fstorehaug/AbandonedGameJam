using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using System.ComponentModel;
[Binding]
public class StormScript : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private int baseTimeToStorm = 1000;

    [Binding]
    public int TimeSpent { get; set; }
    [Binding]
    public int TotalTime { get
        {
            return CalculateTotalTime();
        } }

    private void Start()
    {
        TimeManager.onTick += onTick;
    }

    private void OnDestroy()
    {
        TimeManager.onTick -= onTick;
    }

    private int CalculateTotalTime() 
    {
        return (int)baseTimeToStorm * (1 / GameManager.AbandondIslands + 1);
    }

    public void onTick() 
    {
        if (TimeSpent >= TotalTime)
        {
            GameManager.AbandondIsland();
            TimeSpent = 0;
        }

        TimeSpent++;
        OnPropertyChanged(nameof(TimeSpent));
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
