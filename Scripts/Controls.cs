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

    void Update()
    {
        if (Touchscreen.current == null) return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            swipeStart = touch.position.ReadValue();
            isSwiping = true;
        }
        else if (touch.press.wasReleasedThisFrame && isSwiping)
        {
            Vector2 swipeEnd = touch.position.ReadValue();
            Vector2 delta = swipeEnd - swipeStart;

            isSwiping = false;

            // Only care about direction, not distance
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x > 0)
                    Move(Vector3Int.right);
                else
                    Move(Vector3Int.left);
            }
        }
    }
}
