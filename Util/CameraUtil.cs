using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraUtil
{
    public static bool IsInFrustrum(Camera camera, Vector3 target) {
        Vector3 screenPoint = camera.WorldToViewportPoint(target);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
    public static bool TryGetMouseRaycastHit(Camera cam, LayerMask layerMask, out RaycastHit hit)
    {
        Vector2 position = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(new Vector2(position.x, position.y));
        return Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
    }

}
