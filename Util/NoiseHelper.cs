using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class NoiseHelper
{
    public static List<List<float>> GetLayeredNoise(List<NoiseLayer> layers)
    {
        List<List<float>> noise = null;
        foreach (var l in layers) {
            GetNoise2D(l.size, l.originRandomization, l.scale, l.valueOffset, out List<List<float>> noiseLayer);
            MultiplyNoise(noiseLayer, l.strength);
            if (noise == null)
            {
                noise = noiseLayer;
                continue;
            }
            AddNoise(noise, noiseLayer);
        }

        return noise;
    }

    public static bool GetNoise2D(Vector2Int dimensions, int randomOriginRange, float scale, out List<List<float>> noise)
    {
        return GetNoise2D(dimensions, randomOriginRange, scale, 0, out noise);
    }
    public static bool GetNoise2D(Vector2Int dimensions, int randomOriginRange, float scale, float valueOffset, out List<List<float>> noise)
    {
        Vector2Int origin = new Vector2Int(RandomUtil.RandomInt(-randomOriginRange, randomOriginRange), RandomUtil.RandomInt(-randomOriginRange, randomOriginRange));
        return GetNoise2D(dimensions, origin, scale, valueOffset, out noise);
    }
    public static bool GetNoise2D(Vector2Int dimensions, Vector2Int origin, float scale, float valueOffset, out List<List<float>> noise) {
        noise = new List<List<float>>();
        if (dimensions.x <= 0 || dimensions.y <= 0) return false;
        float y = 0f;
        while (y < dimensions.y)
        {
            var row = new List<float>();
            float x = 0f;
            while (x < dimensions.x)
            {
                float xCoord = origin.x + x / dimensions.x * scale;
                float yCoord = origin.y + y / dimensions.y * scale;
                var value = Mathf.PerlinNoise(xCoord, yCoord);
                row.Add(value + valueOffset);
                x++;
            }
            noise.Add(row);
            y++;
        }
        return true;
    }

    public static List<List<float>> CopyNoise(List<List<float>> noise) {
        List<List<float>> copy = new List<List<float>>();
        noise.ForEach(inner => {
            List<float> innerCopy = new List<float>();
            inner.ForEach(i => innerCopy.Add(i));
            copy.Add(innerCopy);
        });
        return copy;
    }

    public static void NormalizeNoise(List<List<float>> noise, float threshold)
    {
        for (int y = 0; y < noise.Count; y++)
        {
            for (int x = 0; x < noise[y].Count; x++)
            {
                var value = noise[y][x];
                noise[y][x] = value > threshold ? 1 : 0;
            }
        }
    }

    public static void AddNoise(List<List<float>> noise1, List<List<float>> noise2)
    {
        for (int y = 0; y < noise1.Count; y++)
        {
            for (int x = 0; x < noise1.Count; x++)
            {
                noise1[x][y] += noise2[x][y];
            }
        }
    }

    public static void MultiplyNoise(List<List<float>> noise, float mul)
    {
        for (int y = 0; y < noise.Count; y++)
        {
            for (int x = 0; x < noise.Count; x++)
            {
                noise[x][y] *= mul;
            }
        }
    }

    public static List<List<float>> SmoothNoise(List<List<float>> noise, int passes, int range)
    {
        List<List<float>> smoothed = CopyNoise(noise);
        while (passes != 0)
        {
            passes--;
            for (int y = 0; y < noise.Count; y++)
            {
                for (int x = 0; x < noise[y].Count; x++)
                {
                    SmoothNoiseAt(noise, smoothed, new Vector2Int(x,y), range);
                }
            }
            noise = CopyNoise(smoothed);
        }
        return smoothed;
    }

    public static void SmoothNoiseAt(List<List<float>> original, List<List<float>> smoothed, Vector2Int pos, int range)
    {
        float val = original[pos.y][pos.x];
        List<float> neighbors = GetNeighbors(original, pos, range);
        smoothed[pos.y][pos.x] = (val + neighbors.Sum()) / (neighbors.Count + 1);
    }

    public static Texture2D ToTexture(List<List<float>> noise)
    {
        var texture = new Texture2D(noise[0].Count, noise.Count);
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                var value = noise[y][x];
                texture.SetPixel(x, y, GetColor(value));
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    }

    private static Color GetColor(float value)
    {
        switch (value) {
            case 1:
                return Color.white;
            case 0.5f:
                return Color.magenta;
            default:
                return Color.black;
        }
    }

    public static List<NoiseArea> GetAreas(List<List<float>> noise)
    {
        var result = new List<NoiseArea>();
        List<Vector2Int> addedCoords = new List<Vector2Int>();
        while (FindNewArea(noise, addedCoords, out NoiseArea newArea)) {
            result.Add(newArea);
        }
        return result;
    }

    private static bool FindNewArea(List<List<float>> noise, List<Vector2Int> addedCoords, out NoiseArea newArea)
    {
        newArea = null;
        Vector2Int unknownCoord = Vector2Int.zero - Vector2Int.one;
        for (int y = 0; y < noise.Count; y++)
        {
            for (int x = 0; x < noise[y].Count; x++)
            {
                var coord = new Vector2Int(x, y);
                if (addedCoords.Contains(coord)) continue;
                unknownCoord = coord;
                break;
            }
            if (unknownCoord != Vector2Int.zero - Vector2Int.one) break; ;
        }
        if (unknownCoord == Vector2Int.zero - Vector2Int.one) return false;
        newArea = GetAreaFromCoord(unknownCoord, noise, addedCoords);
        return true;
    }

    private static NoiseArea GetAreaFromCoord(Vector2Int current, List<List<float>> noise, List<Vector2Int> addedCoords)
    {
        var newArea = new NoiseArea() {
            value = noise[current.y][current.x]
        };
        List<Vector2Int> open = new List<Vector2Int>() { current };
        while (open.Any()) {
            current = open.FirstOrDefault();
            addedCoords.Add(current);
            List<Vector2Int> neighbors = GetNeighbors(current, noise, noise[current.y][current.x]);
            var newNodes = neighbors.Where(x => !addedCoords.Contains(x) && !open.Contains(x));
            open.AddRange(newNodes);
            open.Remove(current);
            newArea.coords.Add(current);
        }
        newArea.center = RandomUtil.Shuffle(newArea.coords)[0];
        return newArea;
    }

    public static List<Vector2Int> GetNeighbors(Vector2Int current, List<List<float>> noise, float value, bool includeDiagonals = true)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        for (int y = -1; y <= 1; y++)
        {
            var yCoord = current.y + y;
            if (yCoord < 0 || yCoord > noise.Count - 1) continue;
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0) continue;
                if (!includeDiagonals && IsDiagonal(x,y)) continue;
                var xCoord = current.x + x;
                if (xCoord < 0 || xCoord > noise[0].Count - 1) continue;
                if (noise[yCoord][xCoord] != value) continue;
                neighbors.Add(new Vector2Int(xCoord, yCoord));
            }
        }
        return neighbors;
    }

    private static bool IsDiagonal(int x, int y)
    {
        return x != 0 && y != 0;
    }

    public static void CreatePaths(List<List<float>> noise, List<NoiseArea> walkableAreas, NoiseArea startArea)
    {
        var currentArea = walkableAreas.FirstOrDefault();
        var connectedAreas = new List<NoiseArea>() { currentArea };
        while (currentArea != null) {
            currentArea = walkableAreas.Find(x => !connectedAreas.Contains(x));
            if (currentArea == null) continue;
            NoiseArea goalArea = null;
            connectedAreas.ForEach(connectedArea => {
                if (goalArea == null) {
                    goalArea = connectedArea;
                    return;
                }
                var distanceA = Vector2Int.Distance(goalArea.center, currentArea.center);
                var distanceB = Vector2Int.Distance(connectedArea.center, currentArea.center);
                if (distanceA < distanceB) return;
                goalArea = connectedArea;
            });
            if (goalArea == null) return;
            CreatePath(noise, currentArea, goalArea);
            connectedAreas.Add(currentArea);
        }
    }

    private static void CreatePath(List<List<float>> noise, NoiseArea startArea, NoiseArea goalArea)
    {
        var current = startArea.center;
        var max = 10000;
        while (current != goalArea.center && max > 0)
        {
            max--;
            if (current.x != goalArea.center.x)
            {
                current.x += goalArea.center.x < current.x ? -1 : 1;
                noise[current.y][current.x] = startArea.value;
            }
            if (current.y != goalArea.center.y)
            {
                current.y += goalArea.center.y < current.y ? -1 : 1;
                noise[current.y][current.x] = startArea.value;
            }
            if (goalArea.coords.Contains(current)) break;
        }
    }

    public static void Erode(List<List<float>> noise, List<int> erosionCycles, float targetValue = 1)
    {
        while (erosionCycles.Count > 0) {
            var erosionLevel = erosionCycles.FirstOrDefault();
            erosionCycles.RemoveAt(0);
            List<Vector2Int> coords = new List<Vector2Int>();
            for (int y = 0; y < noise.Count; y++)
            {
                for (int x = 0; x < noise[y].Count; x++)
                {
                    var neighbors = GetNeighbors(new Vector2Int(x, y), noise, noise[y][x], false);
                    if (neighbors.Count <= erosionLevel) coords.Add(new Vector2Int(x, y));
                }
            }
            coords.ForEach(coord => {
                noise[coord.y][coord.x] = 0;
            });
        }
    }

    public static void ErodeAreas(List<List<float>> noise, List<NoiseArea> noiseAreas, int minCoords)
    {
        List<NoiseArea> toRemove = noiseAreas.Where(area => area.coords.Count < minCoords).ToList();
        toRemove.Select(x => x.coords).ToList().ForEach(coords => {
            coords.ForEach(coord => {
                noise[coord.y][coord.x] = 0;
            });
        });
        noiseAreas.RemoveAll(area => toRemove.Contains(area));
    }

    public static void CreateEdges(List<List<float>> noise)
    {
        for (int y = 0; y < noise.Count; y++)
        {
            for (int x = 0; x < noise[y].Count; x++)
            {
                var value = noise[y][x];
                if (value != 0) continue;
                var neighbors = GetNeighbors(new Vector2Int(x, y), noise, 1);
                if (neighbors.Count == 0) continue;
                noise[y][x] = 0.1f;
            }
        }
    }
    public static List<float> GetNeighbors(List<List<float>> noise, Vector2Int pos, int range = 1)
    {
        List<float> neighbors = new List<float>();
        for (int y = pos.y - range; y <= pos.y + range; y++)
        {
            for (int x = pos.x - range; x <= pos.x + range; x++)
            {
                if (y <= 0) continue;
                if (x <= 0) continue;

                if (y > noise.Count - 1) continue;
                if (x > noise[y].Count - 1) continue;
                neighbors.Add(noise[y][x]);
            }
        }
        return neighbors;
    }
    public static void GetExtremes(List<List<float>> noise, out float highest, out float lowest)
    {
        highest = Mathf.NegativeInfinity;
        lowest = Mathf.Infinity;
        
        for (int y = 0; y < noise.Count; y++)
        {
            for (int x = 0; x < noise[y].Count; x++)
            {
                float height = noise[y][x];
                if (height < lowest) lowest = height;
                if (height > highest) highest = height;
            }
        }
    }
}

public class NoiseLayer {
    public float scale;
    public float strength;
    public float valueOffset;
    public Vector2Int size;
    public int originRandomization = 5000;
}

public class NoiseArea {
    public float value;
    public Vector2Int center;
    public List<Vector2Int> coords = new List<Vector2Int>();
}