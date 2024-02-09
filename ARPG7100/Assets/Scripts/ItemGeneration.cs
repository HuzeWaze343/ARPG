using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemGeneration
    {
    static bool baseItemsLoaded = false;
    static bool affixesLoaded = false;
    static List<BaseItem> baseItems = new List<BaseItem>();
    static List<Affix> affixes = new List<Affix>();
    
    public static void LoadAffixes()
        {
        if (affixesLoaded) return;
        //import all the affixes and store them for generating on the fly
        TextAsset textAFF = (TextAsset)Resources.Load("Affixes");
        string[] affixesText = textAFF.text.Split('\n');

        //have to start at 1 because first line is a description of the rows
        //have to use length -1 because excel adds a blank line at the end
        for (int i = 1; i < affixesText.Length - 1; i++)
            {
            Affix aff = new Affix(affixesText[i]);
            affixes.Add(aff);
            Debug.Log($"!New affix imported:\n" +
                $"mod: {aff.modifier}\n" +
                $"baseType: {aff.baseType}\n" +
                $"weight: {aff.weight}, val1: {aff.val1}, val2: {aff.val2}");
            }
        affixesLoaded = true;
        }
    public static void LoadBaseItems()
        {
        if (baseItemsLoaded) return;
        //import all the baseitems and store them for generating on the fly
        TextAsset textBI = (TextAsset)Resources.Load("BaseItems");
        string[] itemsText = textBI.text.Split('\n');

        //have to start at 1 because first line is a description of the rows
        //have to use length -1 because excel adds a blank line at the end
        for (int i = 1; i < itemsText.Length - 1; i++)
            {
            BaseItem bItem = new BaseItem(itemsText[i]);
            baseItems.Add(bItem);
            Debug.Log($"!New item imported\n" +
                $"Name: {bItem.name}\n" +
                $"Type/Subtype: {bItem.type}/{bItem.subType}\n" +
                $"weight: {bItem.weight}, val1: {bItem.val1}, val2: {bItem.val2}");
            }
        baseItemsLoaded = true;
        }
    public static Item GenerateItem()
        {
        Item newItem = new Item();
        int rng;

        int minAffixes = 0; //the minimum and maximum amount of affixes that the generated item can have
        int maxAffixes = 0;

        //get the total weight of all baseitems that can drop (lower weight = rarer item)
        int totalWeight = 0;
        foreach (BaseItem b in baseItems)
            totalWeight += b.weight;

        //generate random number from 1 > totalweight, loop until we hit this number to select baseitem
        rng = Random.Range(1, totalWeight);
        foreach (BaseItem b in baseItems)
            {
            rng -= b.weight;
            if (rng <= 0)
                {
                newItem.BaseItem = b;
                break;
                }
            }

        //roll for the rarity of the item (normal 0-49, magic 50-93, rare 94-99, unique 100)
        rng = Random.Range(1, 100);
        if (rng < 50) newItem.Rarity = "Normal";
        else if (rng < 94)
            {
            newItem.Rarity = "Magic";
            minAffixes = 1;
            maxAffixes = 2;
            }
        else if (rng < 99)
            {
            newItem.Rarity = "Rare";
            minAffixes = 3;
            maxAffixes = 4;
            }
        else
            {
            newItem.Rarity = "Unique";
            minAffixes = 5;
            maxAffixes = 6;
            }

        //generate the base stats for the item (dmg for weps, hp for armor)
        if (newItem.BaseItem.type == "weapon")
            {
            newItem.AddModifier("addedMinDMG", newItem.BaseItem.val1);
            newItem.AddModifier("addedMaxDMG", newItem.BaseItem.val2);
            }
        else if (newItem.BaseItem.type == "armour")
            newItem.AddModifier("maxHP", newItem.BaseItem.val1);

        //build list of applicable modifiers
        string baseType = newItem.BaseItem.type;
        string subType = newItem.BaseItem.subType;
        List<Affix> applicableAffixes = new List<Affix>();
        foreach (Affix aff in affixes)
            if (aff.baseType == baseType | aff.baseType == subType) applicableAffixes.Add(aff);

        //add affixes to the item, removing them from the list of applicable affixes as we go
        int totalAffixes = 0;

        while (totalAffixes < maxAffixes)
            {
            if (applicableAffixes.Count < 1) break; //if theres no more mods to add, break
            if (totalAffixes >= minAffixes & Random.Range(1, 10) > 7) break; //if item already has > minaffixes, 30% chance to add another; else break out of loop
            //get the total weight of all possible modifiers
            totalWeight = 0; 
            foreach (Affix aff in applicableAffixes)
                totalWeight += aff.weight;

            rng = Random.Range(1, totalWeight);
            Affix selectedAffix = new Affix();
            foreach (Affix aff in applicableAffixes)    //loop through applicableAffixes until the selectedAffix is found
                {
                rng -= aff.weight;
                if(rng <= 0)
                    {
                    selectedAffix = aff;
                    break;
                    }
                }
            newItem.AddModifier(selectedAffix.modifier, Random.Range(selectedAffix.val1, selectedAffix.val2));  //add modifier to the item, value between val1/val2
            
            //reset the list of affixes, removing all affixes that match the affix that was selected
            List<Affix> newApplicableAffixes = new List<Affix>();
            foreach (Affix a in applicableAffixes)
                {
                if (a.modifier != selectedAffix.modifier) newApplicableAffixes.Add(a);
                }
            applicableAffixes = newApplicableAffixes;

            /*foreach (Affix a in applicableAffixes)
                if (a.modifier == selectedAffix.modifier) applicableAffixes.Remove(a);    //remove all matching modifiers from the applicable affixes, cant double them up*/
            }

        GenerateDescription(newItem);
        GenerateName(newItem);

        Debug.Log($"!NEW ITEM\n" +
            $"{newItem.ItemName}\n" +
            $"{newItem.Rarity} rarity\n" +
            $"{newItem.Description}");

        return newItem;
        }
    public static void GenerateDescription(Item item)
        {
        string description = "";

        if (item.Modifiers.ContainsKey("addedMinDMG") && item.Modifiers.ContainsKey("addedMaxDMG"))
            description += $"{item.Modifiers["addedMinDMG"]} - {item.Modifiers["addedMaxDMG"]} Damage\n\n";
        else if (item.Modifiers.ContainsKey("addedMinDMG"))
            description += $"+{item.Modifiers["addedMinDMG"]} to minimum damage\n";
        else if (item.Modifiers.ContainsKey("addedMaxDMG"))
            description += $"+{item.Modifiers["addedMaxDMG"]} to maximum damage\n";

        foreach (KeyValuePair<string, int> mod in item.Modifiers)
            {
            string s = "";
            if (mod.Key != "addedMinDMG" && mod.Key != "addedMaxDMG")
                {
                switch (mod.Key)    //add cases here for each modifier that is added
                    {
                    case "incAttackSpeed":
                        s = $"{mod.Value}% increased attack/cast speed";
                        break;
                    case "maxHP":
                        s = $"+{mod.Value} to maximum health";
                        break;
                    case "addedProjectiles":
                        s = $"SKills fire {mod.Value} additional projectiles";
                        break;
                    case "incMovementSpeed":
                        s = $"{mod.Value}% increased movement speed";
                        break;
                    default:
                        s = $"ERR: Modtype: {mod.Key}, Value: {mod.Value}";
                        break;
                    }
                s += "\n";
                description += s;
                }
            }
        item.Description = description.TrimEnd();   //trims the new line at the end
        }
    public static void GenerateName(Item item)
        {
        string prefix;
        string suffix;
        string[] armourPrefixes = { "Sturdy ", "Armored ", "Enriched ", "Enhanced ", "Ironclad ", "Plated ", "Strenghened ", "Impenetrable " };
        string[] armourSuffixes = { " of the Ox", " of Winds", " of Freedom", " of Resistance", " of the Ogre", " of Kings", " of Knighthood", " of Strength", " of the Goliath", " of Thick Skin" };
        string[] weaponPrefixes = { "Reaver's ", "Executioner's ", "Merciless ", "Dervish's ", "Cremating ", "Templar's "};
        string[] weaponSuffixes = { " of Slashing", " of Rending", " of Exsanguination", " of Skill", " of Mastery", " of Ferocity", " of Anger", " of Acrimony", " of Liquifaction", " of Virulence"};
        string[] jewelleryPrefixes = { "-blank-" };             //----------------- FILL THESE OUT
        string[] jewellerySuffixes = { "-blank-"};              //----------------- FILL THESE OUT
        string baseType = item.BaseItem.type;

        if (item.Rarity == "Normal") { item.ItemName = item.BaseItem.name; return; }            //if item is normal we dont need to generate a name

        if (baseType == "armour")
            {
            prefix = armourPrefixes[Random.Range(0, armourPrefixes.Length - 1)];
            suffix = armourSuffixes[Random.Range(0, armourSuffixes.Length - 1)];
            }
        else if (baseType == "weapon")
            {
            prefix = weaponPrefixes[Random.Range(0, weaponPrefixes.Length - 1)];
            suffix = weaponSuffixes[Random.Range(0, weaponSuffixes.Length - 1)];
            }
        else if (baseType == "jewellery")
            {
            prefix = jewelleryPrefixes[Random.Range(0, jewelleryPrefixes.Length - 1)];
            suffix = jewelleryPrefixes[Random.Range(0, jewelleryPrefixes.Length - 1)];
            }
        else
            {
            prefix = "ERR";
            suffix = "ERR";
            Debug.LogError("Item basetype was not recognised");
            }
        if (item.Rarity == "Magic") { item.ItemName = prefix + item.BaseItem.name; return; }    //if item is magic, give a prefix
        item.ItemName = prefix + item.BaseItem.name + suffix;                                   //if item is rare or unique, give a prefix and suffix
        }
    }


