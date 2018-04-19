using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public static PlayerInventory Instance;

    public int copper, iron;
    public Dictionary<Affilation, int> standings = new Dictionary<Affilation, int>();
    public Dictionary<Flavors, int> gelato_inventory = new Dictionary<Flavors, int>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        standings.Add(Affilation.ChihuahuaFederation, 0);
        gelato_inventory.Add(Flavors.Lemon, 10);
        gelato_inventory.Add(Flavors.Chocolate, 10);
        gelato_inventory.Add(Flavors.Mango, 10);
        gelato_inventory.Add(Flavors.Strawberry, 10);
        gelato_inventory.Add(Flavors.Vanilla, 10);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
