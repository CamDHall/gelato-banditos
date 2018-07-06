using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using UnityEngine;

public class GelatoCanon : SerializedMonoBehaviour
{

    Flavors currentFlav;
    public float scaleValue = 0.3f;
    public Dictionary<Flavors, GameObject> flavorObjects;
    List<List<GameObject>> inventory;
    Dictionary<Flavors, int> inHand;
    List<GameObject> toGive;

    int i = 0, len = 0;

    public static GelatoCanon Instance;

    public bool holding = false;
    public Material mat;
    GameObject currentItem;

    int cannonIndex = 0;

    private void Awake()
    {
        Instance = this;
        inventory = new List<List<GameObject>>();
        inHand = new Dictionary<Flavors, int>();
        toGive = new List<GameObject>();
    }

    void Update () {
        if(!CharacterManager.Instance.inStation && !holding && Input.GetAxis("LT") > 0 && inventory.Count > 0)
        {
            LaunchItem();
        } else if(CharacterManager.Instance.inStation && Input.GetAxis("LT") > 0 && inHand.Count > 0)
        {
            GiveGelato();
        }

        if(Input.GetAxis("LT") == 0 && holding && !CharacterManager.Instance.inStation)
        {
            holding = false;

            if(currentItem.GetComponent<Gelato>() != null)
            {
                FireGelato();
            } else
            {
                PlaceItem();
            }
        }

        if(CharacterManager.Instance.inStation && Input.GetButtonDown("Y"))
        {
            AddToHand();
        }
	}

    void GiveGelato()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position + (transform.forward * 2), 5);

        if(colls.Length > 0)
        {
            foreach(Collider col in colls)
            {
                if(col.tag == "Basket")
                {
                    CharacterManager.Instance.pData.standings[FactionManager.factionAffil] += Utilts.ChangeInStanding(inHand, FactionManager.factionAffil);

                    int len = toGive.Count;

                    for(int i = 0; i < len; i++)
                    {
                        Destroy(toGive[i]);
                    }

                    toGive.Clear();
                }
            }
        }
    }

    void AddToHand()
    {
        currentItem = inventory[cannonIndex][0];
        inventory[cannonIndex].Remove(currentItem);

        if (inventory[cannonIndex].Count == 0)
        {
            inventory.Remove(inventory[cannonIndex]);
            UpdateCounter(0, false);
        }
        if (currentItem.GetComponent<Gelato>() != null)
        {
            currentItem.transform.SetParent(transform.parent);
            currentItem.transform.localPosition = new Vector3(0, 0, 0.5f);

            Flavors flav = currentItem.GetComponent<Gelato>().flavor;

            if(!inHand.ContainsKey(flav))
            {
                inHand.Add(flav, 1);
            } else
            {
                inHand[flav]++;
            }

            toGive.Add(currentItem);

            CharacterManager.Instance.pData.gelato_inventory[currentFlav]--;
        }

        holding = true;
    }

    void PlaceItem()
    {
        currentItem.GetComponent<StationWeapon>().Enable();
        currentItem.transform.localScale = new Vector3(1, 1, 1);
    }

    void LaunchItem()
    {
        currentItem = inventory[cannonIndex][0];
        inventory[cannonIndex].Remove(currentItem);

        if (inventory[cannonIndex].Count == 0)
        {
            inventory.Remove(inventory[cannonIndex]);
            UpdateCounter(0, false);
        }
        if (currentItem.GetComponent<Gelato>() != null)
        {
            currentItem.transform.SetParent(transform.parent.parent);
            currentItem.transform.localPosition = new Vector3(0, 0.25f, 0);
            CharacterManager.Instance.pData.gelato_inventory[currentFlav]--;
        } else
        {
            currentItem.GetComponent<StationWeapon>().friendly = true;
            currentItem.transform.position = PlayerMovement.player.transform.position + (PlayerMovement.player.transform.forward * 750);
            currentItem.transform.SetParent(null);
            PlaceItem();
        }
        currentItem.transform.localRotation = Quaternion.identity;

        holding = true;
    }

    public void UpdateCounter(int direction, bool newItem)
    {
        if(direction < -1 || direction > 1)
        {
            Debug.Log("WRONG DIRECTION VALUE");
            return;
        }

        // Only instiantiate items when new items are added to inventory
        if(newItem)
        {
            ClearHand();

            if (!CharacterManager.Instance.inStation && CharacterManager.Instance.pData.weapons != null && CharacterManager.Instance.pData.weapons.Count > 0)
            {
                foreach (string weaponName in CharacterManager.Instance.pData.weapons.Keys)
                {
                    len = CharacterManager.Instance.pData.weapons[weaponName].Count;
                    List<GameObject> newItems = new List<GameObject>();

                    for (i = 0; i < len; i++)
                    {
                        string _tempName = Regex.Replace(weaponName, "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))", "");

                        GameObject weapon = Instantiate(Resources.Load(_tempName) as GameObject, transform);
                        weapon.GetComponent<StationWeapon>().friendly = true;
                        weapon.GetComponent<StationWeapon>().Disable();
                        weapon.transform.rotation = Quaternion.Euler(0, 90, 0);
                        weapon.transform.localScale = new Vector3(0.006f, 0.006f, 0.006f);
                        weapon.SetActive(true);

                        newItems.Add(weapon);
                    }

                    inventory.Add(newItems);
                }
            }

            if(CharacterManager.Instance.pData.gelato_inventory != null && CharacterManager.Instance.pData.gelato_inventory.Count > 0)
            {
                int num = CharacterManager.Instance.pData.gelato_inventory.Count;

                foreach(Flavors flav in CharacterManager.Instance.pData.gelato_inventory.Keys)
                {
                    len = CharacterManager.Instance.pData.gelato_inventory[flav];
                    List<GameObject> newItems = new List<GameObject>();

                    for (i = 0; i < CharacterManager.Instance.pData.gelato_inventory[flav]; i++)
                    {
                        GameObject temp = Instantiate(flavorObjects[flav], transform);
                        temp.transform.localPosition = Vector3.zero;
                        temp.transform.rotation = Quaternion.Euler(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
                        temp.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
                        temp.GetComponent<Gelato>().flavor = flav;
                        newItems.Add(temp);
                    }

                    inventory.Add(newItems);
                }
            }
        }

        int newIndex = direction + cannonIndex;

        if (newIndex >= inventory.Count) newIndex = 0;
        else if (newIndex < 0) newIndex = inventory.Count - 1;
        
        for(int i = 0; i < inventory.Count; i++)
        {
            if(i != newIndex)
            {
                foreach(GameObject obj in inventory[i]) obj.SetActive(false);
            } else
            {
                foreach(GameObject obj in inventory[i]) obj.SetActive(true);
            }
        }

        cannonIndex = newIndex;
    }

    public void ResetCounter(bool newItem)
    {
        cannonIndex = 0;
        UpdateCounter(0, newItem);
    }

    void ClearHand()
    {
        if(inventory.Count > 0)
        {
            foreach(List<GameObject> objs in inventory)
            {
                foreach (GameObject obj in objs) Destroy(obj);
            }

            inventory.Clear();
        }
    }

    void FireGelato()
    {
        Gelato g = currentItem.GetComponent<Gelato>();
        RaycastHit hit;

        if (Physics.Raycast(PlayerMovement.player.transform.position, Camera.main.transform.forward, out hit) && hit.transform.tag == "Bandito")
        {
            AudioManager.Instance.Whip();
            g.target = hit.transform;
        }
        else
        {
            g.dir = transform.forward;
        }

        g.bc.enabled = true;
        g.launched = true;
    }
}
