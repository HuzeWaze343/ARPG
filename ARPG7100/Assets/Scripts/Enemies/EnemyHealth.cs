using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private float hp;
    public float maxHp;

    public GameObject hpBar;
    Slider hpSlider;

    private void Start()
        {
        hp = maxHp;
        if (hpBar != null)
            {
            hpSlider = hpBar.GetComponent<Slider>();
            UpdateHpBar();
            }
        }
    public void damage(float dmg)
        {
        hp -= dmg;
        if (hp <= 0)
            {
            hp = 0;
            Destroy(gameObject);
            }
        if (hpBar != null) UpdateHpBar();
        }
    public void heal(float heal)
        {
        hp += heal;
        if (hp > maxHp) hp = maxHp;
        if (hpBar != null) UpdateHpBar();
        }
    public void UpdateHpBar()
        {
        hpSlider.value = hp / maxHp;
        }
    }
