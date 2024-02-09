using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUIManager : MonoBehaviour
{
    int selectedSlot = -1;
    Skill selectedSkill;

    [SerializeField]
    GameObject skillSlotsParent;
    SkillSlot[] skillSlots;

    public GameObject clickedSlot;
    public GameObject clickedSkill;

    [SerializeField]
    GameObject[] skillsUISlots;
    [SerializeField]
    GameObject skillsUIParent;
    GameObject[] skillsUI;

    private void Awake()
        {
        skillSlots = skillSlotsParent.GetComponentsInChildren<SkillSlot>();

        //get all gameobjects for all skills in the skills UI
        int children = skillsUIParent.transform.childCount;
        skillsUI = new GameObject[children];
        for (int i = 0; i < skillsUI.Length; i++)
            {
            skillsUI[i] = skillsUIParent.transform.GetChild(i).gameObject;
            }
        }
    public void SetClickedSlot(GameObject obj)
        {
        clickedSlot = obj;
        }
    public void SetClickedSkill(GameObject obj)
        {
        clickedSkill = obj;
        }

    public void SelectSkill(Skill skill)
        {
        selectedSkill = skill;

        //hide all outlines
        foreach (GameObject obj in skillsUI)
            obj.GetComponent<Outline>().enabled = false;
        //show the outline of the clickedslot
        clickedSkill.GetComponent<Outline>().enabled = true;

        if (selectedSlot > -1)
            Equip();
        }
    public void SelectSlot(int slot)
        {
        selectedSlot = slot;

        //hide all outlines
        foreach (GameObject obj in skillsUISlots)
            obj.GetComponent<Outline>().enabled = false;
        //show the outline of the clickedslot
        clickedSlot.GetComponent<Outline>().enabled = true;

        if (selectedSkill != null)
            Equip();
        }

    void Equip()
        {
        SkillManager.instance.EquipSkill(selectedSkill, skillSlots[selectedSlot]);
        selectedSkill = null;
        selectedSlot = -1;

        //reset the outline of all the clicked slots/skills
        foreach (GameObject obj in skillsUISlots)
            obj.GetComponent<Outline>().enabled = false;

        foreach (GameObject obj in skillsUI)
            obj.GetComponent<Outline>().enabled = false;
        }
    }
