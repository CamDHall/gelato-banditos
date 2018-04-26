using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ingredients { VanillaBean, CocoaBean }
public class PlayerInventory : MonoBehaviour
{

    public static PlayerInventory Instance;

    public int copper, iron;
    public Dictionary<Ingredients, int> ingredientsHeld = new Dictionary<Ingredients, int>();
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

        ingredientsHeld.Add(Ingredients.VanillaBean, 10);
        ingredientsHeld.Add(Ingredients.CocoaBean, 5);
        /// Temp
        /// 

        flavors = gelatoContainer.GetComponents<Flavor>();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
