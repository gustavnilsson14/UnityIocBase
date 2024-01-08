using System;
using UnityEngine;

public class PhysicsUtil
{
    public static Collider[] OverlapCapsule(CapsuleCollider col, Quaternion rotation, int layerMask) {
        var point1 = col.transform.position + (col.transform.up * (col.height / 4));
        var point2 = col.transform.position + (col.transform.up * -(col.height / 4));
        return Physics.OverlapCapsule(point1, point2, col.radius, layerMask);
    }

    public static Collider[] OverlapBox(BoxCollider col, Quaternion rotation, LayerMask layerMask)
    {
        return Physics.OverlapBox(col.transform.position + col.center, col.size / 2, rotation, layerMask);
    }

    public static Collider[] OverlapSphere(SphereCollider col, LayerMask layerMask)
    {
        return Physics.OverlapSphere(col.transform.position, col.radius, layerMask);
    }
    public static Collider[] Overlap(Collider col, LayerMask layerMask, Quaternion rot)
    {
        if (col is SphereCollider) return OverlapSphere(col as SphereCollider, layerMask);
        if (col is SphereCollider) return OverlapBox(col as BoxCollider, rot, layerMask);
        return new Collider[0];

    }
}
