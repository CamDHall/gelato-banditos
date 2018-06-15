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
    Queue<GameObject> inHand = new Queue<GameObject>();
    //    List<GameObject> inHand = new List<GameObject>();

    public static GelatoCanon Instance;

    public bool holding = false;
    public Material mat;
    GameObject currentItem;

    int cannonIndex = 0;
    List<Flavors> flavorKeys;
    List<string> weaponKeys;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instance = this;
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
        currentItem = inHand.Dequeue();

        if(currentItem.GetComponent<Gelato>() != null)
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

    public void UpdateCounter(int direction)
    {
        if(direction < -1 || direction > 1)
        {
            Debug.Log("WRONG DIRECTION VALUE");
            return;
        }

        int newIndex = direction + cannonIndex;

        int weaponCount = 0;

        flavorKeys = new List<Flavors>(PlayerInventory.Instance.gelato_inventory.Keys);

        if (PlayerInventory.Instance.weapons.Count > 0)
        {
            weaponKeys = new List<string>(PlayerInventory.Instance.weapons.Keys);
            weaponCount = weaponKeys.Count;
        }
        string weaponKey = "";
        int num = 0;

        if ((weaponCount != 0 && weaponCount + flavorKeys.Count < newIndex) || flavorKeys.Count - 1 < newIndex)
            newIndex = 0;

        if(cannonIndex == 0 && direction == -1)
            newIndex = weaponCount + flavorKeys.Count - 1;


        if(weaponKeys != null && weaponKeys.Count > newIndex)
        {
            weaponKey = weaponKeys[(cannonIndex + direction)];

            ClearHand();
            for (int i = 0; i < weaponCount; i++)
            {
                string _tempName = PlayerInventory.Instance.weapons[weaponKey][0].name;
                _tempName = Regex.Replace(_tempName, "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))", "");
                GameObject weapon = Instantiate(Resources.Load(_tempName) as GameObject, transform);
                weapon.GetComponent<StationWeapon>().friendly = true;
                weapon.GetComponent<StationWeapon>().Disable();
                weapon.transform.rotation = Quaternion.Euler(0, 90, 0);
                weapon.transform.localScale = new Vector3(0.006f, 0.006f, 0.006f);
                weapon.SetActive(true);
                inHand.Enqueue(weapon);
            }
        } else if (newIndex < (flavorKeys.Count + weaponCount))
        {
            Flavors flav;
            flav = flavorKeys[newIndex - weaponCount];
            num = PlayerInventory.Instance.gelato_inventory[flav];

            ClearHand();
            for (int i = 0; i < num; i++)
            {
                GameObject temp = Instantiate(flavorObjects[flav], transform);
                temp.transform.localPosition = Vector3.zero;
                temp.transform.rotation = Quaternion.Euler(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
                temp.GetComponent<Gelato>().flavor = flav;

                inHand.Enqueue(temp);
            }
        }

        cannonIndex = newIndex;
    }

    public void ResetCounter()
    {
        cannonIndex = 0;
        UpdateCounter(0);
    }

    void ClearHand()
    {
        foreach (GameObject obj in inHand)
        {
            Destroy(obj);
        }

        inHand.Clear();
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
