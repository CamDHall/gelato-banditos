using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public static MenuManager Instance;
    public Button flavorRecipe;

    public GameObject gelatoSub;
    public GameObject gelatoContainer;
    Flavor[] possibleFlavors;

    Vector2 padding = new Vector2(100, 50);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (gelatoContainer != null) possibleFlavors = gelatoContainer.GetComponents<Flavor>();
    }

    public void DisplayRecipes()
    {
        int x = -2, y = -2;

        foreach(Flavor flav in possibleFlavors)
        {
            Button temp = Instantiate(flavorRecipe);

            temp.transform.SetParent(gelatoSub.transform, false);
            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * 150, y * 100) + padding;

            if (x <= 2)
            {
                x++;
            } else
            {
                x = 0;
                if (y <= 2) y++;
                else y = -2;
            }

            temp.GetComponentInChildren<Text>().text = flav.flavor.ToString();

            if (!Utilts.CanMakeRecipe(flav)) temp.image.color = Color.red; 
        }
    }

    public void MakeReceipe()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;

        try
        {
            PlayerInventory.Instance.gelato_inventory[(Flavors)System.Enum.Parse(typeof(Flavors), name)]++;
        } catch
        {
            throw new System.Exception("Gelato Flavor Name Mismatch");
        }
    }
}
