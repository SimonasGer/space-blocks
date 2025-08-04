using UnityEngine;
using UnityEngine.Tilemaps;

public class ActiveBlocks : MonoBehaviour
{
    public Tilemap active, inactive, frame;
    public GameController gameController;
    void FallBlock(Vector3Int position, TileBase tile)
    {
        if (!CheckGround(position + Vector3Int.down))
        {
            active.SetTile(position + Vector3Int.down, tile);
        }
        else
        {
            inactive.SetTile(position, tile);
        }
        
        active.SetTile(position, null);
    }
    // Start the fall active loop
    public void FallActive()
    {
        bool allFell = true;
        // Check if there are any active tiles
        foreach (var position in active.cellBounds.allPositionsWithin)
        {
            var tile = active.GetTile(position);
            if (tile != null)
            {
                allFell = false;
            }
        }
        // If the are any active blocks, start the function
        if (!allFell)
        {
            foreach (var position in active.cellBounds.allPositionsWithin)
            {
                var tile = active.GetTile(position);
                if (tile != null)
                {
                    FallBlock(position, tile);
                }
            }
            FallActive();
        }
        else
        {
            gameController.state = GameController.States.inactive;
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
}
