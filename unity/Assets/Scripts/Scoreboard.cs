using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

// TODO eliminate GUIText, but use UI.Text
public class Scoreboard : MonoBehaviour {
    public FlashOnChangeText[] MUTexts = new FlashOnChangeText[2];
    public FlashOnChangeText BursterText;
    public FlashOnChangeText HackIndicatorText;
    public FlashOnChangeText DaysText;
    public FlashOnChangeText LevelText;
    public FlashOnChangeText APText;

    private TimeTicker hackIndicatorTimer = new TimeTicker(0.5f);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HackIndicatorText.Text = hackIndicatorTimer.IsTimeout() ? "BURSTER" : "HACK";
    }

    public void DisplayMU(Faction faction, float value)
    {
        var i = (int)faction;
        MUTexts[i].Text = Mathf.Floor(value).ToString();
    }

    public void DisplayBursters(int value)
    {
        BursterText.Text = value.ToString();
    }

    public void DisplayHack()
    {
        hackIndicatorTimer.Start();
    }

    public void DisplayDays(int value)
    {
        DaysText.Text = value.ToString();
    }

    public void DisplayLevelAndAP(int level, int ap)
    {
        LevelText.Text = "L" + level;
        APText.Text = ap.ToString();
    }
}
