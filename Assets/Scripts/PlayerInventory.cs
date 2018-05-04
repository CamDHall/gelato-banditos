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

    public GameObject gelatoContainer;
    Flavor[] flavors;

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

        GelatoCanon.Instance.UpdateCounter(Flavors.Lemon);
        /// Temp
        /// 

        flavors = gelatoContainer.GetComponents<Flavor>();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
