using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction { ChihuahuaFederation, Juarez }
public class FactionManager : MonoBehaviour {

    public Faction faction;
    public static Faction factionAffil;

    private void Awake()
    {
        factionAffil = faction;
    }
}