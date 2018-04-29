using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

        ResourceType res = (ResourceType)System.Enum.Parse(typeof(ResourceType), name);
        if (!PlayerInventory.Instance.resources.ContainsKey(res)) PlayerInventory.Instance.resources.Add(res, amount);

        PlayerInventory.Instance.resources[res] += amount;
    }

    public static int ChangeInStanding(Dictionary<Flavors, int> changes, Affilation group)
    {
        int val = 0;

        List<Flavor> flavClasses = PlayerInventory.Instance.gelatoContainer.GetComponents<Flavor>().ToList();
        Debug.Log(flavClasses.Count);
        foreach(Flavors flavor in changes.Keys)
        {
            Flavor tempFlavClass = null;

            foreach(Flavor flavClass in flavClasses)
            {
                if(flavClass.flavor == flavor)
                {
                    tempFlavClass = flavClass;
                    break;
                }
            }

            Debug.Log(tempFlavClass);

            foreach(FlavorQualities fv in tempFlavClass.flavQualities)
            {
                val += GameManager.Instance.aff_prefs[group][fv] * changes[flavor];
                Debug.Log(val);
            }

            flavClasses.Remove(tempFlavClass);
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
        Dictionary<Ingredient, int> temp = new Dictionary<Ingredient, int>();

        foreach(Ingredient ing in PlayerInventory.Instance.ingredientsHeld.Keys)
        {
            temp.Add(ing, PlayerInventory.Instance.ingredientsHeld[ing]);
        }

        int amount = 0;
        bool t = true;
        while (t)
        {
            foreach (Ingredient ing in flav.ingredientsNeeded.Keys)
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

    public static int GetDropDownVal(GameObject go)
    {
        Dropdown dp = go.GetComponentInChildren<Dropdown>();
        return int.Parse(dp.options[dp.value].text);
    }

    public static int GetDropDownVal(Dropdown dp)
    {
        return int.Parse(dp.options[dp.value].text);
    }
}
