using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileMapGenerator : MonoBehaviour
{

    public MapTile mapTilePrefab;
    public int mapTileSize = 1;

    private int dimX;
    private int dimY;

    private MapTile[,] mapTiles;

    private PlayerControlls playerControlls;
    private Vector2 mousePosition;

    void Start()
    {
        playerControlls = new PlayerControlls();

        GenerateTileMap(32, 32);
        playerControlls.Enable();
        playerControlls.KeyboardMouse.TileSelect.performed += OnTileSelect;
        playerControlls.KeyboardMouse.MousePosition.performed += context => mousePosition = context.ReadValue<Vector2>();
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

    private void OnTileSelect(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        float d = (this.transform.position.y - ray.origin.y) / ray.direction.y;
        Vector3 hitPosWorld = ray.origin + ray.direction * d;
        Vector3 hitPosLocal = this.transform.InverseTransformPoint(hitPosWorld);

        MapTile tile = GetTile((int)hitPosLocal.x, (int)hitPosLocal.z);
        if(tile != null)
        {
            GameObject.Destroy(tile.gameObject);
        }
    }
}
