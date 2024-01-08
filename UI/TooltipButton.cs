using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipButton : Button, ITooltip
{
    [SerializeField]
    public string text;
    public string GetText() => text;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        TooltipLogic.I.OnPointerEnter(this, eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        TooltipLogic.I.OnPointerExit(this, eventData);
    }

    protected override void OnDestroy()
    {
        TooltipLogic.I.OnPointerExit(this, null);
    }

    public void SetTitle(string newText) => GetComponentInChildren<TMP_Text>().text = newText;
}
