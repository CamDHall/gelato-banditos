using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilts {

    public static void ClearField(Vector3 pos, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(pos, radius, 1 << LayerMask.NameToLayer("Astro"));
        int count = hits.Length;

        for (int i = 0; i < count; i++)
        {
            if (hits[i].transform.tag == "Astro")
                MonoBehaviour.Destroy(hits[i].gameObject);
        }
    }

    public static void GetResources(GameObject obj)
    {
        int amount = Mathf.CeilToInt(obj.transform.localScale.x + obj.transform.localScale.y + obj.transform.localScale.z);
        string name = obj.name.Replace("(Clone)", "");
        if (name == "Copper") CharacterManager.Instance.copper += amount;
        else if (name == "Iron") CharacterManager.Instance.iron += amount;
    }
}
