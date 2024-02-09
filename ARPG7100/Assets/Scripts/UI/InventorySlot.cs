using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    Sprite iconSprite;
    Item item;
    public void AddItem(Item newItem)
        {
        item = newItem;

        string filename = item.BaseItem.name;
        filename = string.Join("", filename.Split(' '));
        iconSprite = Resources.Load<Sprite>("ItemSprites/" + filename);

        if (iconSprite != null)
            icon.sprite = iconSprite;
        else
            icon.sprite = null;

        switch (item.Rarity)
            {
            default:
                icon.color = Color.white;
                break;
            case "Magic":
                icon.color = Color.blue;
                break;
            case "Rare":
                icon.color = Color.yellow;
                break;
            case "Unique":
                icon.color = Color.magenta;
                break;
            }

        icon.enabled = true;
        }
    public void ClearSlot()
        {
        item = null;
        icon.enabled = false;
        }
    public void OnPointerClick(PointerEventData eventData)
        {
        //check for item and doubleclick
        if (item == null) return;
        if (eventData.clickCount < 2) return;
        //check item was equipped before continuing
        if (!Equipment.instance.Equip(item)) return;
        Inventory.instance.remove(item);
        }
    public void OnPointerEnter(PointerEventData eventData)
        {
        if (item == null) return;
        TooltipSystem.ShowItemTooltip(item);
        }
    public void OnPointerExit(PointerEventData eventData)
        {
        TooltipSystem.HideItemTooltip();
        }
    }
