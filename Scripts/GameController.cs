using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public Tilemap frame, falling, active, inactive;
    public enum States { start, spawn, fall, active, inactive, delete };
    public States state = States.start;
    public SpawnBlocks spawnBlocks;
    public FallingBlocks fallingBlocks;
    public ActiveBlocks activeBlocks;
    public InactiveBlocks inactiveBlocks;
    public DeleteBlocks deleteBlocks;
    public Save save;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float TickSpeed()
    {
        return Mathf.Max(0.25f, 1.0f - (deleteBlocks.score / 1000f));
        
    }
    void Start()
    {
        InvokeRepeating(nameof(GameStates), 1f, TickSpeed());
    }

    void GameStates()
    {
        Debug.Log("Tick speed:" + TickSpeed());
        switch (state)
        {
            case States.start:
                save.AutoLoad();
                state = States.spawn;
                Debug.Log("Start state");
                break;
            case States.spawn:
                spawnBlocks.SpawnShape();
                state = States.fall;
                Debug.Log("Spawn state");
                break;
            case States.fall:
                fallingBlocks.FallBlocks();
                Debug.Log("Fall state");
                break;
            case States.active:
                activeBlocks.FallActive();
                Debug.Log("Active state");
                break;
            case States.inactive:
                inactiveBlocks.FallAll();
                state = States.delete;
                Debug.Log("Inactive state");
                break;
            case States.delete:
                deleteBlocks.CheckMatches();
                Debug.Log("Delete state");
                break;
            default:
                state = States.start;
                break;
        }
    }
}
