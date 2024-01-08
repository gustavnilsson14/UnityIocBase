using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static void Clear(this GameObject go)
    {
        foreach (Transform child in go.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public static void SetLayerRecursive(this GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform) {
            child.gameObject.SetLayerRecursive(layer);
        }
    }

    public static Vector3 GetCenter(this GameObject go) {
        if (go.TryGetComponent(out Collider col)) return col.bounds.center;
        return go.transform.position;
    }



}
