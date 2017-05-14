using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public int ButtonID;

    private bool lastState;

    public void OnPointerDown(PointerEventData eventData)
    {
        InputUtil.SetTouchButtonDown(ButtonID, !lastState);
        InputUtil.SetTouchButton(ButtonID, true);
        lastState = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputUtil.SetTouchButtonDown(ButtonID, false);
        InputUtil.SetTouchButton(ButtonID, false);
        lastState = false;
    }

    public void LateUpdate()
    {
        InputUtil.SetTouchButtonDown(ButtonID, false); // button down should be one shot
    }
}
