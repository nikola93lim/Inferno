using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityClass
{
    public static float DistanceSquared(Vector3 a, Vector3 b)
    {
        float num = a.x - b.x;
        float num2 = a.y - b.y;
        float num3 = a.z - b.z;
        float distance = num * num + num2 * num2 + num3 * num3;
        return distance;
    }
}
