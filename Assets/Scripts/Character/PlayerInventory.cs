using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Ingredient { VanillaBean, CocoaBean, Lemon, Mango, Strawberry }
public enum ResourceType { Copper, Iron }

public class PlayerInventory : SerializedMonoBehaviour
{
    public static PlayerInventory Instance;
    public PlayerData playerData = new PlayerData();

    float dPadHorizontal = 0;
    bool HorizontalPadInUse = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        /// Temp
        playerData.standings = new Dictionary<Affilation, int>();
        playerData.standings.Add(Affilation.ChihuahuaFederation, 0);

        playerData.gelato_inventory = new Dictionary<Flavors, int>();
        playerData.gelato_inventory.Add(Flavors.Lemon, 10);
        playerData.gelato_inventory.Add(Flavors.Chocolate, 10);
        playerData.gelato_inventory.Add(Flavors.Mango, 10);
        playerData.gelato_inventory.Add(Flavors.Strawberry, 10);
        playerData.gelato_inventory.Add(Flavors.Vanilla, 10);

        playerData.ingredientsHeld = new Dictionary<Ingredient, int>();
        playerData.ingredientsHeld.Add(Ingredient.VanillaBean, 10);
        playerData.ingredientsHeld.Add(Ingredient.CocoaBean, 5);

        playerData.flavors = playerData.gelatoContainer.GetComponents<Flavor>();
        playerData.weapons = new Dictionary<string, List<GameObject>>();

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
    public GameObject gelatoContainer;
    public Flavor[] flavors;
}
