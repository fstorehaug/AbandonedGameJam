using UnityEngine;

public class BuildingPlacementManager
{
    public static BuildingPlacementManager instance;

    private BuildingData buildingData;
    private GameObject buildingGhost;
    private bool isPlacing = false;

    public void StartPlacement(BuildingData buildingData)
    {
        this.buildingData = buildingData;

        buildingGhost = GameObject.Instantiate(buildingData.model);

        isPlacing = true;
    }

    public void EndPlacement()
    {
        isPlacing = false;
    }

    private void Start()
    {
        InteractionManager.instance.onTileSelect += OnTileSelect;
    }

    private void Update()
    {
        if(isPlacing)
        {

        }
    }

    private void OnTileSelect(int x, int y)
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
}
