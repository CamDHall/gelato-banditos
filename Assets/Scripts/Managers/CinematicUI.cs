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

    Vector2 padding = new Vector2(100, 100);

    private void Awake()
    {
        Instance = this;
    }

    public void Attack()
    {
        PlayerInventory.Instance.pData.standings[GameManager.Instance.nearestStation.GetComponent<SpaceStation>().spaceStation_affil] = -1;
        CameraManager.Instance.Reset();
    }

    public void Leave()
    {
        PlayerMovement.player.transform.position += (PlayerMovement.player.transform.forward * -2500);
        PlayerMovement.player.transform.LookAt(GameManager.Instance.nearestStation.gameObject.transform);
        GameManager.Instance.nearestStation.sc.aiActive = false;
        storePanel.gameObject.SetActive(false);
        givePanel.gameObject.SetActive(false);
        warning.gameObject.SetActive(true);

        CameraManager.Instance.Reset();
    }

    public void GiveAmount()
    {
        Dropdown[] dropDowns = CameraManager.Instance.cinematicCanvas.GetComponentsInChildren<Dropdown>();

        Dictionary<Flavors, int> temp = new Dictionary<Flavors, int>();

        foreach(Dropdown drop in dropDowns)
        {
            RectTransform parent = drop.GetComponentInParent<RectTransform>().parent.GetComponentInChildren<RectTransform>();
            int amount = int.Parse(drop.options[drop.value].text);
            Flavors flav = (Flavors)System.Enum.Parse(typeof(Flavors), parent.GetComponentInChildren<Text>().text);

            if (!temp.ContainsKey(flav))
            {
                temp.Add(flav, amount);
            } else
            {
                temp[flav] += amount;
            }
        }

        Affilation aff = GameManager.Instance.nearestStation.spaceStation_affil;

        PlayerInventory.Instance.pData.standings[aff] = Utilts.ChangeInStanding(temp, aff);
        Utilts.RemoveGelato(temp);

        givePanel.gameObject.SetActive(false);
    }

    public void GiveGelato()
    {
        givePanel.gameObject.SetActive(true);

        List<Flavors> flavors = new List<Flavors>(PlayerInventory.Instance.pData.gelato_inventory.Keys);

        int count = 0;

        Vector2 padding = new Vector2(100, 50);

        for(int y = -2; y < 2; y++)
        {
            for(int x = -2; x < 2; x++)
            {
                count++;
                RectTransform temp = (RectTransform)Instantiate(flavor_prefab);
                temp.transform.SetParent(givePanel.transform, false);
                temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * 150, y * 100) + padding;

                temp.GetComponentInChildren<Text>().text = flavors[count - 1].ToString();

                Dropdown dp = temp.GetComponentInChildren<Dropdown>();
                dp.ClearOptions();
                List<string> num = new List<string>();
                
                for(int i = 0; i < PlayerInventory.Instance.pData.gelato_inventory[flavors[count - 1]] + 1; i++)
                {
                    num.Add((i).ToString());
                }

                dp.AddOptions(num);

                if (count >= flavors.Count) break;
            }

            if (count >= flavors.Count) break;
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

            if (PlayerInventory.Instance.pData.resources.ContainsKey(res))
            {
                amountCanAfford = Mathf.FloorToInt(PlayerInventory.Instance.pData.resources[res] / cost);
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

        if (PlayerInventory.Instance.pData.resources.ContainsKey(sWeaponRes))
        {
            sWeaponAmountCanAfford = Mathf.FloorToInt(PlayerInventory.Instance.pData.resources[sWeaponRes] / sWeaponCost);
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
                PlayerInventory.Instance.pData.resources[si.resType] -= (si.cost * amount);

                if (si.itemType == StoreItemType.Ingriedent)
                {
                    Ingredient ing = (Ingredient)System.Enum.Parse(typeof(Ingredient), dp.transform.parent.name);

                    if (PlayerInventory.Instance.pData.ingredientsHeld.ContainsKey(ing))
                    {
                        PlayerInventory.Instance.pData.ingredientsHeld[ing] += amount;
                    }
                    else
                    {
                        PlayerInventory.Instance.pData.ingredientsHeld.Add(ing, amount);
                    }
                } else if(si.itemType == StoreItemType.StationWeapon)
                {
                    if (PlayerInventory.Instance.pData.weapons.ContainsKey(si.name))
                    {
                        PlayerInventory.Instance.pData.weapons[si.name].Add(si.obj);
                    }
                    else
                    {
                        List<GameObject> temp = new List<GameObject>();
                        temp.Add(si.obj);
                        PlayerInventory.Instance.pData.weapons.Add(si.name, temp);
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

        Dictionary<ResourceType, int> tempResDict = PlayerInventory.Instance.pData.resources;

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

        Dictionary<ResourceType, int> tempResDict = PlayerInventory.Instance.pData.resources;

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
