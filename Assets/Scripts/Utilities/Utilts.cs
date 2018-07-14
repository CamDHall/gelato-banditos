using System.Collections.Generic;
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

        if (CharacterManager.Instance.pData.resources == null) CharacterManager.Instance.pData.resources = new Dictionary<ResourceType, int>();

        ResourceType res = (ResourceType)System.Enum.Parse(typeof(ResourceType), name);
        if (!CharacterManager.Instance.pData.resources.ContainsKey(res)) CharacterManager.Instance.pData.resources.Add(res, amount);

        CharacterManager.Instance.pData.resources[res] += amount;
    }

    public static int ChangeInStanding(Dictionary<Flavors, int> changes, Faction group)
    {
        int val = 0;
        List<Flavor> flavClasses = CharacterManager.Instance.pData.flavors.ToList();

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

            foreach(FlavorQualities fv in tempFlavClass.flavQualities)
            {
                val += CharacterManager.Instance.pData.aff_prefs[group][fv] * changes[flavor];
            }

            flavClasses.Remove(tempFlavClass);
        }


        return val;
    }

    public static void RemoveGelato(Dictionary<Flavors, int> changes)
    {
        foreach(Flavors flavor in changes.Keys)
        {
            int difference = CharacterManager.Instance.pData.gelato_inventory[flavor] - changes[flavor];
            if(difference > 0)
            {
                CharacterManager.Instance.pData.gelato_inventory[flavor] = difference;
            } else if(difference == 0)
            {
                CharacterManager.Instance.pData.gelato_inventory.Remove(flavor);
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

        foreach(Ingredient ing in CharacterManager.Instance.pData.ingredientsHeld.Keys)
        {
            temp.Add(ing, CharacterManager.Instance.pData.ingredientsHeld[ing]);
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

    public static GameObject FindTarget(Transform obj)
    {
        Collider[] colls = Physics.OverlapSphere(obj.position, 1000);
        List<GameObject> banditos = new List<GameObject>();

        foreach (Collider col in colls)
        {
            if (col.gameObject.tag == "Bandito" && col.gameObject != obj.gameObject)
            {
                banditos.Add(col.gameObject);
            }
        }

        float closestDist = 50000;
        GameObject closest = null;

        foreach (GameObject bandit in banditos)
        {
            float dist = Vector3.Distance(obj.position, bandit.transform.position);
            if (dist < closestDist)
            {
                closest = bandit;
                closestDist = dist;
            }
        }

        return closest;
    }

    public static void FireLaser(Transform obj, LineRenderer prefab_laser)
    {
        Ray ray = new Ray(obj.transform.position, obj.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        LineRenderer laser = UnityEngine.MonoBehaviour.Instantiate(prefab_laser);
        laser.transform.SetParent(obj);

        bool foundTarget = false;

        foreach (RaycastHit hit in hits) {
            if (hit.transform.gameObject == obj.gameObject) continue;

            Transform parent = hit.transform.parent;
            if (parent != null && obj == parent) continue;

            laser.enabled = true;
            foundTarget = true;

            string hTag = hit.transform.tag;
            laser.SetPosition(0, obj.position);
            laser.SetPosition(1, hit.point);
            if (hTag == "Player")
            {
                PlayerMovement.player.TakeDamge(1);
                foundTarget = true;
                break;
            } else
            {
                if (hTag == "Astro" || hTag == "StationWeapons")
                {
                    AudioManager.Instance.AstroCrack();
                    break;
                }

                IDamageable _IDamage = hit.transform.GetComponent<IDamageable>();
                if(_IDamage == null)
                {
                    Debug.Log(hit.transform);
                    _IDamage = hit.transform.parent.GetComponent<IDamageable>();
                }

                if (_IDamage != null) _IDamage.TakeDamage(1);

                if (hTag == "Astro")
                {
                    UnityEngine.MonoBehaviour.Destroy(hit.transform.gameObject, Time.deltaTime * 5);
                    Utilts.GetResources(hit.transform.gameObject);
                    break;
                }
            }
        }

        if(!foundTarget)
        {
            laser.SetPosition(0, obj.position);
            laser.SetPosition(1, ray.GetPoint(10000));
        }
    }
}

public class ChatMessage
{
    public Color color;
    public string colorStr;
    public string messageTxt;

    public string finalMessage;

    public ChatMessage(Color incolor, string inmessage)
    {
        color = incolor;
        messageTxt = inmessage;

        GenerateFinalMessage(true);
    }

    public ChatMessage(string finalColor, string inmessage)
    {
        colorStr = finalColor;
        messageTxt = inmessage;

        GenerateFinalMessage(false);
    }

    public void GenerateFinalMessage(bool colorNeedsCalculated)
    {
        if(colorNeedsCalculated)
            colorStr = "#" + ColorUtility.ToHtmlStringRGBA(color);

        finalMessage = string.Format("<color={0}>{1}</color>", colorStr, messageTxt);
    }
}