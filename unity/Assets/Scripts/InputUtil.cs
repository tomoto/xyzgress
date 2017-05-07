using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputUtil {
    public const string X = "Horizontal";
    public const string Y = "Vertical";
    public const string Button1 = "Fire1";
    public const string Button2 = "Fire2";

    public static int GetX()
    {
        return GetAxis(X);
    }

    public static int GetY()
    {
        return GetAxis(Y);
    }

    private static int GetAxis(string name)
    {
        return Mathf.FloorToInt(Input.GetAxisRaw(name));
    }
}
