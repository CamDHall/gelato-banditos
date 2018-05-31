using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoreItemType { Ingriedent, StationWeapon }
public class StoreItemInfo : MonoBehaviour {

    public int cost;
    public ResourceType resType;
    public StoreItemType itemType;
    public GameObject obj;
    public string name;
}
