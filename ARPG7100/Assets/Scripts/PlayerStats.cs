using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
    {
    public static bool statsLoaded = false;
    public static Dictionary<string, Stat> Stats { get; set; } = new Dictionary<string, Stat>();
    public static void LoadBaseStats()
        {
        if (statsLoaded) return;
        TextAsset ta = (TextAsset)Resources.Load("BaseStats");
        string[] baseStats = ta.text.Split('\n');
        //skip first line, it describes the rows of the document - skip last line, its a blank line added by excel for some reason /shrug
        for (int i = 1; i < baseStats.Length - 1; i++)
            {
            Stat newStat = new Stat(baseStats[i]);
            Stats.Add(newStat.Name,newStat);
            }
        statsLoaded = true;
        }
    }
public class Stat
    {
    public string Name { get; private set; }
    public int BaseValue { get; private set; }
    private List<int> Modifiers { get; set; } = new List<int>();
    public Stat(string s) //splits a string into 2 parts, statname and basevalue; for use with the BaseStats tsv file
        {
        string[] sParts = s.Split('\t');
        Name = sParts[0];
        BaseValue = Convert.ToInt32(sParts[1]);
        }
    public Stat(string name, int baseValue)
        {
        Name = name;
        BaseValue = baseValue;
        }
    public float GetValue()
        {
        float final = BaseValue;
        foreach (int mod in Modifiers) final += mod;
        return final;
        }
    public void AddModifier(int mod)
        {
        Modifiers.Add(mod);
        }
    public void RemoveModifier(int mod)
        {
        Modifiers.Remove(mod);
        }
    }
