using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour {

    public Affilation afil;
    public static Affilation factionAffil;

    private void Awake()
    {
        factionAffil = afil;
    }
}