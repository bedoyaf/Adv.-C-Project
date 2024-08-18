using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    private TileBase floorTile, wallTop;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> tilepositions, Tilemap tilemap, TileBase tile)
    {
        int i = 0;
        foreach (var tilepos in tilepositions)
        {
            Debug.Log("pos number: " + i);
            i++;
            PaintSingleTile(tilemap, tile, tilepos);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int tilepos)
    {
        Debug.Log(tilepos);
    //    var tilePosition = tilemap.WorldToCell((Vector3Int)tilepos);
        tilemap.SetTile((Vector3Int)tilepos, tile);
        Debug.Log(tilemap.size);
    }
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void paintSingleBasickWall(Vector2Int position)
    {
        PaintSingleTile(wallTilemap, wallTop, position);
    }
}
