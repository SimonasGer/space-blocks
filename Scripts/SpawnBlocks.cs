using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnBlocks : MonoBehaviour
{
    public BlockData.ShapeData data;
    public Vector3Int spawnPoint;
    public Tilemap falling;
    public List<TileBase> colors;

    // I need this method to be usable in a switch statement
    public void SpawnShape()
    {
        if (data == null || colors == null || colors.Count == 0 || falling == null)
            return;

        // Example: Place tiles at spawnPoint based on data.shape
        int shapeIndex = Random.Range(0, data.shapes.Length);
        BlockData.ShapeInfo shapeInfo = data.shapes[shapeIndex];
        foreach (var cell in shapeInfo.blockPositions)
        {
            int colorIndex = Random.Range(0, colors.Count);
            falling.SetTile(spawnPoint + cell, colors[colorIndex]);
        }

    }
}
