using UnityEngine;

public class FakeParent : MonoBehaviour
{
    public Transform fakeParent;
    void Awake() => transform.position = fakeParent.position;
    void Update() => transform.position = fakeParent.position;
}
