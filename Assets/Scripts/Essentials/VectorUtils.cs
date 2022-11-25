using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
    public static Vector2 ToVector2(this Vector3 vector3)
    {
        return (Vector2)vector3;
    }
}
