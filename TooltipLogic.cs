using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipLogic : InterfaceLogicBase
{
    public static TooltipLogic I;
    public RectTransform myTooltip;

    public void OnPointerEnter(ITooltip tooltip, PointerEventData eventData)
    {
        myTooltip.gameObject.SetActive(true);
        myTooltip.position = eventData.position;
        myTooltip.GetComponentInChildren<TMP_Text>().text = tooltip.GetText();
    }

    public void OnPointerExit(ITooltip tooltip, PointerEventData eventData)
    {
        myTooltip.gameObject.SetActive(false);
    }

}

public interface ITooltip: IPointerEnterHandler, IPointerExitHandler
{
    string GetText();
}
