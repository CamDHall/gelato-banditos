using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Ingredient { VanillaBean, CocoaBean, Lemon, Mango, Strawberry }
public enum ResourceType { Copper, Iron }
public class PlayerInventory : SerializedMonoBehaviour
{

    public static PlayerInventory Instance;

    public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    public Dictionary<Ingredient, int> ingredientsHeld = new Dictionary<Ingredient, int>();
    public Dictionary<Affilation, int> standings = new Dictionary<Affilation, int>();
    public Dictionary<Flavors, int> gelato_inventory = new Dictionary<Flavors, int>();
    public Dictionary<string, List<GameObject>> weapons = new Dictionary<string, List<GameObject>>();
    public GameObject gelatoContainer;
    Flavor[] flavors;

    float dPadHorizontal = 0;
    bool HorizontalPadInUse = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        /// Temp
        standings.Add(Affilation.ChihuahuaFederation, 0);
        gelato_inventory.Add(Flavors.Lemon, 10);
        gelato_inventory.Add(Flavors.Chocolate, 10);
        gelato_inventory.Add(Flavors.Mango, 10);
        gelato_inventory.Add(Flavors.Strawberry, 10);
        gelato_inventory.Add(Flavors.Vanilla, 10);

        ingredientsHeld.Add(Ingredient.VanillaBean, 10);
        ingredientsHeld.Add(Ingredient.CocoaBean, 5);

        flavors = gelatoContainer.GetComponents<Flavor>();
        weapons = new Dictionary<string, List<GameObject>>();

        GelatoCanon.Instance.UpdateCounter(0);
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
                GelatoCanon.Instance.UpdateCounter(1);
                HorizontalPadInUse = true;
            }
            else if (dPadHorizontal < 0)
            {
                GelatoCanon.Instance.UpdateCounter(-1);
                HorizontalPadInUse = true;
            }
        } else if(dPadHorizontal ==  0) 
        {
            HorizontalPadInUse = false;
        }
    }
}
