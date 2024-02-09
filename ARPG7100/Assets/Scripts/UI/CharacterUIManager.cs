using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIManager : MonoBehaviour
{
    [SerializeField]
    private Transform inventorySlotsParent;
    InventorySlot[] inventorySlots;

    [SerializeField]
    private Transform equipmentSlotsParent;
    EquipmentSlot[] equipmentSlots;

    [SerializeField]
    private Text statsheetTxt;

    Inventory inventory;
    Equipment equipment;
    private void Awake()
        {
        //grab references to our singletons
        inventory = Inventory.instance;
        equipment = Equipment.instance;


        //whenever itemchangedcallback is invoked (item equipped, unequipped, dropped etc), update the full UI
        inventory.onItemChangedCallback += UpdateInventoryUI;
        inventory.onItemChangedCallback += UpdateStatSheetUI;
        inventory.onItemChangedCallback += UpdateEquipmentUI;

        //set up our inventory and equipment slots
        inventorySlots = inventorySlotsParent.GetComponentsInChildren<InventorySlot>();
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
        }
    private void OnEnable()
        {
        UpdateInventoryUI();
        UpdateStatSheetUI();
        UpdateEquipmentUI();
        }
    public void UpdateInventoryUI()
        {
        //update inventory ui
        for (int i = 0; i < inventorySlots.Length; i++)
            {
            if (i < inventory.items.Count)
                {
                inventorySlots[i].AddItem(inventory.items[i]);
                }
            else
                {
                inventorySlots[i].ClearSlot();
                }
            }
        }
    public void UpdateStatSheetUI()
        {
        string s =
            $"Maximum Health:\t{PlayerStats.Stats["maxHP"].GetValue()}\n" +
            $"Base Damage:\t{PlayerStats.Stats["addedMinDMG"].GetValue()} - {PlayerStats.Stats["addedMinDMG"].GetValue() + PlayerStats.Stats["addedMaxDMG"].GetValue()}\n" +
            $"Faster Run/Walk:\t{PlayerStats.Stats["incMovementSpeed"].GetValue()}\n" +
            $"Faster Attack/Cast Speed:\t{PlayerStats.Stats["incAttackSpeed"].GetValue()}\n" +
            $"Extra Projectiles:\t{PlayerStats.Stats["addedProjectiles"].GetValue()}\n" +
            $"Increased Damage:\t{PlayerStats.Stats["incDamage"].GetValue()}";
        statsheetTxt.text = s;
        }
    public void UpdateEquipmentUI()
        {
        //update equipment ui
        foreach (EquipmentSlot slot in equipmentSlots)
            {
            if (equipment.equippedItems[slot.slotName] == null)
                slot.ClearSlot();

            else
                {
                slot.item = equipment.equippedItems[slot.slotName];
                slot.FillSlot();
                }
            }
        }
}
