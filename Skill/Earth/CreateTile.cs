using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateTile : MonoBehaviour
{
    public TileBase tile;
    private Tilemap tilemap;
    private TilemapCollider2D tilemapCollider2D;
    private Vector3Int tilePositionInt;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapCollider2D = GetComponent<TilemapCollider2D>();
    }

    void Update()
    {
        
    }

    public void CreateOneTile(Vector3 position)
    {
        Vector3 tilePosition = tilemapCollider2D.ClosestPoint(position);
        tilePositionInt = tilemap.WorldToCell(tilePosition);
        tilemap.SetTile(tilePositionInt, tile);
    }
    
}
