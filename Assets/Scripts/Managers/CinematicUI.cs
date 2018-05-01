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

    Vector2 padding = new Vector2(100, 100);

    private void Awake()
    {
        Instance = this;
    }

    public void Attack()
    {
        PlayerInventory.Instance.standings[GameManager.Instance.nearestStation.GetComponent<SpaceStation>().spaceStation_affil] = -1;
        CameraManager.Instance.Reset();
    }

    public void Leave()
    {
        float col_size = GameManager.Instance.nearestStation.GetComponent<SphereCollider>().radius;
        Collider[] colls = Physics.OverlapSphere(PlayerMovement.player.transform.position, col_size);
        PlayerMovement.player.transform.position += (PlayerMovement.player.transform.forward * -(col_size + 1500));

        foreach(Collider col in colls)
        {
            if(col.gameObject.name.Contains("SpaceStation"))
            {
                PlayerMovement.player.transform.LookAt(col.gameObject.transform);
                break;
            }
        }

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

            temp.Add(flav, amount);
        }

        Affilation aff = GameManager.Instance.nearestStation.spaceStation_affil;

        PlayerInventory.Instance.standings[aff] = Utilts.ChangeInStanding(temp, aff);
        Utilts.RemoveGelato(temp);

        givePanel.gameObject.SetActive(false);
    }

    public void GiveGelato()
    {
        givePanel.gameObject.SetActive(true);

        List<Flavors> flavors = new List<Flavors>(PlayerInventory.Instance.gelato_inventory.Keys);

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
                
                for(int i = 0; i < PlayerInventory.Instance.gelato_inventory[flavors[count - 1]] + 1; i++)
                {
                    num.Add((i).ToString());
                }

                dp.AddOptions(num);

                if (count >= flavors.Count) break;
            }

            if (count >= flavors.Count) break;
        }
    }

    public void SetupStore(Affilation affil)
    {
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

            if (x < 1)
            {
                x++;
            }
            else
            {
                x = -2;
                y++;
            }

            if (PlayerInventory.Instance.resources.ContainsKey(res))
            {
                amountCanAfford = Mathf.FloorToInt(PlayerInventory.Instance.resources[res] / cost);
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
    }

    public void BuyItem()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        Dropdown[] lst = go.transform.parent.GetComponentsInChildren<Dropdown>(true);

        foreach (Dropdown dp in lst)
        {
            if (dp.enabled)
            {
                Ingredient ing = (Ingredient)System.Enum.Parse(typeof(Ingredient), dp.transform.parent.name);
                int amount = Utilts.GetDropDownVal(dp);
                StoreItemInfo si = dp.transform.parent.GetComponent<StoreItemInfo>();
                PlayerInventory.Instance.resources[si.resType] -= (si.cost * amount);

                if(PlayerInventory.Instance.ingredientsHeld.ContainsKey(ing))
                {
                    PlayerInventory.Instance.ingredientsHeld[ing] += amount;
                } else
                {
                    PlayerInventory.Instance.ingredientsHeld.Add(ing, amount);
                }
            }
        }

        UpdateOptions(go);
    }

    void UpdateOptions(GameObject parent, Dropdown dp)
    {
        Dropdown[] menus = parent.GetComponentsInChildren<Dropdown>();

        Dictionary<ResourceType, int> tempResDict = PlayerInventory.Instance.resources;

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

        Dictionary<ResourceType, int> tempResDict = PlayerInventory.Instance.resources;

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
