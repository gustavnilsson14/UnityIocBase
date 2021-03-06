using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class BehaviourBase : MonoBehaviour, IBase, IPointerClickHandler
{
    public DestroyEvent onDestroy { get; set; }
    public CollisionEvent onCollision { get; set; }
    public ParticleCollisionEvent onParticleCollision { get; set; }
    public TriggerEvent onTriggerEnter { get; set; }
    public TriggerEvent onTriggerExit { get; set; }
    public ClickEvent onClick { get; set; }
    public int uniqueId { get; set; }
    public GameObject GetGameObject() => gameObject;

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnCollisionEnter(Collision collision)
    {
        if (onCollision == null)
            return;
        onCollision.Invoke(this, collision);
    }
    private void OnParticleCollision(GameObject other)
    {
        if (onParticleCollision == null)
            return;
        onParticleCollision.Invoke(this, other);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider == null)
            return;
        onTriggerEnter.Invoke(this, collider);
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider == null)
            return;
        onTriggerExit.Invoke(this, collider);
    }
    private void OnDestroy()
    {
        if (onDestroy == null)
            return;
        onDestroy.Invoke(this);
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke(this);
    }
    public T As<T>() where T : class
    {
        return this as T;
    }
    public static bool GetBehaviourOfType<T>(out T behaviour, GameObject gameObject) where T : class
    {
        behaviour = null;
        List<BehaviourBase> behaviourBases = gameObject.GetComponents<BehaviourBase>().ToList();

        foreach (BehaviourBase behaviourBase in behaviourBases)
        {
            if (behaviourBase is T)
            {
                behaviour = behaviourBase as T;
                return true;
            }
        }
        return false;
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

public static class BehaviourBaseNullExtension
{
    public static bool IsNull(this BehaviourBase targetObject)
    {
        if (targetObject == null)
            return true;
        
        return false;
    }
}
