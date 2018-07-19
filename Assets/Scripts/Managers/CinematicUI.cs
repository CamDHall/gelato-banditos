using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CinematicUI : MonoBehaviour {

    public static CinematicUI Instance;
    public RectTransform flavor_prefab;
    public RectTransform storeItem;
    public RectTransform givePanel;
    public RectTransform storePanel;
    public RectTransform warning;

    // Randomize later
    public List<GameObject> weaponPrefabs;

    Vector2 padding = new Vector2(100, 100);

    int xCoord, yCoord;

    private void Awake()
    {
        Instance = this;
    }

    public void Leave()
    {
        CharacterManager.Instance.character.transform.position = CharacterManager.Instance.transform.position + new Vector3(0, 0, 5);
        CharacterManager.Instance.character.enabled = true;
        storePanel.gameObject.SetActive(false);
    }

    public void SetupStore(Secretariat sec)
    {
        Faction affil = sec.faction;

        CinematicUI.Instance.storePanel.gameObject.SetActive(true);

        /// Temp
        /// Repalce with affil specific items
        /// 
        xCoord = -2;
        yCoord = -2;

        foreach(Ingredient ingredient in sec.inventory.ingredients.Keys)
        {
            CreateStoreItem(sec.inventory.ingredients[ingredient], ingredient.ToString());
        }

        foreach(GameObject weapon in sec.inventory.weapons.Keys)
        {
            CreateStoreItem(sec.inventory.weapons[weapon], weapon.name);
        }
    }

    void CreateStoreItem(Dictionary<ResourceType, int> cost, string itemName)
    {
        RectTransform temp = Instantiate(storeItem as RectTransform);
        temp.GetComponent<RectTransform>().SetParent(storePanel.transform, false);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(xCoord * 175, yCoord * 100) + padding;

        temp.GetComponentInChildren<Text>().text = itemName;
        temp.gameObject.name = itemName;

        int amountCanAfford = Utilts.DictionaryMod(CharacterManager.Instance.pData.resources, cost);

        if (xCoord < 1)
        {
            xCoord++;
        }
        else
        {
            xCoord = -2;
            yCoord++;
        }

        Dropdown dp = temp.GetComponentInChildren<Dropdown>();
        dp.onValueChanged.AddListener(delegate { UpdateOptions(dp.transform.parent.parent.gameObject, dp); });

        dp.gameObject.AddComponent<ItemInfo>();
        dp.gameObject.GetComponent<ItemInfo>().itemCost = cost;

        if (amountCanAfford == 0)
        {
            temp.GetComponent<Image>().color = Color.red;
            dp.enabled = false;
        }
        else
        {
            dp.enabled = true;
            dp.ClearOptions();

            List<string> num = new List<string>();

            for (int i = 0; i < amountCanAfford; i++)
            {
                num.Add((i + 1).ToString());
            }

            dp.AddOptions(num);

        }
    }

    public void BuyItem()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        ///
        /// Find a better way to get the correct sec when there are multiple in scene
        ///

        Secretariat sec = GameObject.FindGameObjectWithTag("Secretariat").GetComponent<Secretariat>();

        Dropdown[] lst = go.transform.parent.GetComponentsInChildren<Dropdown>(true);

        foreach (Dropdown dp in lst)
        {
            if (dp.enabled)
            {
                int amount = Utilts.GetDropDownVal(dp);
                ItemInfo itemInfo = dp.gameObject.GetComponent<ItemInfo>();

                foreach(ResourceType rType in itemInfo.itemCost.Keys)
                {
                    CharacterManager.Instance.pData.resources[rType] -= itemInfo.itemCost[rType];
                } 

                if(itemInfo.ingredient != Ingredient.NULL)
                {
                    if(CharacterManager.Instance.pData.ingredientsHeld.ContainsKey(itemInfo.ingredient))
                    {
                        CharacterManager.Instance.pData.ingredientsHeld[itemInfo.ingredient] += amount;
                    } else
                    {
                        CharacterManager.Instance.pData.ingredientsHeld.Add(itemInfo.ingredient, amount);
                    }
                } else if(itemInfo.weapon != null)
                {
                    if (CharacterManager.Instance.pData.weapons.ContainsKey(itemInfo.weapon.name))
                    {
                        CharacterManager.Instance.pData.weapons[itemInfo.weapon.name].Add(itemInfo.weapon);
                    }
                    else
                    {
                        List<GameObject> temp = new List<GameObject>();
                        temp.Add(itemInfo.weapon);
                        CharacterManager.Instance.pData.weapons.Add(itemInfo.weapon.name, temp);
                    }
                }
            }
        }

        GelatoCanon.Instance.ResetCounter(true);
        UpdateOptions(go);
    }

    void UpdateOptions(GameObject parent, Dropdown dp)
    {
        Dropdown[] menus = parent.GetComponentsInChildren<Dropdown>();

        Dictionary<ResourceType, int> tempResDict = CharacterManager.Instance.pData.resources;

        foreach (Dropdown drop in menus)
        {
            if(drop.value > 0)
            {
                ItemInfo itemInfo = drop.transform.GetComponent<ItemInfo>();

                foreach(ResourceType rType in itemInfo.itemCost.Keys)
                {
                    Debug.Log(itemInfo);
                    tempResDict[rType] -= (drop.value * itemInfo.itemCost[rType]);

                    if (tempResDict[rType] == 0) tempResDict.Remove(rType);
                }
            }
        }

        foreach(Dropdown drop in menus)
        {
            if (dp != null && drop == dp) continue;

            ItemInfo itemInfo = drop.transform.parent.GetComponent<ItemInfo>();

            int amountCanAfford = Utilts.DictionaryMod(tempResDict, itemInfo.itemCost);
            List<string> num = new List<string>();
            drop.ClearOptions();

            if (amountCanAfford <= 0)
            {
                drop.transform.parent.GetComponent<Image>().color = Color.red;
                drop.enabled = false;
                continue;
            }

            for (int i = 0; i < amountCanAfford; i++)
            {
                num.Add((i + 1).ToString());
            }

            drop.AddOptions(num);
        }
    }

    void UpdateOptions(GameObject parent)
    {
        Dropdown[] menus = parent.transform.parent.GetComponentsInChildren<Dropdown>();

        Dictionary<ResourceType, int> tempResDict = CharacterManager.Instance.pData.resources;

        foreach (Dropdown drop in menus)
        {
            if (drop.value > 0)
            {
                ItemInfo itemInfo= drop.transform.parent.GetComponent<ItemInfo>();

                foreach(ResourceType rType in itemInfo.itemCost.Keys)
                {
                    tempResDict[rType] -= (drop.value * itemInfo.itemCost[rType]);

                    if (tempResDict[rType] == 0) tempResDict.Remove(rType);
                }
            }
        }

        foreach (Dropdown drop in menus)
        {
            ItemInfo itemInfo = drop.transform.GetComponent<ItemInfo>();

            int amountCanAfford = Utilts.DictionaryMod(tempResDict, itemInfo.itemCost);
            List<string> num = new List<string>();
            drop.ClearOptions();

            if (amountCanAfford <= 0)
            {
                drop.transform.parent.GetComponent<Image>().color = Color.red;
                drop.enabled = false;
                continue;
            }

            for (int i = 0; i < amountCanAfford; i++)
            {
                num.Add((i + 1).ToString());
            }

            drop.AddOptions(num);
        }
    }
}
