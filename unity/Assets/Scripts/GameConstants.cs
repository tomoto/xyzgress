using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants {
    // Overall game rule
    public const int SecondsPerDay = 80;
    public const int DaysPerGame = 7;
    public const int ExceedingMUToWin = 100;

    // Game parameters
    public const float RechargeInterval = 1;
    public const int InitialPlayerBursters = 10;
    public const int MaxBurstersInInventory = 200;

    // Game objects
    public const string MainCamera = "Main Camera";
    public const string MenuScene = "Menu";
    public const string MainScene = "Main";

    // UI parameters
    public const float KeyInputRepeatInterval = 0.2f;
    public const float GameOverScreenWaitTime = 3;
}
