using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileMapGenerator : MonoBehaviour
{
    public static TileMapGenerator instance;

    public MapTile mapTilePrefab;
    public int mapTileSize = 1;

    private int dimX;
    private int dimY;

    private MapTile[,] mapTiles;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InteractionManager.instance.onTileSelect += DeleteTileTest;

        GenerateTileMap(32, 32);
    }

    void Update()
    {
    }

    public void GenerateTileMap(int dimX, int dimY)
    {
        this.dimX = dimX;
        this.dimY = dimY;

        mapTiles = new MapTile[dimY, dimX];

        for(int y = 0; y < dimY; y++)
        {
            for(int x = 0; x < dimX; x++)
            {
                MapTile tile = GameObject.Instantiate(mapTilePrefab);
                tile.transform.parent = this.transform;
                tile.transform.localPosition = new Vector3(x * mapTileSize, 0.05f, y * mapTileSize) + new Vector3(mapTileSize/2, -mapTileSize/2, mapTileSize/2);
                tile.transform.localScale = Vector3.one;
                tile.transform.localRotation = Quaternion.identity;
                tile.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color((x / 10.0f) % 1.0f, (y / 10.0f) % 1.0f , x*y));
                mapTiles[y, x] = tile;
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
}
