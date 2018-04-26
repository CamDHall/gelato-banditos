using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will have to manually change overall quality
/// check if any contradicting qualities exist
/// if so remove one or both
/// </summary>

public enum FlavorQualities { Sweat, Fruity, Savory, Classic, Light, Variety }
public class Flavor : SerializedMonoBehaviour
{
    /// <summary>
    /// When saving gelato that has been made, use a dict that contains a combined flavor (likely a list) and an int for the amount 
    /// </summary>
    /// 
    public Flavors flavor;
    public List<FlavorQualities> flavQualities;
    public Dictionary<Ingredients, int> ingredientsNeeded;
}
