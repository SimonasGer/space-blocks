using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeleteBlocks : MonoBehaviour
{
    public Tilemap inactive;
    public GameController gameController;
    public void CheckMatches()
    {
        HashSet<Vector3Int> toDelete = new();

        foreach (var pos in inactive.cellBounds.allPositionsWithin)
        {
            var tile = inactive.GetTile(pos);
            if (tile == null) continue;

            // Check all directions from this tile
            toDelete.UnionWith(GetMatches(pos, Vector3Int.right));        // →
            toDelete.UnionWith(GetMatches(pos, Vector3Int.down));         // ↓
            toDelete.UnionWith(GetMatches(pos, new Vector3Int(1, -1, 0))); // ↘
            toDelete.UnionWith(GetMatches(pos, new Vector3Int(-1, -1, 0)));// ↙
        }

        // Delete matches
        foreach (var pos in toDelete)
        {
            inactive.SetTile(pos, null);
        }

        if (toDelete.Count > 0)
        {
            Debug.Log($"Cleared {toDelete.Count} tiles!");
            gameController.state = GameController.States.inactive;
        }
        else
        {
            gameController.state = GameController.States.spawn;
        }
    }
    HashSet<Vector3Int> GetMatches(Vector3Int start, Vector3Int direction)
    {
        List<Vector3Int> match = new() { start };
        var targetTile = inactive.GetTile(start);

        for (int i = 1; i < 3; i++) // check next 2 tiles
        {
            Vector3Int next = start + direction * i;
            if (inactive.GetTile(next) == targetTile)
                match.Add(next);
            else
                break;
        }

        return match.Count >= 3 ? new HashSet<Vector3Int>(match) : new();
    }

}
