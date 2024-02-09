using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
    {
    private float hp;
    private float maxHp;

    public GameObject hpBar;
    Slider hpSlider;
    Text hpText;
    GameObject deathScreen;

    private void Start()
        {
        Equipment.instance.onEquipmentChangedCallback += UpdateHpBar;
        if (hpBar != null)
            {
            hpSlider = hpBar.GetComponent<Slider>();
            hpText = hpBar.GetComponentInChildren<Text>();
            }
        maxHp = PlayerStats.Stats["maxHP"].GetValue();
        hp = maxHp;
        if (hpBar != null) UpdateHpBar();
        deathScreen = GameObject.Find("DeathScreen");
        deathScreen.SetActive(false);
        }

    public void damage(float dmg)
        {
        hp -= dmg;
        if (hp <=0)
            {
            hp = 0;
            Time.timeScale = 0;
            deathScreen.SetActive(true);
            }
        if(hpBar != null) UpdateHpBar();
        }
    public void heal(float heal)
        {
        hp += heal;
        if (hp > maxHp) hp = maxHp;
        if (hpBar != null) UpdateHpBar();
        }
    public void UpdateHpBar()
        {
        maxHp = PlayerStats.Stats["maxHP"].GetValue();
        hpSlider.value = hp / maxHp;
        hpText.text = hp + "/" + maxHp;
        }
    }