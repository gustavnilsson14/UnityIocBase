using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLogic : InterfaceLogicBase
{
    public static CollisionLogic I;
    public bool debugCollisionChecks = false;
    public bool CollidesAtPoint(ICollider collider) { 
        return GetColliders(collider.GetCenter().position, collider.GetRadius(), collider.GetCollisionMask()).Count > 0;
    }
    public List<Collider> GetColliders(ICollider collider) { 
        return GetColliders(collider.GetCenter().position, collider.GetRadius(), collider.GetCollisionMask());
    }
    public List<Collider> GetColliders(Vector3 position, float radius, LayerMask layerMask)
    {
        if (debugCollisionChecks)
            DebugCollisionCheck(position, radius);
        return Physics.OverlapBox(position, Vector3.one * (radius / 2), Quaternion.identity, layerMask).ToList();
    }
    public List<Collider> GetColliders(Vector3 position, Vector3 extents, LayerMask layerMask)
    {
        if (debugCollisionChecks)
            DebugCollisionCheck(position, extents);
        return Physics.OverlapBox(position, extents / 2, Quaternion.identity, layerMask).ToList();
    }


    List<GameObject> gizmos = new List<GameObject>();
    private void DebugCollisionCheck(Vector3 position, float radius)
    {
        GameObject gizmo = new GameObject("DEBUGGIZMO");
        gizmo.AddComponent<TimedLife>().lifetime = 1;
        gizmo.transform.localScale = Vector3.one * radius;
        gizmo.transform.position = position;
        gizmos.Add(gizmo);
    }
    private void DebugCollisionCheck(Vector3 position, Vector3 extents)
    {
        GameObject gizmo = new GameObject("DEBUGGIZMO");
        gizmo.AddComponent<TimedLife>().lifetime = 1;
        gizmo.transform.localScale = extents;
        gizmo.transform.position = position;
        gizmos.Add(gizmo);
    }
    private void OnDrawGizmos()
    {
        foreach (GameObject gizmo in gizmos)
        {
            if (gizmo == null)
                continue;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(gizmo.transform.position, gizmo.transform.localScale);
        }
    }
}

public interface ICollider : IAnimated
{
    float GetRadius();
    Transform GetCenter();
    LayerMask GetCollisionMask();
}