using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum Ingredient { VanillaBean, CocoaBean, Lemon, Mango, Strawberry }
public enum ResourceType { Copper, Iron }

public class PlayerInventory : SerializedMonoBehaviour
{
    public static PlayerInventory Instance;
    public PlayerData pData = new PlayerData();

    float dPadHorizontal = 0;
    bool HorizontalPadInUse = false;

    void Awake()
    {
        Instance = this;

        if(File.Exists("PlayerInventory.dat"))
        {
            pData = DataManager.LoadCharacterData();
        }
    }

    void Start()
    {
        /// Temp
        pData.standings = new Dictionary<Affilation, int>();
        pData.standings.Add(Affilation.Juarez, 0);

        pData.gelato_inventory = new Dictionary<Flavors, int>();
        pData.gelato_inventory.Add(Flavors.Lemon, 10);
        pData.gelato_inventory.Add(Flavors.Chocolate, 10);
        pData.gelato_inventory.Add(Flavors.Mango, 10);
        pData.gelato_inventory.Add(Flavors.Strawberry, 10);
        pData.gelato_inventory.Add(Flavors.Vanilla, 10);

        pData.ingredientsHeld = new Dictionary<Ingredient, int>();
        pData.ingredientsHeld.Add(Ingredient.VanillaBean, 10);
        pData.ingredientsHeld.Add(Ingredient.CocoaBean, 5);

        pData.gelatoContainer = GameObject.FindGameObjectWithTag("GelatoContainer");
        pData.flavors = pData.gelatoContainer.GetComponents<Flavor>();
        pData.weapons = new Dictionary<string, List<GameObject>>();

        GelatoCanon.Instance.UpdateCounter(0, true);
        /// Temp
        ///
    }


    // Update is called once per frame
    void Update()
    {
        dPadHorizontal = Input.GetAxis("DpadHorizontal");
        bool tempPad = Input.GetKeyDown(KeyCode.RightArrow);

        if (!HorizontalPadInUse)
        {
            if (dPadHorizontal > 0 || tempPad)
            {
                GelatoCanon.Instance.UpdateCounter(1, false);
                HorizontalPadInUse = true;
            }
            else if (dPadHorizontal < 0)
            {
                GelatoCanon.Instance.UpdateCounter(-1, false);
                HorizontalPadInUse = true;
            }
        } else if(dPadHorizontal ==  0) 
        {
            HorizontalPadInUse = false;
        }
    }
}

public class PlayerData
{
    public Dictionary<ResourceType, int> resources;

    public Dictionary<Ingredient, int> ingredientsHeld;
    public Dictionary<Affilation, int> standings;
    public Dictionary<Flavors, int> gelato_inventory;
    public Dictionary<string, List<GameObject>> weapons;
    public Dictionary<Affilation, Dictionary<FlavorQualities, int>> aff_prefs;
    public GameObject gelatoContainer;
    public Flavor[] flavors;
}
