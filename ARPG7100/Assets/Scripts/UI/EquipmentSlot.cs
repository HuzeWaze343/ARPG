using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string slotName;
    public Image icon;
    public Item item;
    Sprite iconSprite;

    Inventory inventory;
    Equipment equipment;

    void Start()
        {
        //grab singleton references
        inventory = Inventory.instance;
        equipment = Equipment.instance;
        }

    public void EquipItem(Item itemToEquip)
        {
        //check that the item youre equipping is placed in the correct slot
        if (itemToEquip.BaseItem.type != slotName | itemToEquip.BaseItem.subType != slotName) return;
        Equipment.instance.Equip(itemToEquip);

        //return currently equipped item to inventory
        if (item != null) UnequipItem(item);

        //equip the new item and add its modifiers to your playerstats
        item = itemToEquip;
        foreach (KeyValuePair<string, int> mod in item.Modifiers)
            {
            PlayerStats.Stats[mod.Key].AddModifier(mod.Value);
            }

        //enable icon visuals to show that item is equipped
        icon.enabled = true;

        //remove the item from your inventory as its now in an equipment slot
        Inventory.instance.remove(itemToEquip);
        }
    public void UnequipItem(Item itemToUnequip)
        {
        //remove all modifiers that are on the unequipped item
        foreach (KeyValuePair<string, int> mod in item.Modifiers)
            {
            PlayerStats.Stats[mod.Key].RemoveModifier(mod.Value);
            }

        //return the item to your inventory
        Inventory.instance.Add(itemToUnequip);
        ClearSlot();
        }
    public void FillSlot()
        {
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

        //dont unequip if inventory is full
        //if (Inventory.instance.items.Count > -Inventory.instance.inventorySpace) return;

        equipment.Unequip(item);
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
