using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    #region Singleton
    public static Equipment instance;
    void Awake()
        {
        if(instance == null)
            instance = this;
        }
    #endregion
    public delegate void OnEquipmentChanged();
    public OnEquipmentChanged onEquipmentChangedCallback;

    public Dictionary<string, Item> equippedItems = new Dictionary<string, Item>();
    void Start()
        {
        equippedItems.Add("helmet", null);
        equippedItems.Add("chestArmour", null);
        equippedItems.Add("boots", null);
        equippedItems.Add("gloves", null);
        equippedItems.Add("weapon", null);
        equippedItems.Add("ring", null);
        equippedItems.Add("amulet", null);
        equippedItems.Add("trinket", null);
        }

    public bool Equip(Item item)
        {
        string itemSlot = item.BaseItem.type;
        string subItemSlot = item.BaseItem.subType;
        string selItemSlot;

        //find out which item slot the item is being equipped to
        if (equippedItems.ContainsKey(subItemSlot))
            selItemSlot = subItemSlot;
        else selItemSlot = itemSlot;

        //check if there is an item already equipped in the slot you are trying to equip to, unequip it if so
        if (equippedItems[selItemSlot] != null)
            Unequip(equippedItems[selItemSlot]);

        //equip the item
        equippedItems[selItemSlot] = item;

        //add items modifiers to character
        foreach (KeyValuePair<string, int> mod in item.Modifiers)
            {
            PlayerStats.Stats[mod.Key].AddModifier(mod.Value);
            }

        //trigger callback for onEquipmentChanged
        if (onEquipmentChangedCallback != null)
            onEquipmentChangedCallback.Invoke();

        //might add conditions for equipping later? use return type to allow this functionality
        return true;
        }
    public bool Unequip(Item item)
        {
        if (item == null) return false;

        //find out which item slot the item is being unequipped from
        string itemSlot = item.BaseItem.type;
        string subItemSlot = item.BaseItem.subType;
        string selItemSlot;
        if (equippedItems.ContainsKey(subItemSlot))
            selItemSlot = subItemSlot;
        else selItemSlot = itemSlot;

        Item itemToUnequip = item;
        equippedItems[selItemSlot] = null;

        foreach (KeyValuePair<string, int> mod in item.Modifiers)
            {
            PlayerStats.Stats[mod.Key].RemoveModifier(mod.Value);
            }

        bool unequippedSuccessfully = Inventory.instance.Add(item);
        if (!unequippedSuccessfully)
            return false;

        item = null;

        if (onEquipmentChangedCallback != null)
            onEquipmentChangedCallback.Invoke();
        return true;
        }

}
