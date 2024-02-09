using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour
    {
    private GameObject window;
    private Canvas canvas;
    private RectTransform canvasRT;
    private RectTransform rt;
    public void Awake()
        {
        window = gameObject;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasRT = canvas.GetComponent<RectTransform>();
        rt = window.GetComponent<RectTransform>();
        }
    public void DragHandler(BaseEventData data)//Code for Draggable UI
        {
        //Convert BaseEventData to PointerEventData so I can access methods
        PointerEventData pointerData = (PointerEventData)data;

        //blank vector2 to store position to move UI frame to
        Vector2 position;

        //transform pointer position on screen to position on the canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);

        //offset by half the window size so that you move window from the top bar
        //and not from the center of the window
        position.y = position.y - (rt.rect.height / 2) + 20;

        //adjust the transform to new position
        transform.position = canvas.transform.TransformPoint(position);

        position = ClampToCanvas(position);
        transform.position = canvas.transform.TransformPoint(position);
        }
    public Vector2 ClampToCanvas(Vector2 v)
        {
        Vector3[] corners = new Vector3[4];
        Vector3[] canvasCorners = new Vector3[4];
        rt.GetWorldCorners(corners); //returns array of the RectTransform's corners, order: bottom left, top left, top right, bottom right
        rt.GetLocalCorners(canvasCorners);

        if (corners[0].y <= 0) v.y = canvasCorners[0].y - ((canvasRT.rect.height/2) - rt.rect.height);
        if (corners[0].x <= 0) v.x = canvasCorners[0].x - ((canvasRT.rect.width/2) - rt.rect.width);
        if (corners[2].y >= canvasRT.rect.height) v.y = canvasCorners[2].y + ((canvasRT.rect.height/2) - rt.rect.height);
        if (corners[2].x >= canvasRT.rect.width) v.x = canvasCorners[2].x + ((canvasRT.rect.width / 2) - rt.rect.width);
        return v;
        }
    }
