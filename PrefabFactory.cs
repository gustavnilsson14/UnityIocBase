using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PrefabFactory : InterfaceLogicBase
{
    public static PrefabFactory I;
    public InstantiateEvent onInstantiate = new InstantiateEvent();
    public InstantiateEvent onRegisterInternalListeners = new InstantiateEvent();
    public Transform gameRoot;


    protected override IEnumerator DelayedPostStart()
    {
        yield return 0;
        PostStart();
    }

    protected override void PostStart()
    {
        GameObject.FindObjectsOfType(typeof(GameObject)).ToList().ForEach(x => StartCoroutine(RegisterNewInstance(x as GameObject)));
    }

    public GameObject Create(GameObject prefab)
    {
        return Create(prefab, transform, transform);
    }
    public GameObject Create(GameObject prefab, Transform parent)
    {
        return Create(prefab, parent, parent);
    }
    public GameObject Create(GameObject prefab, Transform parent, Transform origin)
    {
        return Create(prefab, parent, origin.position, origin.rotation);
    }

    public GameObject Create(GameObject prefab, Transform parent, Vector3 origin)
    {
        return Create(prefab, parent, origin, Quaternion.identity);
    }

    public GameObject Create(GameObject prefab, Transform parent, Vector3 origin, Quaternion rotation)
    {
        GameObject newGameObject = Instantiate(prefab, parent);
        newGameObject.transform.position = origin;
        newGameObject.transform.rotation = rotation;
        StartCoroutine(RegisterNewInstance(newGameObject));
        return newGameObject;
    }

    public void ManualRegister(GameObject newGameObject) {
        StartCoroutine(RegisterNewInstance(newGameObject));
    }

    public IEnumerator RegisterNewInstance(GameObject newGameObject)
    {
        onInstantiate.Invoke(newGameObject);
        yield return 0;
        onRegisterInternalListeners.Invoke(newGameObject);
    }

    public float deltaTimeLimit;
    public bool logDeltaTimeLimit = false;
    private void Update()
    {
        if (!logDeltaTimeLimit)
            return;
        if (Time.deltaTime < deltaTimeLimit)
            return;
        Debug.LogError($"over limit!!! {Time.deltaTime}");
        Debug.Break();
    }
}
public class InstantiateEvent : UnityEvent<GameObject> { }