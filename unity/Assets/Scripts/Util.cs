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
}
