using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyTile : MonoBehaviour
{
    private Tilemap tilemap;
    private TilemapCollider2D tilemapCollider2D;
    Vector3Int tilePositionInt;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapCollider2D = GetComponent<TilemapCollider2D>();
    }

    void Update()
    {
        
    }

    public void DestroyOneTile(Vector3 position)
    {
        Vector3 tilePosition = tilemapCollider2D.ClosestPoint(position);
        tilePositionInt = tilemap.WorldToCell(tilePosition);
        Invoke("DestroyLater", 0.05f);
    }
    
    void DestroyLater()
    {
        tilemap.SetTile(tilePositionInt, null);
    }
}
