using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Controls : MonoBehaviour
{
    public Tilemap falling, inactive, frame;
    public InputAction inputAction;
    private Vector2 swipeStart;
    private bool isSwiping = false;
    private float swipeThreshold = 30f; // pixels
    public FallingBlocks fallingBlocks;

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.performed += OnMove;
    }

    private void OnDisable()
    {
        inputAction.performed -= OnMove;
        inputAction.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        Debug.Log(input);
        if (input == -1)
        {
            Move(Vector3Int.left);
        }
        else if (input == 1)
        {
            Move(Vector3Int.right);
        }
    }

    Dictionary<Vector3Int, TileBase> ActiveTiles()
    {
        Dictionary<Vector3Int, TileBase> dict = new();
        foreach (var position in falling.cellBounds.allPositionsWithin)
        {
            var tile = falling.GetTile(position);
            if (tile != null)
            {
                dict.Add(position, tile);
            }
        }
        return dict;
    }

    void Move(Vector3Int direction)
    {
        var tiles = ActiveTiles();

        // Sort tile positions based on direction
        var orderedKeys = direction.x > 0
            ? new List<Vector3Int>(tiles.Keys).OrderByDescending(pos => pos.x)
            : new List<Vector3Int>(tiles.Keys).OrderBy(pos => pos.x);

        // Check if all can move
        foreach (var pos in orderedKeys)
        {
            Vector3Int target = pos + direction;
            if (frame.GetTile(target) || inactive.GetTile(target))
                return; // Abort if any tile is blocked
        }

        // Move tiles
        foreach (var pos in orderedKeys)
        {
            var tile = tiles[pos];
            falling.SetTile(pos + direction, tile);
            falling.SetTile(pos, null);
        }
    }
    void Fast()
    {
        if (Keyboard.current != null && Keyboard.current.sKey.isPressed)
        {
            fallingBlocks.fast = true;
        }
    }
    void ShuffleKeyboard()
    {
        if (Keyboard.current != null && Keyboard.current.wKey.wasPressedThisFrame)
        {
            Shuffle();
        }
    }
    void Shuffle()
    {
        Debug.Log("Shuffle");
        var tiles = ActiveTiles();
        if (tiles.Count <= 1) return;

        var positions = tiles.Keys.ToList();
        var values = tiles.Values.ToList();

        // Rotate the colors
        var last = values[^1];
        values.RemoveAt(values.Count - 1);
        values.Insert(0, last);

        // Apply back to tilemap
        for (int i = 0; i < positions.Count; i++)
        {
            falling.SetTile(positions[i], values[i]);
        }
    }


    void Update()
    {
        Fast(); // keyboard
        ShuffleKeyboard(); // keyboard

        if (Touchscreen.current == null) return;

        var touch = Touchscreen.current.primaryTouch;

        // Start swipe
        if (touch.press.wasPressedThisFrame)
        {
            swipeStart = touch.position.ReadValue();
            isSwiping = true;
        }
        // End swipe or tap
        else if (touch.press.wasReleasedThisFrame && isSwiping)
        {
            Vector2 swipeEnd = touch.position.ReadValue();
            Vector2 delta = swipeEnd - swipeStart;
            isSwiping = false;

            // Check swipe direction and threshold
            if (Mathf.Abs(delta.x) > swipeThreshold && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x > 0)
                    Move(Vector3Int.right);
                else
                    Move(Vector3Int.left);
            }
            else if (Mathf.Abs(delta.y) > swipeThreshold && Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
            {
                if (delta.y < 0)
                    fallingBlocks.fast = true;
            }
            else
            {
                // Not a swipe — treat as tap
                Debug.Log("Tapped at: " + swipeEnd);
                Shuffle();
            }
        }
    }
}
