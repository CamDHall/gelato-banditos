using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        if (name == "Copper") PlayerInventory.Instance.copper += amount;
        else if (name == "Iron") PlayerInventory.Instance.iron += amount;
    }

    public static int ChangeInStanding(Dictionary<Flavors, int> changes, Affilation group)
    {
        int val = 0;

        foreach(Flavors flavor in changes.Keys)
        {
            int count = changes[flavor];

            if(GameManager.Instance.affilation_preferences[group].Contains(flavor))
            {
                val += count;
            } else
            {
                val -= count;
            }
        }


        return val;
    }

    public static void RemoveGelato(Dictionary<Flavors, int> changes)
    {
        foreach(Flavors flavor in changes.Keys)
        {
            int difference = PlayerInventory.Instance.gelato_inventory[flavor] - changes[flavor];

            if(difference > 0)
            {
                PlayerInventory.Instance.gelato_inventory[flavor] = difference;
            } else if(difference == 0)
            {
                PlayerInventory.Instance.gelato_inventory.Remove(flavor);
            }
        }
    }

    public static int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1;
        }
        else if (dir < 0f)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public static int CanMakeRecipe(Flavor flav)
    {
        Dictionary<Ingredients, int> temp = new Dictionary<Ingredients, int>();

        foreach(Ingredients ing in PlayerInventory.Instance.ingredientsHeld.Keys)
        {
            temp.Add(ing, PlayerInventory.Instance.ingredientsHeld[ing]);
        }

        int amount = 0;
        bool t = true;
        while (t)
        {
            foreach (Ingredients ing in flav.ingredientsNeeded.Keys)
            {
                if (!temp.ContainsKey(ing))
                {
                    t = false;
                    break;
                }

                int amountRemoved = flav.ingredientsNeeded[ing];
                int amountHeld = temp[ing];

                if (amountHeld - amountRemoved < 0)
                {
                    t = false;
                }
                else if (amountHeld - amountRemoved == 0)
                {
                    temp.Remove(ing);
                }
                else
                {
                    temp[ing] -= amountRemoved;
                }
            }

            if(t) amount++;
        }

        return amount;
    }
}
