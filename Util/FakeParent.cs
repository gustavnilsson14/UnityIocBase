using System.Collections.Generic;
using UnityEngine;

public class FakeParent : MonoBehaviour
{
    public Transform fakeParent;
    public List<SnapAxis> ignoredAxis;
    void Start() => transform.position = fakeParent.position;
    void Update() => transform.position = GetPosition();
    Vector3 GetPosition()
    {
        if (ignoredAxis.Contains(SnapAxis.All))
            return transform.position;
        if (ignoredAxis.Count == 0)
            return fakeParent.position;
        return new Vector3(
            (ignoredAxis.Contains(SnapAxis.X) ? transform.position.x : fakeParent.position.x),
            (ignoredAxis.Contains(SnapAxis.Y) ? transform.position.y : fakeParent.position.y),
            (ignoredAxis.Contains(SnapAxis.Z) ? transform.position.z : fakeParent.position.z)
        );
    }
}
