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

    public void SetupStore()
    {
        Affilation affil = FactionManager.factionAffil;

        CinematicUI.Instance.storePanel.gameObject.SetActive(true);

        /// Temp
        /// Repalce with affil specific items
        /// 

        string[] ing = System.Enum.GetNames(typeof(Ingredient));
        string[] resList = System.Enum.GetNames(typeof(ResourceType));
        int x = -2;
        int y = -2;

        foreach (string s in ing)
        {
            RectTransform temp = Instantiate(storeItem as RectTransform);
            temp.GetComponent<RectTransform>().SetParent(storePanel.transform, false);
            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * 175, y * 100) + padding;

            temp.GetComponentInChildren<Text>().text = s;
            temp.gameObject.name = s;

            int cost = Random.Range(1, 3);
            string resChoice = resList[0];

            ResourceType res = (ResourceType)System.Enum.Parse(typeof(ResourceType), resChoice);
            int amountCanAfford = 0;

            StoreItemInfo si = temp.GetComponent<StoreItemInfo>();
            si.cost = cost;
            si.resType = res;
            si.itemType = StoreItemType.Ingriedent;

            if (x < 1)
            {
                x++;
            }
            else
            {
                x = -2;
                y++;
            }

            if (CharacterManager.Instance.pData.resources.ContainsKey(res))
            {
                amountCanAfford = Mathf.FloorToInt(CharacterManager.Instance.pData.resources[res] / cost);
            }

            Dropdown dp = temp.GetComponentInChildren<Dropdown>();
            dp.onValueChanged.AddListener(delegate { UpdateOptions(dp.transform.parent.parent.gameObject, dp); });

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

        // Naming: sWeapon since right now there's only one type of weapon, must avoid local naming conflicts
        RectTransform singleWeapon = Instantiate(storeItem as RectTransform);
        singleWeapon.GetComponent<RectTransform>().SetParent(storePanel.transform, false);
        singleWeapon.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * 175, y * 100) + padding;

        string name = weaponPrefabs[0].GetComponent<StationWeapon>().name;
        singleWeapon.GetComponentInChildren<Text>().text = name;
        singleWeapon.gameObject.name = name;

        int sWeaponCost = Random.Range(5, 10);
        string sWeaponResChoice = resList[0];

        ResourceType sWeaponRes = (ResourceType)System.Enum.Parse(typeof(ResourceType), sWeaponResChoice);
        int sWeaponAmountCanAfford = 0;

        StoreItemInfo sWeaponSi = singleWeapon.GetComponent<StoreItemInfo>();
        sWeaponSi.cost = sWeaponCost;
        sWeaponSi.resType = sWeaponRes;
        sWeaponSi.itemType = StoreItemType.StationWeapon;
        sWeaponSi.obj = Instantiate(weaponPrefabs[0]);
        sWeaponSi.name = name;

        if (x < 1)
        {
            x++;
        }
        else
        {
            x = -2;
            y++;
        }

        if (CharacterManager.Instance.pData.resources.ContainsKey(sWeaponRes))
        {
            sWeaponAmountCanAfford = Mathf.FloorToInt(CharacterManager.Instance.pData.resources[sWeaponRes] / sWeaponCost);
        }

        Dropdown sWeaponDp = singleWeapon.GetComponentInChildren<Dropdown>();
        sWeaponDp.onValueChanged.AddListener(delegate { UpdateOptions(sWeaponDp.transform.parent.parent.gameObject, sWeaponDp); });

        if (sWeaponAmountCanAfford == 0)
        {
            singleWeapon.GetComponent<Image>().color = Color.red;
            sWeaponDp.enabled = false;
        }
        else
        {
            sWeaponDp.enabled = true;
            sWeaponDp.ClearOptions();

            List<string> num = new List<string>();

            for (int i = 0; i < sWeaponAmountCanAfford; i++)
            {
                num.Add((i + 1).ToString());
            }

            sWeaponDp.AddOptions(num);
        }
    }

    public void SetupStore(StationController controller)
    {
        Affilation affil = controller.station_afil;

        List<GameObject> stationWeapons = controller.stationObj.GetComponent<SpaceStation>().weapons;

        CameraManager.Instance.mainCanvas.SetActive(false);
        CameraManager.Instance.cinematicCanvas.gameObject.SetActive(true);
        CinematicUI.Instance.storePanel.gameObject.SetActive(true);

        /// Temp
        /// Repalce with affil specific items
        /// 

        string[] ing = System.Enum.GetNames(typeof(Ingredient));
        string[] resList = System.Enum.GetNames(typeof(ResourceType));
        int x = -2;
        int y = -2;

        foreach(string s in ing)
        {
            RectTransform temp = Instantiate(storeItem as RectTransform);
            temp.GetComponent<RectTransform>().SetParent(storePanel.transform, false);
            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * 175, y * 100) + padding;

            temp.GetComponentInChildren<Text>().text = s;
            temp.gameObject.name = s;

            int cost = Random.Range(1, 3);
            string resChoice = resList[0];

            ResourceType res = (ResourceType)System.Enum.Parse(typeof(ResourceType), resChoice);
            int amountCanAfford = 0;

            StoreItemInfo si = temp.GetComponent<StoreItemInfo>();
            si.cost = cost;
            si.resType = res;
            si.itemType = StoreItemType.Ingriedent;

            if (x < 1)
            {
                x++;
            }
            else
            {
                x = -2;
                y++;
            }

            if (CharacterManager.Instance.pData.resources.ContainsKey(res))
            {
                amountCanAfford = Mathf.FloorToInt(CharacterManager.Instance.pData.resources[res] / cost);
            }

            Dropdown dp = temp.GetComponentInChildren<Dropdown>();
            dp.onValueChanged.AddListener(delegate { UpdateOptions(dp.transform.parent.parent.gameObject, dp); });

            if (amountCanAfford == 0)
            {
                temp.GetComponent<Image>().color = Color.red;
                dp.enabled = false;
            } else
            {
                dp.enabled = true;
                dp.ClearOptions();

                List<string> num = new List<string>();

                for(int i = 0; i < amountCanAfford; i++)
                {
                    num.Add((i + 1).ToString());
                }

                dp.AddOptions(num);
            }
        }


        // Naming: sWeapon since right now there's only one type of weapon, must avoid local naming conflicts
        RectTransform singleWeapon = Instantiate(storeItem as RectTransform);
        singleWeapon.GetComponent<RectTransform>().SetParent(storePanel.transform, false);
        singleWeapon.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * 175, y * 100) + padding;

        string name = stationWeapons[0].GetComponent<StationWeapon>().name;
        singleWeapon.GetComponentInChildren<Text>().text = name;
        singleWeapon.gameObject.name = name;

        int sWeaponCost = Random.Range(5, 10);
        string sWeaponResChoice = resList[0];

        ResourceType sWeaponRes = (ResourceType)System.Enum.Parse(typeof(ResourceType), sWeaponResChoice);
        int sWeaponAmountCanAfford = 0;

        StoreItemInfo sWeaponSi = singleWeapon.GetComponent<StoreItemInfo>();
        sWeaponSi.cost = sWeaponCost;
        sWeaponSi.resType = sWeaponRes;
        sWeaponSi.itemType = StoreItemType.StationWeapon;
        sWeaponSi.obj = Instantiate(stationWeapons[0]);
        sWeaponSi.name = name;

        if (x < 1)
        {
            x++;
        }
        else
        {
            x = -2;
            y++;
        }

        if (CharacterManager.Instance.pData.resources.ContainsKey(sWeaponRes))
        {
            sWeaponAmountCanAfford = Mathf.FloorToInt(CharacterManager.Instance.pData.resources[sWeaponRes] / sWeaponCost);
        }

        Dropdown sWeaponDp = singleWeapon.GetComponentInChildren<Dropdown>();
        sWeaponDp.onValueChanged.AddListener(delegate { UpdateOptions(sWeaponDp.transform.parent.parent.gameObject, sWeaponDp); });

        if (sWeaponAmountCanAfford == 0)
        {
            singleWeapon.GetComponent<Image>().color = Color.red;
            sWeaponDp.enabled = false;
        }
        else
        {
            if(sWeaponAmountCanAfford > stationWeapons.Count / 2)
            {
                sWeaponAmountCanAfford = stationWeapons.Count / 2;
            }

            sWeaponDp.enabled = true;
            sWeaponDp.ClearOptions();

            List<string> num = new List<string>();

            for (int i = 0; i < sWeaponAmountCanAfford; i++)
            {
                num.Add((i + 1).ToString());
            }

            sWeaponDp.AddOptions(num);
        }
    }

    public void BuyItem()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        Dropdown[] lst = go.transform.parent.GetComponentsInChildren<Dropdown>(true);

        foreach (Dropdown dp in lst)
        {
            if (dp.enabled)
            {
                StoreItemInfo si = dp.transform.parent.GetComponent<StoreItemInfo>();

                int amount = Utilts.GetDropDownVal(dp);
                CharacterManager.Instance.pData.resources[si.resType] -= (si.cost * amount);

                if (si.itemType == StoreItemType.Ingriedent)
                {
                    Ingredient ing = (Ingredient)System.Enum.Parse(typeof(Ingredient), dp.transform.parent.name);

                    if (CharacterManager.Instance.pData.ingredientsHeld.ContainsKey(ing))
                    {
                        CharacterManager.Instance.pData.ingredientsHeld[ing] += amount;
                    }
                    else
                    {
                        CharacterManager.Instance.pData.ingredientsHeld.Add(ing, amount);
                    }
                } else if(si.itemType == StoreItemType.StationWeapon)
                {
                    if (CharacterManager.Instance.pData.weapons.ContainsKey(si.name))
                    {
                        CharacterManager.Instance.pData.weapons[si.name].Add(si.obj);
                    }
                    else
                    {
                        List<GameObject> temp = new List<GameObject>();
                        temp.Add(si.obj);
                        CharacterManager.Instance.pData.weapons.Add(si.name, temp);
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
                StoreItemInfo si = drop.transform.parent.GetComponent<StoreItemInfo>();

                tempResDict[si.resType] -= (drop.value * si.cost);

                if (tempResDict[si.resType] == 0) tempResDict.Remove(si.resType);
            }
        }

        foreach(Dropdown drop in menus)
        {
            if (dp != null && drop == dp) continue;

            StoreItemInfo si = drop.transform.parent.GetComponent<StoreItemInfo>();

            int amountCanAfford = Mathf.FloorToInt(tempResDict[si.resType] / si.cost);
            List<string> num = new List<string>();
            drop.ClearOptions();

            for (int i = 0; i < amountCanAfford; i++)
            {
                num.Add((i + 1).ToString());
            }

            drop.AddOptions(num);

            if (tempResDict[si.resType] < si.cost)
            {
                drop.transform.parent.GetComponent<Image>().color = Color.red;
                drop.enabled = false;
            }
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
                StoreItemInfo si = drop.transform.parent.GetComponent<StoreItemInfo>();

                tempResDict[si.resType] -= (drop.value * si.cost);

                if (tempResDict[si.resType] == 0) tempResDict.Remove(si.resType);
            }
        }

        foreach (Dropdown drop in menus)
        {
            StoreItemInfo si = drop.transform.parent.GetComponent<StoreItemInfo>();

            int amountCanAfford = Mathf.FloorToInt(tempResDict[si.resType] / si.cost);
            List<string> num = new List<string>();
            drop.ClearOptions();

            for (int i = 0; i < amountCanAfford; i++)
            {
                num.Add((i + 1).ToString());
            }

            drop.AddOptions(num);

            if (tempResDict[si.resType] < si.cost)
            {
                drop.transform.parent.GetComponent<Image>().color = Color.red;
                drop.enabled = false;
            }
        }
    }
}
