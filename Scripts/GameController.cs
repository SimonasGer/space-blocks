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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(GameStates), 1f, 1f);
    }

    void GameStates()
    {
        switch (state)
        {
            case States.start:
                CheckSave();
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
                // inactive
                state = States.delete;
                Debug.Log("Inactive state");
                break;
            case States.delete:
                // delete
                state = States.spawn;
                Debug.Log("Delete state");
                break;
            default:
                state = States.start;
                break;
        }
    }

    void CheckSave()
    {

    }
}
