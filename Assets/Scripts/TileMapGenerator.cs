using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class TileMapGenerator : MonoBehaviour
{
    public static TileMapGenerator instance;

    public MapTile mapTilePrefab;
    public MapTile mapTileSlopedPrefab;
    public int mapTileSize = 1;

    private int dimX;
    private int dimY;

    private MapTile[,] mapTiles;

    private Dictionary<string, int> resourcesToAssign = new Dictionary<string, int>() {{"wood", 2000}, {"food", 2000}, {"stone", 1000}, {"iron", 1000}};

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //InteractionManager.instance.onTileSelect += DeleteTileTest;

        GenerateTileMap(10, 10);
    }

    void Update()
    {
    }

    public void GenerateTileMap(int dimX, int dimY)
    {
        this.dimX = dimX;
        this.dimY = dimY;

        mapTiles = new MapTile[dimY, dimX];

        List<MapTile> shuffledTileList = new List<MapTile>();

        for(int y = 0; y < dimY; y++)
        {
            for(int x = 0; x < dimX; x++)
            {
                MapTile tile;
                if(x == 0 || x == dimX - 1 || y == 0 || y == dimY - 1)
                    tile = GameObject.Instantiate(mapTileSlopedPrefab);
                else
                    tile = GameObject.Instantiate(mapTilePrefab);

                tile.transform.parent = this.transform;
                tile.transform.localPosition = new Vector3(x * mapTileSize, UnityEngine.Random.Range(-0.03f, 0.03f), y * mapTileSize) + new Vector3(mapTileSize/2, -mapTileSize/2, mapTileSize/2);
                tile.transform.localScale = Vector3.one;
                tile.transform.localRotation = Quaternion.identity;
                //tile.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color((x / 10.0f) % 1.0f, (y / 10.0f) % 1.0f , x*y));
                mapTiles[y, x] = tile;
                shuffledTileList.Add(tile);
            }
        }

        foreach(var resourcePair in resourcesToAssign)
        {
            // Shuffle tile list
            var rand = new System.Random();
            shuffledTileList = shuffledTileList.OrderBy (x => rand.Next()).ToList();

            int tilesWithResources = shuffledTileList.Count / 2;
            int resourcesPerTile = resourcePair.Value / (tilesWithResources * (GameManager.AbandondIslands +1));

            for(int iTile = 0; iTile < tilesWithResources; iTile++)
            {
                shuffledTileList[iTile].addAvailableResource(resourcePair.Key, resourcesPerTile);
            }
        }
    }

    public MapTile GetTile(int x, int y)
    {
        if(x >= 0 && x < dimX && y >= 0 && y < dimY)
        {
            return mapTiles[y, x];
        }
        else
            return null;
    }

    private void DeleteTileTest(int x, int y)
    {
        MapTile tile = GetTile(x, y);
        if(tile != null)
        {
            GameObject.Destroy(tile.gameObject);
        }
    }

	public void DeleteAllTiles()
	{
		for (int x = 0; x < dimX; ++x)
			for (int y = 0; y < dimY; ++y)
				DeleteTileTest(x, y);
	}
}
