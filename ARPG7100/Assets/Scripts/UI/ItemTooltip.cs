using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
    {
    public Text headerField;
    public Text mainTextField;
    public Text subTextField;
    public RectTransform rectTransform;

    private void Awake()
        {
        rectTransform = GetComponent<RectTransform>();
        }

    public void UpdateTooltip(Item item)
        {
        headerField.text = item.ItemName;
        mainTextField.text = item.Rarity;
        subTextField.text = item.Description;

        switch (item.Rarity)
            {
            case "Normal":
                mainTextField.color = Color.white;
                break;
            case "Magic":
                mainTextField.color = Color.blue;
                break;
            case "Rare":
                mainTextField.color = Color.yellow;
                break;
            case "Unique":
                mainTextField.color = Color.magenta;
                break;
            }
        }
    private void Update()
        {
        Vector2 pos = Input.mousePosition;

        float pivotX = pos.x / Screen.width;
        float pivotY = pos.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = pos;
        }
    }
