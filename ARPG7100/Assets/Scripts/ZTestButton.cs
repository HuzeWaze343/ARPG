using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZTestButton : MonoBehaviour
    {
    [SerializeField]
    SkillSlot slot;
    [SerializeField]
    Skill skill;
    public void testFunction()
        {
        Inventory.instance.Add(ItemGeneration.GenerateItem());
        }
    public void testFunction2()
        {
        SkillManager.instance.EquipSkill(skill, slot);
        }
    }
