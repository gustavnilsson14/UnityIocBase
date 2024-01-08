using UnityEngine;
using UnityEngine.UIElements;

public static class BoxColliderExtensions
{
    public static Vector3 GetRandomPoint(this BoxCollider box)
    {
        return new Vector3(
            box.center.x + Random.Range(-box.size.x, box.size.x),
            box.center.y + Random.Range(-box.size.x, box.size.x),
            box.center.z + Random.Range(-box.size.z, box.size.z)
        ) + box.transform.position;
    }
}

public static class SphereColliderExtensions
{
    public static Vector3 GetRandomPoint(this SphereCollider sphere, bool edgeOnly = false)
    {
        Vector3 random = Random.insideUnitSphere;
        if (edgeOnly) random = random.normalized;
        return sphere.transform.position + (random * sphere.radius);
    }
    public static Vector3 GetRandomPointOnCircle(this SphereCollider sphere, bool edgeOnly = false)
    {
        Vector2 random = Random.insideUnitCircle;
        if (edgeOnly) random = random.normalized;
        random *= sphere.radius;
        return new Vector3(random.x + sphere.transform.position.x, sphere.transform.position.y, random.y + sphere.transform.position.z);
    }
}
