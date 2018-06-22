using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using UnityEngine;

public class GelatoCanon : SerializedMonoBehaviour
{

    Flavors currentFlav;
    public Dictionary<Flavors, GameObject> flavorObjects;
    List<List<GameObject>> inHand; // sligtly faster than Queue, dictionary option
    int i = 0, len = 0;

    public static GelatoCanon Instance;

    public bool holding = false;
    public Material mat;
    GameObject currentItem;

    int cannonIndex = 0;

    private void Awake()
    {
        Instance = this;
        inHand = new List<List<GameObject>>();
    }

    void Update () {

        if(!holding && Input.GetAxis("LT") > 0 && inHand.Count > 0)
        {
            LaunchItem();
        }

        if(Input.GetAxis("LT") == 0 && holding)
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
	}

    void PlaceItem()
    {
        currentItem.GetComponent<StationWeapon>().Enable();
        currentItem.transform.localScale = new Vector3(1, 1, 1);
    }

    void LaunchItem()
    {
        currentItem = inHand[cannonIndex][0];
        inHand[cannonIndex].Remove(currentItem);

        if (inHand[cannonIndex].Count == 0)
        {
            inHand.Remove(inHand[cannonIndex]);
            UpdateCounter(0, false);
        }
        if (currentItem.GetComponent<Gelato>() != null)
        {
            currentItem.transform.SetParent(transform.parent.parent);
            currentItem.transform.localPosition = new Vector3(0, 0.25f, 0);
            PlayerInventory.Instance.gelato_inventory[currentFlav]--;
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
            if (PlayerInventory.Instance.weapons != null && PlayerInventory.Instance.weapons.Count > 0)
            {
                foreach (string weaponName in PlayerInventory.Instance.weapons.Keys)
                {
                    len = PlayerInventory.Instance.weapons[weaponName].Count;
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

                    inHand.Add(newItems);
                }
            }

            if(PlayerInventory.Instance.gelato_inventory != null && PlayerInventory.Instance.gelato_inventory.Count > 0)
            {
                int num = PlayerInventory.Instance.gelato_inventory.Count;

                foreach(Flavors flav in PlayerInventory.Instance.gelato_inventory.Keys)
                {
                    len = PlayerInventory.Instance.gelato_inventory[flav];
                    List<GameObject> newItems = new List<GameObject>();

                    for (i = 0; i < PlayerInventory.Instance.gelato_inventory[flav]; i++)
                    {
                        GameObject temp = Instantiate(flavorObjects[flav], transform);
                        temp.transform.localPosition = Vector3.zero;
                        temp.transform.rotation = Quaternion.Euler(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
                        temp.GetComponent<Gelato>().flavor = flav;
                        newItems.Add(temp);
                    }

                    inHand.Add(newItems);
                }
            }
        }

        int newIndex = direction + cannonIndex;

        if (newIndex >= inHand.Count) newIndex = 0;
        else if (newIndex < 0) newIndex = inHand.Count - 1;
        
        for(int i = 0; i < inHand.Count; i++)
        {
            if(i != newIndex)
            {
                foreach(GameObject obj in inHand[i]) obj.SetActive(false);
            } else
            {
                foreach(GameObject obj in inHand[i]) obj.SetActive(true);
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
        if(inHand.Count > 0)
        {
            foreach(List<GameObject> objs in inHand)
            {
                foreach (GameObject obj in objs) Destroy(obj);
            }

            inHand.Clear();
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
