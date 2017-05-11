using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {
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

    public static void DrawPixelatedLine(int x1, int y1, int x2, int y2, System.Action<int, int> setPixelFunc)
    {
        int idx = Mathf.Abs(x2 - x1);
        int idy = Mathf.Abs(y2 - y1);
        int stepCount = Mathf.Max(idx, idy);

        float dx = (x2 - x1) / (float)stepCount;
        float dy = (y2 - y1) / (float)stepCount;
        float x = x1;
        float y = y1;

        for (int i = 0; i < stepCount; i++)
        {
            setPixelFunc(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            x += dx;
            y += dy;
        }
    }
}
