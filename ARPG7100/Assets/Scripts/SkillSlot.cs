using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
    {
    [SerializeField]
    string inputAction;
    public Skill skill;

    void Update()
        {
        if(Input.GetButton(inputAction))
            {
            if (skill != null)
                skill.Fire();
            }
        }
    }
