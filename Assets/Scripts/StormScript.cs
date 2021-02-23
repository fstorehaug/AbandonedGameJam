using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using System.ComponentModel;
[Binding]
public class StormScript : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private int baseTimeToStorm = 1000;
    private int timeToStorm { get { return CalculateTotalTime(); } }
    private int timeSpent;
 
    [Binding]
    public int TimeLeft { get { return timeToStorm - timeSpent; } }

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
        return (int)baseTimeToStorm * (1 /( GameManager.AbandondIslands + 1));
    }

    public void onTick() 
    {
        if (timeSpent >= timeToStorm)
        {
            GameManager.AbandondIsland();
            timeSpent = 0;
        }

        timeSpent++;
        OnPropertyChanged(nameof(timeSpent));
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
