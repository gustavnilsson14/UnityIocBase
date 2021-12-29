using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtil
{
    public static bool IsInFrustrum(Camera camera, Vector3 target) {
        Vector3 screenPoint = camera.WorldToViewportPoint(target);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
