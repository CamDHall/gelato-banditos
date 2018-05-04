using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelatoCanon : MonoBehaviour {

    Flavors currentFlav;
    public GameObject lemonGelato;
    Queue<GameObject> inHand = new Queue<GameObject>();
    //    List<GameObject> inHand = new List<GameObject>();

    public static GelatoCanon Instance;

    public bool holding = false;
    public Material mat;
    GameObject currentCone;
    float holdTimer = 0;

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
            PlaceCone();
            holdTimer = Time.timeSinceLevelLoad + 1;
        }

        if(Input.GetButtonUp("Cannon") && holding)
        {
            holding = false;
            Gelato g = currentCone.GetComponent<Gelato>();
            RaycastHit hit;

            if(Physics.SphereCast(transform.position, 50, transform.forward, out hit) && hit.transform.tag == "Bandito")
            {
                AudioManager.Instance.Whip();
                g.target = hit.transform;
            } else
            {
                g.dir = transform.forward;
            }

            g.bc.enabled = true;
            g.launched = true;
        }
	}

    void PlaceCone()
    {
        currentCone = inHand.Dequeue();

        currentCone.transform.SetParent(transform.parent.parent);
        currentCone.transform.localPosition = new Vector3(0, 0.25f, 0);

        currentCone.transform.localRotation = Quaternion.identity;

        holding = true;
        PlayerInventory.Instance.gelato_inventory[currentFlav]--;
    }

    public void UpdateCounter(Flavors flav)
    {
        int num = PlayerInventory.Instance.gelato_inventory[flav];

        for (int i = 0; i < num; i++)
        {
            GameObject temp = Instantiate(lemonGelato, transform);
            temp.transform.localPosition = Vector3.zero;
            temp.transform.rotation = Quaternion.Euler(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));

            temp.GetComponent<Gelato>().flavor = flav;

            inHand.Enqueue(temp);
        }
    }
}
