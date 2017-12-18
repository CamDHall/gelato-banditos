using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AsteroidUtil {

    public static Vector3 Placement(int size, float xWidth, float yWidth, float zDepth)
    {
        Vector3 Pos = new Vector3((float)Random.Range(-size / xWidth, size / xWidth), (float)Random.Range(-size / yWidth, size / yWidth),
                (float)Random.Range(-size / zDepth, size / zDepth));

        return Pos;
    }

    public static Quaternion Rotation()
    {
        Quaternion rot = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));

        return rot;
    }

    public static Vector3 Scale()
    {
        Vector3 scale = new Vector3(Random.Range(0.5f, 1.25f), Random.Range(0.5f, 1.25f), Random.Range(0.5f, 1.25f));

        return scale;
    }
}
