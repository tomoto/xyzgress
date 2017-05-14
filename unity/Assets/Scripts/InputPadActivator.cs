using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputPadActivator : MonoBehaviour {
    public static InputPadActivator GetInstance()
    {
        return GameObject.Find("InputPadActivator").GetComponent<InputPadActivator>();
    }

    public GameObject InputPad;

    private TimeTicker activationTimer = new TimeTicker(2);
    private bool activationTimerCounting;
    private bool IsActive { get { return InputPad.activeSelf; } }

    // Use this for initialization
    void Start()
    {
        InputPad.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsActive)
        {
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                if (!activationTimerCounting)
                {
                    activationTimer.Start();
                    activationTimerCounting = true;
                }
                else
                {
                    if (activationTimer.IsTimeout())
                    {
                        InputPad.SetActive(true);
                        activationTimerCounting = false;
                        SoundManager.GetInstance().MenuSelectSound.Play();
                    }
                }
            }
            else
            {
                activationTimerCounting = false;
            }
        }
    }
}
