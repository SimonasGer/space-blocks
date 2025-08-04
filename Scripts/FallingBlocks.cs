using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingBlocks : MonoBehaviour
{
    public Tilemap falling, frame, inactive, active;
    public GameController gameController;
    public
    void FallBlock(Vector3Int position, TileBase tile)
    {
        falling.SetTile(position + Vector3Int.down, tile);
        falling.SetTile(position, null);
    }
    public void FallBlocks()
    {
        bool ground = false;
        foreach (var position in falling.cellBounds.allPositionsWithin)
        {
            var tile = falling.GetTile(position);
            if (tile != null && CheckGround(position + Vector3Int.down))
            {
                ground = true;
            }

        }
        if (!ground)
        {
            foreach (var position in falling.cellBounds.allPositionsWithin)
            {
                var tile = falling.GetTile(position);
                if (tile != null)
                {
                    FallBlock(position, tile);
                }
            }
            gameController.state = GameController.States.fall;
        }
        else
        {
            Convert();
            Debug.Log("Reached ground");
            gameController.state = GameController.States.active;
        }
    }

    public bool CheckGround(Vector3Int position)
    {
        if (frame.GetTile(position) || inactive.GetTile(position))
        {
            return true;
        }
        return false;
    }
    public void Convert()
    {
        foreach (var position in falling.cellBounds.allPositionsWithin)
        {
            var tile = falling.GetTile(position);
            if (tile != null)
            {
                active.SetTile(position, tile);
                falling.SetTile(position, null);
            }
        }
    }
}
