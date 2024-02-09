using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
    [SerializeField]
    string header;
    [SerializeField]
    string tags;
    [SerializeField]
    string subHeader;
    [SerializeField]
    string content;
    public void OnPointerEnter(PointerEventData eventData)
        {
        TooltipSystem.ShowTooltip(header, tags, subHeader, content);
        }
    public void OnPointerExit(PointerEventData eventData)
        {
        TooltipSystem.HideTooltip();
        }
    }
