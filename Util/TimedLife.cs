using UnityEngine;
public class TimedLife : MonoBehaviour
{
    public float lifetime = 1;
    void Start() => Destroy(gameObject, lifetime);
}
