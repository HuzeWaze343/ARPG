using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem
    {
    public string type;
    public string subType;
    public string name;
    public int weight;
    public int val1;
    public int val2;

    public BaseItem(string baseItem)
        {
        //split input into its individual parts
        //format =
        //type subtype name weight val1 val2
        string[] itemParts = baseItem.Split('\t');
        type = itemParts[0];
        subType = itemParts[1];
        name = itemParts[2];
        weight = Convert.ToInt16(itemParts[3]);
        if (itemParts[4] == string.Empty)
            {
            val1 = 0;
            val2 = 0;
            }
        else
            {
            val1 = Convert.ToInt16(itemParts[4]);
            val2 = Convert.ToInt16(itemParts[5]);
            }
        }
    }