using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputUtil {
    public const string X = "Horizontal";
    public const string Y = "Vertical";
    private static string[] ButtonNames = new string[] { "Fire1", "Fire2" };

    // TODO hmm, I don't like using static too much
    private static int TouchAxisX;
    private static int TouchAxisY;
    private static bool[] TouchButtonValues;
    private static bool[] TouchButtonDownValues;

    public static void Clear()
    {
        TouchAxisX = 0;
        TouchAxisY = 0;
        TouchButtonValues = new bool[ButtonNames.Length];
        TouchButtonDownValues = new bool[ButtonNames.Length];
    }

    public static int GetX()
    {
        return Util.Or(GetAxis(X), TouchAxisX);
    }

    public static int GetY()
    {
        return Util.Or(GetAxis(Y), TouchAxisY);
    }

    private static int GetAxis(string name)
    {
        return Mathf.FloorToInt(Input.GetAxisRaw(name));
    }

    public static void SetTouchAxes(int dx, int dy)
    {
        TouchAxisX = dx;
        TouchAxisY = dy;
    }

    public static bool GetButton(int id)
    {
        var result = Input.GetButton(ButtonNames[id]);
        return result || TouchButtonValues[id];
    }

    public static bool GetButtonDown(int id)
    {
        var result = Input.GetButtonDown(ButtonNames[id]);
        return result || TouchButtonDownValues[id];
    }

    public static void SetTouchButton(int id, bool value)
    {
        TouchButtonValues[id] = value;
    }

    public static void SetTouchButtonDown(int id, bool value)
    {
        TouchButtonDownValues[id] = value;
    }
}
