using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicUI : MonoBehaviour {

    public RectTransform flavor_prefab;
    public RectTransform panel;

	void Start () {
		
	}
	
	void Update () {

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
    }

    public void GiveGelato()
    {
        SpaceStation station = GameManager.Instance.nearestStation;

        panel.gameObject.SetActive(true);

        List<Flavors> flavors = new List<Flavors>(PlayerInventory.Instance.gelato_inventory.Keys);

        int count = 0;

        Vector2 padding = new Vector2(100, 50);

        for(int y = -2; y < 2; y++)
        {
            for(int x = -2; x < 2; x++)
            {
                count++;
                RectTransform temp = (RectTransform)Instantiate(flavor_prefab);
                temp.transform.SetParent(CameraManager.Instance.cinematicCanvas.transform, false);
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
}
