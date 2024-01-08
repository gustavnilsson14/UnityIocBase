using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerClickHandler : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onClick = new UnityEvent();
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        onClick.Invoke();
    }
}
