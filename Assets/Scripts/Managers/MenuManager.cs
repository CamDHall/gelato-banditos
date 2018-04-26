using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public static MenuManager Instance;
    public Button flavorRecipe;

    private void Awake()
    {
        Instance = this;
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
