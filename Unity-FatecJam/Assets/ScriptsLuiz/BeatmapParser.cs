using UnityEngine;
using System.IO;

public static class BeatmapParser {
    public static Beatmap LoadBeatmap(string filePath)
    {
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            return JsonUtility.FromJson<Beatmap>(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot find beatmap file at: " + filePath);
            return null;
        }
    }
}