using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {
    // Vector

    public static Vector3 Set2D(this Vector3 target, Vector3 source)
    {
        return new Vector3(source.x, source.y, target.z);
    }

    public static Vector2 To2D(this Vector3 target)
    {
        return new Vector2(target.x, target.y);
    }

    public static Vector3 To3D(this Vector2 v, float depth = 0)
    {
        return new Vector3(v.x, v.y, depth);
    }

    // Logical

    public static int Or(int a, int b)
    {
        return a != 0 ? a : b;
    }

    // Math

    public static bool InRange(float x, float min, float max)
    {
        return x >= min && x <= max; // inclusive
    }

    // Unity Objects

    public static T Instantiate<T>(T original, GameObject parent) where T : Object
    {
        if (parent != null)
        {
            return Object.Instantiate(original, parent.transform, false);
        }
        else
        {
            return Object.Instantiate(original);
        }
    }

    // Unity delay
    public static void RunAfterSeconds(this MonoBehaviour context, float seconds, System.Action action)
    {
        context.StartCoroutine(DoAfterSecondsCoroutine(seconds, action));
    }

    private static IEnumerator DoAfterSecondsCoroutine(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    // Geometory

    public static Quaternion Get4WayRotation(float dx, float dy)
    {
        var rotation = dy > 0 ? 0 : dy < 0 ? 180 : dx > 0 ? 270 : dx < 0 ? 90 : 0;
        return Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    public const float PixelsPerUnit = 8;

    public static Texture2D CreateTexture(int width, int height)
    {
        var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        var pixels = texture.GetPixels32();
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.clear;
        texture.SetPixels32(pixels);
        return texture;
    }

    public static void DrawPixelatedCircle(int cx, int cy, int r, System.Action<int, int> setPixelFunc)
    {
        var y0 = r;
        for (var x = 0; x < r; x++)
        {
            var y1 = Mathf.FloorToInt(Mathf.Sqrt((r * r) - (x + 1) * (x + 1)));
            for (var y = y1; y < Mathf.Max(y0, y1 + 1); y++)
            {
                setPixelFunc(cx + x, cy + y);
                setPixelFunc(cx - x, cy + y);
                setPixelFunc(cx + x, cy - y);
                setPixelFunc(cx - x, cy - y);
            }
            y0 = y1;
        }
    }

    public static void DrawPixelatedLine(Vector2 p1, Vector2 p2, System.Action<int, int> setPixelFunc)
    {
        int stepCount = Mathf.FloorToInt(Mathf.Max(Mathf.Abs(p2.x - p1.x), Mathf.Abs(p2.y - p1.y)) + 1);
        var d = (p2 - p1) / stepCount;
        var p = p1;

        for (int i = 0; i < stepCount; i++)
        {
            setPixelFunc(Mathf.FloorToInt(p.x), Mathf.FloorToInt(p.y));
            p += d;
        }
    }

    public static Rect GetBoundingRect(Vector2 p1, Vector2 p2)
    {
        var x = Mathf.Min(p1.x, p2.x);
        var y = Mathf.Min(p1.y, p2.y);
        var w = Mathf.Abs(p2.x - p1.x);
        var h = Mathf.Abs(p2.y - p1.y);
        return new Rect(x, y, w, h);
    }
}
