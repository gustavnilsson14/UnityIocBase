using UnityEngine;
public class TimedLife : MonoBehaviour
{
    public float lifetime = 1;
    public float expiry;
    void Start() {
        expiry = Time.time + lifetime;
        Destroy(gameObject, lifetime);
    }
}
