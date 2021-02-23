using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public float tickInterval = 1.0f;
    
    public static UnityAction onTick;

    private float accumulatedTime = 0.0f;

    private void Update()
    {
        accumulatedTime += Time.deltaTime;

        while(accumulatedTime > tickInterval)
        {
            accumulatedTime -= tickInterval;
            onTick?.Invoke();
        }
    }
}
