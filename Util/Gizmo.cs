using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public Color color = Color.magenta;
    public float radius = 1;

    public bool line = false;
    public bool sphere = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        if (sphere) Gizmos.DrawSphere(transform.position, radius);
        if (line) Gizmos.DrawLine(transform.position, transform.position +(transform.forward * radius * 4));
    }
}
