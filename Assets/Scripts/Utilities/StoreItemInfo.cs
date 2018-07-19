using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoreItemType { Ingriedent, StationWeapon }
[CreateAssetMenu(fileName = "Inventory", menuName ="Inventory/FactionInventory", order = 1)]
public class StoreItemInfo : SerializedScriptableObject
{
    public Dictionary<GameObject, Dictionary<ResourceType, int>> weapons;
    public Dictionary<Ingredient, Dictionary<ResourceType, int>> ingredients;
}