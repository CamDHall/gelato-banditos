using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AsteroidUtil {

    public static Vector3 Placement(float size, float xWidth, float yWidth, float zDepth)
    {
        Vector3 Pos = new Vector3((float)Random.Range(-size / xWidth, size / xWidth), (float)Random.Range(-size / yWidth, size / yWidth),
                (float)Random.Range(-size / zDepth, size / zDepth));

        return Pos;
    }

    public static Vector3 CenterPlace(float size, float xWidth, float yWidth, float zDepth)
    {
        float x = Random.Range(-size / xWidth, size / xWidth);
        float y = Random.Range(-size / yWidth, size / yWidth);
        float z = Random.Range(-size / zDepth, size / zDepth);

        Vector3 Pos = new Vector3(x, y, z);

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

    public static void DetermineCollider(GameObject obj)
    {
        Vector3 scale = obj.transform.localScale;

        if(Mathf.Abs(scale.x - scale.y) > 0.25f || Mathf.Abs(scale.y - scale.z) > 0.25f ||
            Mathf.Abs(scale.x - scale.z) > 0.25f)
        {
            obj.AddComponent<BoxCollider>();
        } else
        {
            obj.AddComponent<SphereCollider>();
        }

    }

    public static float ClampAwayZero(float num, float limit)
    {
        float newNum = 0;
        limit = Mathf.Abs(limit);

        if(num < 0 && Mathf.Abs(num) > limit)
        {
            newNum = limit;
        } else if(num > 0 && num < limit)
        {
            newNum = limit;
        } else
        {
            newNum = num;
        }

        return newNum;
    }
}
