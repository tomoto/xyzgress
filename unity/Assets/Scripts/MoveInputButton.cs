using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveInputButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public int dx;
    public int dy;

    public void OnPointerEnter(PointerEventData eventData)
    {
        InputUtil.SetTouchAxes(dx, dy);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputUtil.SetTouchAxes(0, 0);
    }
}
