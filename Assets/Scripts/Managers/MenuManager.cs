using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public static MenuManager Instance;
    public Button flavorRecipe;

    public GameObject gelatoSub;
    public GameObject gelatoContainer;
    Flavor[] possibleFlavors;

    List<Button> flavBtns = new List<Button>();
    Vector2 padding = new Vector2(100, 50);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (gelatoContainer != null)
        {
            possibleFlavors = gelatoContainer.GetComponents<Flavor>();

            int x = -2, y = -2;

            foreach (Flavor flav in possibleFlavors)
            {
                Button temp = Instantiate(flavorRecipe);

                temp.transform.SetParent(gelatoSub.transform, false);
                temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * 150, y * 100) + padding;

                if (x <= 2)
                {
                    x++;
                }
                else
                {
                    x = 0;
                    if (y <= 2) y++;
                    else y = -2;
                }

                temp.GetComponentInChildren<Text>().text = flav.flavor.ToString();
                temp.gameObject.name = flav.flavor.ToString();
                temp.onClick.AddListener(MakeReceipe);

                temp.GetComponent<FlavGetter>().associatedFlav = flav;

                if (!Utilts.CanMakeRecipe(flav))
                {
                    temp.enabled = false;
                    temp.image.color = Color.red;
                }

                flavBtns.Add(temp);
                temp.gameObject.SetActive(false);
            }
        }
    }

    public void DisplayRecipes()
    {
        foreach(Button btn in flavBtns)
        {
            btn.gameObject.SetActive(true);
        }
    }

    public void UpdateCraftableRecipes()
    {
        foreach(Button btn in flavBtns)
        {
            Flavor flav = btn.GetComponent<FlavGetter>().associatedFlav;

            if (!Utilts.CanMakeRecipe(flav))
            {
                btn.enabled = false;
                btn.GetComponent<Image>().color = Color.red;
            }
        }
    }

    public void MakeReceipe()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        string name = go.name;

        Flavors flav;
        try
        {
            flav = (Flavors)System.Enum.Parse(typeof(Flavors), name);

            if(PlayerInventory.Instance.gelato_inventory.ContainsKey(flav))
            {
                PlayerInventory.Instance.gelato_inventory[flav]++;
            } else
            {
                PlayerInventory.Instance.gelato_inventory.Add(flav, 1);
            }
        } catch
        {
            throw new System.Exception("Gelato Flavor Mismatch");
        }

        Flavor flavClass = go.GetComponent<FlavGetter>().associatedFlav;

        Ingredients[] ingredients = flavClass.ingredientsNeeded.Keys.ToArray();

        foreach(Ingredients ing in flavClass.ingredientsNeeded.Keys)
        {
            int removeAmount = flavClass.ingredientsNeeded[ing];
            int amountHeld = PlayerInventory.Instance.ingredientsHeld[ing];

            if(amountHeld - removeAmount == 0)
            {
                PlayerInventory.Instance.ingredientsHeld.Remove(ing);
            } else
            {
                PlayerInventory.Instance.ingredientsHeld[ing] -= removeAmount;
            }
        }

        UpdateCraftableRecipes();
    }
}
