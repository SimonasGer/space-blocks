using UnityEngine;

public class BlockData : MonoBehaviour
{
[System.Serializable]
public class ShapeInfo
{
    public Vector3Int[] blockPositions; // Positions of blocks relative to the shape's origin
    public string name; // Name of the shape
}
[System.Serializable]
public class ShapeData
{
    public ShapeInfo[] shapes; // List of predefined shapes
}
}
