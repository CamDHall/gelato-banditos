using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using UnityEngine;

public class GelatoCanon : MonoBehaviour {

    Flavors currentFlav;
    public GameObject lemonGelato;
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
		if(Input.GetButtonDown("Cannon") && inHand.Count > 0 && !holding)
        {
            LaunchItem();
        }

        if(Input.GetButtonUp("Cannon") && holding)
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
        } else
        {
            currentItem.GetComponent<StationWeapon>().friendly = true;
            currentItem.transform.SetParent(null);
            currentItem.transform.position = transform.position + (transform.forward * 500);
        }
        currentItem.transform.localRotation = Quaternion.identity;

        holding = true;
        PlayerInventory.Instance.gelato_inventory[currentFlav]--;
    }

    public void UpdateCounter(int direction)
    {
        if(direction < -1 || direction > 1)
        {
            Debug.Log("WRONG DIRECTION VALUE");
            return;
        }

        flavorKeys = new List<Flavors>(PlayerInventory.Instance.gelato_inventory.Keys);

        if (PlayerInventory.Instance.weapons.Count > 0)
        {
            weaponKeys = new List<string>(PlayerInventory.Instance.weapons.Keys);
        }
        string weaponKey = "";
        int num = 0;

        if (direction + cannonIndex < flavorKeys.Count && direction + cannonIndex >= 0)
        {
            Flavors flav;
            flav = flavorKeys[cannonIndex + direction];
            num = PlayerInventory.Instance.gelato_inventory[flav];

            ClearHand();
            for (int i = 0; i < num; i++)
            {
                GameObject temp = Instantiate(lemonGelato, transform);
                temp.transform.localPosition = Vector3.zero;
                temp.transform.rotation = Quaternion.Euler(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
                temp.GetComponent<Gelato>().flavor = flav;

                inHand.Enqueue(temp);
            }
        } else if(weaponKeys != null)
        {
            if (weaponKeys.Count <= (cannonIndex + direction) - flavorKeys.Count)
            {
                return;
            }

            weaponKey = weaponKeys[(cannonIndex + direction) - flavorKeys.Count];

            ClearHand();
            string _tempName = PlayerInventory.Instance.weapons[weaponKey].name;
            _tempName = Regex.Replace(_tempName, "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))", "");
            GameObject weapon = Instantiate(Resources.Load(_tempName) as GameObject, transform);
            weapon.GetComponent<StationWeapon>().friendly = true;
            weapon.GetComponent<StationWeapon>().Disable();
            Debug.Log(transform.parent.position);
            //weapon.transform.localPosition = transform.localPosition;
            weapon.SetActive(true);
            weapon.transform.rotation = Quaternion.Euler(0, 90, 0);
            inHand.Enqueue(weapon);
        }

        cannonIndex += direction;
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
