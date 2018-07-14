using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

public enum Ingredient { VanillaBean, CocoaBean, Lemon, Mango, Strawberry }
public enum ResourceType { Copper, Iron, Steel }
public enum StandingType { ENEMY, UNFRIENDLY, NEUTRAL, FRIENDLY, ALLIED}
public class CharacterManager : SerializedMonoBehaviour {

    public bool resetData;

    public static CharacterManager Instance;

    public CharacterMovement character;
    public PlayerData pData;

    float dPadHorizontal = 0;
    bool horizontalPadInUse = false;

    public bool inStation = false;

    private void Awake()
    {
        Instance = this;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SpaceStation") inStation = true;
    }

    private void Start()
    {
        pData = DataManager.LoadCharacterData();
        pData.gelatoContainer = GameObject.FindGameObjectWithTag("GelatoContainer");
        pData.flavors = pData.gelatoContainer.GetComponents<Flavor>();

        GelatoCanon.Instance.UpdateCounter(0, true);
    }

    void Update () {
        dPadHorizontal = Input.GetAxis("DpadHorizontal");
        bool tempPad = Input.GetKeyDown(KeyCode.RightArrow);

        if (!horizontalPadInUse)
        {
            if (dPadHorizontal > 0 || tempPad)
            {
                GelatoCanon.Instance.UpdateCounter(1, false);
                horizontalPadInUse = true;
            }
            else if (dPadHorizontal < 0)
            {
                GelatoCanon.Instance.UpdateCounter(-1, false);
                horizontalPadInUse = true;
            }
        }
        else if (dPadHorizontal == 0)
        {
            horizontalPadInUse = false;
        }
    }
}

public class PlayerData
{
    public Dictionary<ResourceType, int> resources;

    public Dictionary<Ingredient, int> ingredientsHeld;
    public Dictionary<Faction, int> standings;
    public Dictionary<Flavors, int> gelato_inventory;
    public Dictionary<string, List<GameObject>> weapons;
    public Dictionary<Faction, Dictionary<FlavorQualities, int>> aff_prefs;
    public GameObject gelatoContainer;
    public Flavor[] flavors;
}