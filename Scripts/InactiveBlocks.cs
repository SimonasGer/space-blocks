using UnityEngine;
using UnityEngine.Tilemaps;

public class InactiveBlocks : MonoBehaviour
{
    public Tilemap inactive, frame;
    public GameController gameController;
    public void FallAll()
    {
        bool anyFalling = false;
        foreach (var position in inactive.cellBounds.allPositionsWithin)
        {
            var tile = inactive.GetTile(position);
            var down = inactive.GetTile(position + Vector3Int.down);
            var floor = frame.GetTile(position + Vector3Int.down);
            if (tile != null && down == null && floor == null)
            {
                inactive.SetTile(position, null);
                inactive.SetTile(position + Vector3Int.down, tile);
                anyFalling = true;
            }
        }
        if (anyFalling)
        {
            FallAll();
        } else
        {
            gameController.state = GameController.States.delete;
        }
        
    }
}
