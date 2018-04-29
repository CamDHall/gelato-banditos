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
    public Text gelatoInfo;
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

                gelatoSub.SetActive(false);

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

                flavBtns.Add(temp);
            }
        }
    }

    public void DisplayRecipes()
    {
        gelatoSub.SetActive(true);
        foreach(Button btn in flavBtns)
        {
            btn.gameObject.SetActive(true);
        }

        UpdateCraftableRecipes();
        UpdateGelatoMenu();
    }

    public void UpdateGelatoMenu()
    {
        string info = "";

        foreach (Flavors flav in PlayerInventory.Instance.gelato_inventory.Keys)
        {
            info += flav.ToString() + "\t" + PlayerInventory.Instance.gelato_inventory[flav] + "\t\t";
        }

        gelatoInfo.text = info;
    }

    public void UpdateCraftableRecipes()
    {
        foreach(Button btn in flavBtns)
        {
            Flavor flav = btn.GetComponent<FlavGetter>().associatedFlav;
            int amountCanBeMade = Utilts.CanMakeRecipe(flav);

            Dropdown dp = btn.GetComponentInChildren<Dropdown>();

            if (amountCanBeMade == 0)
            {
                btn.enabled = false;
                btn.GetComponent<Image>().color = Color.red;
                if(dp != null) dp.gameObject.SetActive(false);
            } else
            {
                dp.gameObject.SetActive(true);
                btn.GetComponent<Image>().color = Color.white;
                dp.ClearOptions();
                List<string> num = new List<string>();

                for (int i = 0; i < amountCanBeMade; i++)
                {
                    num.Add((i + 1).ToString());
                }

                dp.AddOptions(num);
            }
        }
    }

    public void MakeReceipe()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        string name = go.name;

        Flavors flav;

        int amount = Utilts.GetDropDownVal(go);

        try
        {
            flav = (Flavors)System.Enum.Parse(typeof(Flavors), name);

            if (PlayerInventory.Instance.gelato_inventory.ContainsKey(flav))
            {
                PlayerInventory.Instance.gelato_inventory[flav] += amount;
            } else
            {
                PlayerInventory.Instance.gelato_inventory.Add(flav, amount);
            }
        } catch
        {
            throw new System.Exception("Gelato Flavor Mismatch");
        }

        Flavor flavClass = go.GetComponent<FlavGetter>().associatedFlav;

        Ingredient[] ingredients = flavClass.ingredientsNeeded.Keys.ToArray();

        foreach(Ingredient ing in ingredients)
        {
            int removeAmount = flavClass.ingredientsNeeded[ing] * amount;
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
        UpdateGelatoMenu();
    }
}
