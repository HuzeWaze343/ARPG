using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
    {
    public static SkillManager instance;
    private void Awake()
        {
        if (instance == null)
            instance = this;
        }
    public void EquipSkill(Skill skillToEquip, SkillSlot skillSlot)
        {
        //remove the old skill from the slot, if there is one already equipped
        UnequipSkill(skillSlot);

        //adds new skill
        skillSlot.gameObject.AddComponent(skillToEquip.GetType());
        //the skill we unequipped is only destroyed at the end of the update
        //so we have to grab ALL skill components on this slot, and then grab the most recently created one (the one we just equipped)
        Skill[] skills = skillSlot.gameObject.GetComponents<Skill>();
        skillSlot.skill = skills[skills.Length - 1];
        Image icon = skillSlot.gameObject.transform.Find("Icon").gameObject.GetComponent<Image>();
        icon.sprite = Resources.Load<Sprite>("SkillIcons/" + skillToEquip.name + "Icon");
        }
    public void UnequipSkill(SkillSlot skillSlot)
        {
        Skill skillToUnequip = skillSlot.gameObject.GetComponent<Skill>();
        if (skillToUnequip != null)
            Destroy(skillSlot.gameObject.GetComponent<Skill>());

        skillSlot.skill = null;
        }
    }
