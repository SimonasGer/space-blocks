using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Menu : MonoBehaviour
{
    public DeleteBlocks deleteBlocks;
    public GameController gameController;
    public Tilemap falling, active, inactive, frame;

    public void MenuRestart()
    {
        deleteBlocks.score = 0;
        deleteBlocks.multiplier = 1;
        gameController.state = GameController.States.start;
        BoundsInt cords = frame.cellBounds;
        foreach (var cord in cords.allPositionsWithin)
        {
            falling.SetTile(cord, null);
            active.SetTile(cord, null);
            inactive.SetTile(cord, null);
        }

    }
    public void MenuQuit()
    {
        Application.Quit();
    }
}
