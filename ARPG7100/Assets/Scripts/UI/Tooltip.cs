using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text headerField;
    public Text tagsField;
    public Text subHeaderField;
    public Text contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;

    RectTransform rectTransform;

    private void Awake()
        {
        rectTransform = GetComponent<RectTransform>();
        }
    public void SetText(string header, string tags, string subHeader, string content)
        {
        //activate/deactivate fields as needed
        if (string.IsNullOrEmpty(header))
            headerField.gameObject.SetActive(false);
        else
            headerField.gameObject.SetActive(true);

        if (string.IsNullOrEmpty(tags))
            tagsField.gameObject.SetActive(false);
        else
            tagsField.gameObject.SetActive(true);

        if (string.IsNullOrEmpty(subHeader))
            subHeaderField.gameObject.SetActive(false);
        else
            subHeaderField.gameObject.SetActive(true);

        if (string.IsNullOrEmpty(content))
            contentField.gameObject.SetActive(false);
        else
            contentField.gameObject.SetActive(true);

        headerField.text = header;
        tagsField.text = tags;
        subHeaderField.text = subHeader;
        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
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
