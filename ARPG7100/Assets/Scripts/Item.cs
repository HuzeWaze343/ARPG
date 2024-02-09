using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
    {
    private BaseItem baseItem;
    private string itemName;
    private string description;
    private string rarity;
    private Dictionary<string, int> modifiers;
    public BaseItem BaseItem { get; set; }
    public string ItemName { get; set; }
    public string Description { get; set; }
    public string Rarity { get; set; }
    public Dictionary<string, int> Modifiers { get; set; } = new Dictionary<string, int>();
    public void AddModifier(string modName, int value)
        {
        if (Modifiers.ContainsKey(modName)) Modifiers[modName] += value;
        else Modifiers.Add(modName, value);
        }
    
    }