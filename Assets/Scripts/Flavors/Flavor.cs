using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ingredients { Sugar, VanillaBean, Lemon, Cocoa}
public class Flavor {

    public string name;
    public Dictionary<Ingredients, int> requiredIng;


}
