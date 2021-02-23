using System.Collections.Generic;
using UnityEngine;

public class BuildingDatabase : MonoBehaviour
{
    public List<BuildingData> buildings;

    public static BuildingDatabase instance;

    private void Awake()
    {
        instance = this;
    }
}