using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnBlocks : MonoBehaviour
{
    public BlockData.ShapeData data;
    public DeleteBlocks deleteBlocks;
    public Vector3Int spawnPoint;
    public Tilemap falling, inactive;
    public List<TileBase> colors;
    public Save save;
    public Menu menu;

    // I need this method to be usable in a switch statement
    public void SpawnShape()
    {
        if (data == null || colors == null || colors.Count == 0 || falling == null)
            return;
        GameOver();
        save.AutoSave();
        deleteBlocks.multiplier = 1;
        // Example: Place tiles at spawnPoint based on data.shape
        int shapeIndex = Random.Range(0, data.shapes.Length);
        BlockData.ShapeInfo shapeInfo = data.shapes[shapeIndex];
        foreach (var cell in shapeInfo.blockPositions)
        {
            int colorIndex = Random.Range(0, colors.Count);
            falling.SetTile(spawnPoint + cell, colors[colorIndex]);
        }

    }
    void GameOver()
    {
        BoundsInt tiles = inactive.cellBounds;
        foreach (var tile in tiles.allPositionsWithin)
        {
            if (tile.y >= spawnPoint.y)
            {
                menu.MenuRestart();
            }
        }
    }
}
