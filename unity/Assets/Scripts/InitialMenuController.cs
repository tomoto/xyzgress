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
    private TimeTicker gameStartWaitTimer;
    private GameObject Mask;

	// Use this for initialization
	void Start () {
        Mask = transform.Find("Mask").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        // Transitive state
        switch (currentStep)
        {
            case 2:
                gameStartWaitTimer = new TimeTicker(2.0f).Start();
                Mask.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                StartCoroutine(PlaySoundAfterSeconds(SoundManager.GetInstance().WelcomeBackVoice, 0.7f, 1.5f));
                DontDestroyOnLoad(SoundManager.GetInstance());
                currentStep++;
                return; // do not proceed
            case 3:
                Mask.transform.localScale = Mask.transform.localScale * (1 + Time.deltaTime * 5);
                if (gameStartWaitTimer.IsTimeout()) StartGame();
                return; // do not proceed
        }

        // Interactive state
        var input = GetInput();
        bool valueChanged = false;

        switch (currentStep)
        {
            case 0:
                int newFaction = Mathf.Clamp(Faction - input, 0, 1);
                valueChanged = Faction != newFaction;
                Faction = newFaction;
                break;
            case 1:
                int newLevel = Mathf.Clamp(Level - input, 0, 2);
                valueChanged = Level != newLevel;
                Level = newLevel;
                break;
        }

        if (valueChanged)
        {
            SoundManager.GetInstance().MenuMoveSound.Play();
        }

        UpdateArrow();

        if (Input.GetButtonDown(InputUtil.Button1))
        {
            SoundManager.GetInstance().MenuSelectSound.Play();
            currentStep++;
        }

        if (Input.GetButtonDown(InputUtil.Button2) && currentStep > 0)
        {
            currentStep--;
        }
	}

    private IEnumerator PlaySoundAfterSeconds(AudioClip sound, float seconds, float volume)
    {
        yield return new WaitForSeconds(seconds);
        sound.Play(volume);
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
