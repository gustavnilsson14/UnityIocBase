using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 Round(this Vector3 pos) => new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    public static Vector3 GetMean(List<Vector3> positions)
    {
        if (positions.Count == 0) return Vector3.zero;
        Vector3 meanVector = Vector3.zero;
        foreach (Vector3 pos in positions)
            meanVector += pos;
        return meanVector / positions.Count;
    }

}