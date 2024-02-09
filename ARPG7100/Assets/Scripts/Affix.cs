using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affix
    {
    public string baseType;
    public string modifier;
    public int weight;
    public int val1;
    public int val2;
    public Affix() { }
    public Affix(string affix)
        {
        //split input into its individual parts
        //format =
        //basetype modifier weight val1 val2
        string[] affixParts = affix.Split('\t');
        baseType = affixParts[0];
        modifier = affixParts[1];
        weight = Convert.ToInt16(affixParts[2]);
        val1 = Convert.ToInt16(affixParts[3]);
        val2 = Convert.ToInt16(affixParts[4]);
        }
    }