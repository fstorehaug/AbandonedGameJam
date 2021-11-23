using System;
using UnityEngine;
using UnityEngine.Events;

public class BuildingPlacementManager : MonoBehaviour
{
    public static BuildingPlacementManager instance;

    private BuildingData buildingData;
    private GameObject buildingGhost;
    private bool isPlacing = false;

    public UnityAction<bool> onPlacementStateChanged;

    public void StartPlacement(BuildingData buildingData)
    {
        this.buildingData = buildingData;

        if (buildingGhost != null)
            Destroy(buildingGhost);

        buildingGhost = GameObject.Instantiate(buildingData.model);

        isPlacing = true;
        onPlacementStateChanged?.Invoke(true);
    }

    public void EndPlacement()
    {
        isPlacing = false;
        Destroy(buildingGhost);
        onPlacementStateChanged?.Invoke(false);
    }

    public bool IsPlacing()
    {
        return isPlacing;
    }

    public BuildingData GetBuildingData()
    {
        return buildingData;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InteractionManager.instance.onTileHover += OnTileHover;
        InteractionManager.instance.onTileSelect += OnTileSelect;
    }

    private void OnDestroy()
    {
        InteractionManager.instance.onTileHover -= OnTileHover;
        InteractionManager.instance.onTileSelect -= OnTileSelect;
    }

    private void OnTileHover(int x, int y)
    {
        if(isPlacing)
        {
            MapTile tile = TileMapGenerator.instance.GetTile(x, y);
            if(tile != null)
            {
                buildingGhost.transform.parent = tile.transform;
                buildingGhost.transform.localPosition = new Vector3(0.0f, TileMapGenerator.instance.mapTileSize * 0.5f, 0.0f);
            }
        }
    }

    private void OnTileSelect(int x, int y)
    {
        if(isPlacing)
        {
            MapTile tile = TileMapGenerator.instance.GetTile(x, y);
            if(tile != null)
            {
                Player player = GameManager.player; // TODO: FIXME

                if(BuildingManager.CanBuild(player, buildingData, tile))
                {
                    BuildingManager.Build(player, buildingData, tile);
                    EndPlacement();
                }
            }
        }
    }
}
