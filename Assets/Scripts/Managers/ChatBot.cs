using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
/// <summary>
/// I'm using an enum instead of passing an array of colors for two reasons
/// 1. When creating messages I should be thinking about the order of importance, not color
/// 2. MOST IMPORANT: Later different types may have animations, like shake etc...
/// </summary>
public class ChatBot : MonoBehaviour {

    public static ChatBot Instance;
    public StoredData data;
    ChatBotUI uiController;

    // type checks
    bool containsNews, containsWarning, containsGoodNews, containsRegular;

    float timer;
    Dictionary<string, string> keywords;
    string currentQuestion;

    Array allFlavors, allIngredients, allResources, allFactions;
    string currentMessage;

    List<ChatMessage> messages = new List<ChatMessage>();

    private void Awake()
    {
        Instance = this;

        string filename = Application.dataPath + "/Data/Game Info/keywords.txt";
        keywords = new Dictionary<string, string>();

        string[] temp = File.ReadAllLines(filename);

        foreach (string line in temp)
        {
            string[] pair = line.Split(',');
            keywords.Add(pair[0].ToLower(), pair[1]);
        }

        uiController = GetComponent<ChatBotUI>();
    }

    void Start () {
        allFlavors = Enum.GetValues(typeof(Flavors));
        allResources = Enum.GetValues(typeof(ResourceType));
        allIngredients = Enum.GetValues(typeof(Ingredient));
        allFactions = Enum.GetValues(typeof(Faction));
    }

    public void Question(string text)
    {
        if (text.Contains("bean")) text = text.Replace(" bean", "bean");

        foreach (string key in keywords.Keys)
        {
            if (text.Contains(key))
            {
                currentQuestion = text;
                Invoke(keywords[key], 0);
                break;
            }
        }
    }

    public void DisplayMessage(List<ChatMessage> messages, bool isTimed, float timerLength)
    {
        uiController.Expand(isTimed, timerLength);

        string result = "";

        foreach(ChatMessage message in messages)
        {
            result += message.finalMessage + " ";
        }

        uiController.DisplayMessage(result, isTimed, timerLength);
    }

    public void DisplayMessage(List<ChatMessage> messages, bool isTimed)
    {

        uiController.Expand(isTimed);

        string result = "";

        foreach (ChatMessage message in messages)
        {
            result += message.finalMessage + " ";
        }

        uiController.DisplayMessage(result, isTimed);
    }

    void amount()
    {
        bool containedResource = false, containedGelato = false, containedIngredient = false;
        // Resources
        if (currentQuestion.Contains("resource") || currentQuestion.Contains("resources"))
            containedResource = true;

        if (AmountNumber(allResources, CharacterManager.Instance.pData.resources, "Resources", containedResource)) return;

        // Ingredients
        if (currentQuestion.Contains("ingredient") || currentQuestion.Contains("ingredients"))
            containedIngredient = true;
        if (AmountNumber(allIngredients, CharacterManager.Instance.pData.ingredientsHeld, "Ingredients", containedIngredient)) return;

        // Gelato
        if (currentQuestion.Contains("gelato"))
            containedGelato = true;
        if (AmountNumber(allFlavors, CharacterManager.Instance.pData.gelato_inventory, "Gelato", containedGelato)) return;

        messages.Add(new ChatMessage(data.REGULAR_COLOR, "Sorry, I don't know what that is."));
        DisplayMessage(messages, false);
    }

    void standing()
    {
        if (FactionStandings(allFactions, CharacterManager.Instance.pData.standings, true)) return;

        messages.Add(new ChatMessage(data.REGULAR_COLOR, "Sorry, I don't know who that is."));
        DisplayMessage(messages, false);
    }

    bool AmountNumber(Array allVar, IDictionary refDict, string elementName, bool checkAll)
    {
        int amount = 0;
        string color = "";
        messages.Clear();

        messages.Add(new ChatMessage(data.NEWS_COLOR, elementName + ": \n\n"));

        foreach (Enum vType in allVar)
        {
            amount = 0;
            if (refDict.Contains(vType))
                amount = (int)refDict[vType];

            if (amount <= 0) color = data.WARNING_COLOR;
            else color = data.GOOD_NEWS_COLOR;

            string vString = vType.ToString();

            messages.Add(new ChatMessage(data.REGULAR_COLOR, vString + ": \t\t"));

            if (currentQuestion.Contains(vString.ToLower()))
            {
                messages.Clear();
                messages.Add(new ChatMessage(data.NEWS_COLOR, elementName + ": \n\n"));
                messages.Add(new ChatMessage(data.REGULAR_COLOR, vString + ": \t\t"));

                messages.Add(new ChatMessage(color, amount.ToString()));
                DisplayMessage(messages, false);
                return true;
            }

            if(checkAll)
                messages.Add(new ChatMessage(color, amount.ToString() + "\n"));
        }

        if (checkAll) DisplayMessage(messages, false);

        return checkAll;
    }

    bool FactionStandings(Array allVar, IDictionary refDict, bool checkAll)
    {
        int amount = 0;
        StandingType sType;
        string color = "";
        List<ChatMessage> messages = new List<ChatMessage>();

        foreach (Enum vType in allVar)
        {
            amount = 0;
            if (refDict.Contains(vType))
                amount = (int)refDict[vType];

            if (amount < 0 && amount >= -10)
            {
                color = data.REGULAR_COLOR;
                sType = StandingType.UNFRIENDLY;
            }
            else if (amount < -10)
            {
                color = data.WARNING_COLOR;
                sType = StandingType.ENEMY;
            }
            else if (amount < 10)
            {
                color = data.NEWS_COLOR;
                sType = StandingType.FRIENDLY;
            }
            else
            {
                color = data.GOOD_NEWS_COLOR;
                sType = StandingType.ALLIED;
            }

            string vString = vType.ToString();

            messages.Add(new ChatMessage(data.REGULAR_COLOR, vString + ": \t"));

            if (currentQuestion.Contains(vString.ToLower()))
            {
                messages.Clear();
                messages.Add(new ChatMessage(data.REGULAR_COLOR, vString + ": \t"));

                messages.Add(new ChatMessage(color, sType.ToString()));
                DisplayMessage(messages, false);
                return true;
            }

            if (checkAll)
                messages.Add(new ChatMessage(color, sType.ToString() + "\n"));
        }

        if (checkAll) DisplayMessage(messages, false);

        return checkAll;
    }
}
