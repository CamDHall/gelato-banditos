using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public static PlayerInventory Instance;

    public int copper, iron;
    public Dictionary<string, int> ingriedents = new Dictionary<string, int>();
    public Dictionary<Affilation, int> standings = new Dictionary<Affilation, int>();
    public Dictionary<Flavors, int> gelato_inventory = new Dictionary<Flavors, int>();

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

        ingriedents.Add("Cocoa Bean", 10);
        ingriedents.Add("Sugar", 10);
        ingriedents.Add("Cream", 10);
        ingriedents.Add("Vanilla Extract", 10);
        ingriedents.Add("Lemon Fruit", 10);
        ingriedents.Add("Mango(s)", 10);
        ingriedents.Add("Strawberry(s)", 10);
        ingriedents.Add("Passion Fruit(s)", 10);
        /// Temp
    }


    // Update is called once per frame
    void Update()
    {

    }
}
