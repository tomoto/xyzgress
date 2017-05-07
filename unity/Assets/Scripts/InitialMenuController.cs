using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitialMenuController : MonoBehaviour {
    // Output for the main scene
    public static int Faction { get; private set; }
    public static int Level { get; private set; }
    public static bool IsHaijin { get { return Level == 2; } }

    // External Properties
    public GameObject[] FactionArrows = new GameObject[2];
    public GameObject[] LevelArrows = new GameObject[3];
    public GameObject[] LevelTexts = new GameObject[3];
    public GameObject[] Steps = new GameObject[2];

    private int currentStep;
    private TimeTicker keyInputRepeatTimer = new TimeTicker(GameConstants.KeyInputRepeatInterval);

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentStep)
        {
            case 0:
                Faction = Mathf.Clamp(Faction - GetInput(), 0, 1);
                break;
            case 1:
                Level = Mathf.Clamp(Level - GetInput(), 0, 2);
                break;
            case 2:
                StartGame();
                break;
        }

        UpdateArrow();

        if (Input.GetButtonDown(InputUtil.Button1))
        {
            currentStep++;
        }

        if (Input.GetButtonDown(InputUtil.Button2) && currentStep > 0)
        {
            currentStep--;
        }
	}

    private void UpdateArrow()
    {
        for (int i = 0; i <= 1; i++)
        {
            FactionArrows[i].SetActive(Faction == i);
        }

        var currentMaterial = FactionArrows[Faction].GetComponent<Renderer>().material;

        Steps[1].SetActive(currentStep == 1);

        for (int i = 0; i <= 2; i++)
        {
            LevelArrows[i].SetActive(Level == i);
            LevelArrows[i].GetComponent<Renderer>().material = currentMaterial;
            LevelTexts[i].GetComponent<Text>().color = Level == i ? currentMaterial.color : Color.white;
        }
    }

    private int GetInput()
    {
        var input = InputUtil.GetY();
        if (input != 0)
        {
            if (keyInputRepeatTimer.IsTimeout())
            {
                keyInputRepeatTimer.Start();
                return input;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            keyInputRepeatTimer.Reset();
            return input;
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene(GameConstants.MainScene);
    }
}
