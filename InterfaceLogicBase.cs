using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class InterfaceLogicBase : MonoBehaviour
{
    public List<GameObject> myInstances = new List<GameObject>();

    protected virtual void Awake()
    {
        FieldInfo field = GetType().GetField("I", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        field.SetValue(null, this);
    }
    protected virtual void Start()
    {
        RegisterInstances();
        RegisterListeners();
        StartCoroutine(DelayedPostStart());
    }

    protected virtual IEnumerator DelayedPostStart()
    {
        yield return new WaitForSeconds(0.1f);
        PostStart();
    }

    protected virtual void PostStart() { }

    protected virtual void RegisterInstances() { }

    protected virtual void RegisterListeners()
    {
        PrefabFactory.I.onInstantiate.AddListener(OnInstantiate);
        PrefabFactory.I.onRegisterInternalListeners.AddListener(OnRegisterInternalListeners);
    }

    protected void Initialize<T>(Action<T> initMethod, IBase newBase)
    {
        if (!(newBase is T))
            return;
        if ((T)newBase == null)
            return;
        initMethod((T)newBase);
    }

    protected void RegisterListeners<T>(Action<T> registerMethod, IBase newBase) {
        if (!(newBase is T))
            return;
        if ((T)newBase == null)
            return;
        registerMethod((T)newBase);
    }

    protected virtual void OnInstantiate(GameObject newInstance)
    {
        if (!newInstance.TryGetComponent(out IBase newBase))
            return;
        myInstances.Add(newInstance);
        newInstance.GetComponents<IBase>().ToList().ForEach(x => OnInstantiate(newInstance, x));
    }
    protected virtual void OnInstantiate(GameObject newInstance, IBase newBase) {
        if (newBase.onDestroy != null) {
            newBase.onDestroy.AddListener(UnRegister);
            return;
        }
        newBase.uniqueId = newInstance.GetInstanceID();
        newBase.onDestroy = new DestroyEvent(newBase, "Destroy");
        newBase.onCollision = new CollisionEvent(newBase, "Collision");
        newBase.onParticleCollision = new ParticleCollisionEvent(newBase, "ParticleCollision");
        newBase.onTriggerEnter = new TriggerEvent(newBase, "TriggerEnter");
        newBase.onTriggerExit = new TriggerEvent(newBase, "TriggerExit");
        newBase.onClick = new ClickEvent(newBase, "Click");
    }

    protected virtual void OnRegisterInternalListeners(GameObject newInstance)
    {
        if (newInstance == null)
            return;
        if (!newInstance.TryGetComponent(out IBase newBase))
            return;
        newInstance.GetComponents<IBase>().ToList().ForEach(x => OnRegisterInternalListeners(newInstance, x));
    }
    protected virtual void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        newBase.onDestroy.AddListener(UnRegister);
    }

    protected virtual void UnRegister(IBase b)
    {
        UnRegister(b, null);
    }
    protected virtual void UnRegister(IBase b, List<IList> instanceLists)
    {
        myInstances.Remove(b.GetGameObject());
        if (instanceLists == null)
            return;
        instanceLists.ForEach(x => x.Remove(b));
    }

    public T GetById<T>(int uniueqId) where T : IBase
    {
        GameObject result = myInstances.Find(x => x.GetComponent<IBase>().uniqueId == uniueqId);
        if (result == null)
            return default(T);
        return result.GetComponent<T>();
    }
    public bool TryGetById<T>(int uniueqId, out T o) where T : IBase
    {
        o = GetById<T>(uniueqId);
        return o != null;
    }
}

public interface IBase
{
    int uniqueId { get; set; }
    GameObject GetGameObject();
    DestroyEvent onDestroy { get; set; }
    ParticleCollisionEvent onParticleCollision { get; set; }
    CollisionEvent onCollision { get; set; }
    TriggerEvent onTriggerEnter { get; set; }
    TriggerEvent onTriggerExit { get; set; }
    ClickEvent onClick { get; set; }
    public T As<T>() where T : class;
}
public class DestroyEvent : AnimationEvent<IBase>
{
    public DestroyEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}
public class CollisionEvent : AnimationEvent<IBase, Collision>
{
    public CollisionEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}
public class ParticleCollisionEvent : AnimationEvent<IBase, GameObject>
{
    public ParticleCollisionEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}
public class TriggerEvent : AnimationEvent<IBase, Collider>
{
    public TriggerEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}
public class ClickEvent : AnimationEvent<IBase>
{
    public ClickEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}
public class BaseEvent : AnimationEvent<IBase>
{
    public BaseEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}