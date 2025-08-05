using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Save : MonoBehaviour
{
    public DeleteBlocks deleteBlocks;
    public Tilemap inactive;
    public Score score;

    public void AutoSave()
    {
        SaveData data = new()
        {
            score = deleteBlocks.score,
            multiplier = deleteBlocks.multiplier
        };

        foreach (var pos in inactive.cellBounds.allPositionsWithin)
        {
            TileBase tile = inactive.GetTile(pos);
            if (tile != null)
            {
                data.tiles.Add(new TileData
                {
                    x = pos.x,
                    y = pos.y,
                    color = tile,
                });
            }
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }
    public void AutoLoad()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        deleteBlocks.score = data.score;
        deleteBlocks.multiplier = data.multiplier;
        score.UpdateScore(data.score);
        foreach (var tileData in data.tiles)
        {
            Vector3Int pos = new(tileData.x, tileData.y, 0);
            TileBase tile = tileData.color;
            inactive.SetTile(pos, tile);
        }
    }

}
[System.Serializable]
public class SaveData
{
    public int score;
    public int multiplier;
    public List<TileData> tiles = new();
}

[System.Serializable]
public class TileData
{
    public int x, y;
    public TileBase color;
}

