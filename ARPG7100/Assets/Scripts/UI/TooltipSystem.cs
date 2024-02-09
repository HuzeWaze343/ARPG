using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;
    public Tooltip tooltip;
    public ItemTooltip itemTooltip;

    public void Awake()
        {
        instance = this;
        }
    public static void ShowItemTooltip(Item item)
        {
        instance.itemTooltip.UpdateTooltip(item);
        instance.itemTooltip.gameObject.SetActive(true);
        }
    public static void HideItemTooltip()
        {
        instance.itemTooltip.gameObject.SetActive(false);
        }
    public static void ShowTooltip(string header, string tags, string subheader, string content)
        {
        instance.tooltip.SetText(header, tags, subheader, content);
        instance.tooltip.gameObject.SetActive(true);
        }
    public static void HideTooltip()
        {
        instance.tooltip.gameObject.SetActive(false);
        }
    }
