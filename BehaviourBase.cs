using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BehaviourBase : MonoBehaviour, IBase, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DisableEvent onDisable { get; set; }
    public CollisionEvent onCollision { get; set; }
    public ParticleCollisionEvent onParticleCollision { get; set; }
    public TriggerEvent onTriggerEnter { get; set; }
    public TriggerEvent onTriggerExit { get; set; }
    public ClickEvent onClick { get; set; }
    public int uniqueId { get; set; }
    public MouseDownEvent onMouseDown { get; set; }
    public MouseUpEvent onMouseUp { get; set; }
    public BeginDragEvent onBeginDrag { get; set; }
    public DragEvent onDrag { get; set; }
    public EndDragEvent onEndDrag { get; set; }

    public GameObject GetGameObject() => gameObject;
    public Transform GetTransform() => transform;

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
        if (collider == null) return;
        if (onTriggerEnter == null) return;
        onTriggerEnter.Invoke(this, collider);
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider == null) return;
        if (onTriggerExit == null) return;
        onTriggerExit.Invoke(this, collider);
    }
    private void OnDisable()
    {
        if (onDisable == null)
            return;
        onDisable.Invoke(this);
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (onClick == null) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;
        onClick.Invoke(this, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onMouseDown == null) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;
        onMouseDown.Invoke(this, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onMouseUp == null) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;
        onMouseUp.Invoke(this, eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag == null) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;
        onBeginDrag.Invoke(this, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag == null) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;
        onDrag.Invoke(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag == null) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;
        onEndDrag.Invoke(this, eventData);
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

    T IBase.GetComponent<T>()
    {
        return GetComponent<T>();
    }

    public Vector3 GetPosition() => transform.position;

    public string GetName() => GetGameObject().name;
}