using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

    string[] resourceNames, flavorNames, ingredientNames;
    string currentMessage;

    private void Awake()
    {
        Instance = this;

        string filename = Application.dataPath + "/Data/Game Info/keywords.txt";
        keywords = new Dictionary<string, string>();

        string[] temp = File.ReadAllLines(filename);

        foreach(string line in temp)
        {
            string[] pair = line.Split(',');
            keywords.Add(pair[0].ToLower(), pair[1]);
        }

        uiController = GetComponent<ChatBotUI>();
    }

    void Start () {
        resourceNames = System.Enum.GetNames(typeof(ResourceType));
        flavorNames = System.Enum.GetNames(typeof(Flavors));
        ingredientNames = System.Enum.GetNames(typeof(Ingredient));
	}

    public void Question(string text)
    {
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

    public void DisplayMessage(List<ChatMessage> messages, bool isTimed)
    {
        
        uiController.Expand(true);

        string result = "";

        foreach(ChatMessage message in messages)
        {
            result += message.finalMessage + " ";
        }

        uiController.DisplayMessage(result, isTimed);
    }

    void amount()
    {
        List<ChatMessage> messages = new List<ChatMessage>();
        int amount = 0;
        string message = "";
        string color = "";
        bool hadResource = false;
        
        // Resources
        foreach (string resource in resourceNames)
        {
            amount = 0;
            if(currentQuestion.Contains(resource.ToLower()))
            {
                messages.Add(new ChatMessage(data.NEWS_COLOR, "Resources: " + "\n\n"));
                messages.Add(new ChatMessage(data.REGULAR_COLOR, resource + ": " ));
                ResourceType rType =(ResourceType)System.Enum.Parse(typeof(ResourceType), resource);

                if (CharacterManager.Instance.pData.resources.ContainsKey(rType))
                {
                    amount = CharacterManager.Instance.pData.resources[rType];
                }

                if (amount > 0)
                    color = data.GOOD_NEWS_COLOR;
                else
                    color = data.WARNING_COLOR;

                messages.Add(new ChatMessage(color, amount.ToString()));
                DisplayMessage(messages, false);
                return;
            }
        }

        if (currentQuestion.Contains("resource") || currentQuestion.Contains("resources"))
        {
            messages.Add(new ChatMessage(data.NEWS_COLOR, "Resources: " + "\n\n"));

            foreach(ResourceType rType in CharacterManager.Instance.pData.resources.Keys)
            {
                int rAmount = CharacterManager.Instance.pData.resources[rType];

                messages.Add(new ChatMessage(data.REGULAR_COLOR, rType.ToString() + ": " + "\t\t"));

                if (rAmount <= 0) messages.Add(new ChatMessage(data.WARNING_COLOR, rAmount.ToString() + "\n"));
                else messages.Add(new ChatMessage(data.GOOD_NEWS_COLOR, rAmount.ToString() + "\n"));
            }

            DisplayMessage(messages, false);
            return;
        }
        
        // Gelato
        if (currentQuestion.Contains("gelato"))
        {
            messages.Add(new ChatMessage(data.NEWS_COLOR, "Your current Gelato holdings: \n\n"));
            foreach (string flavor in flavorNames)
            {
                amount = 0;

                if (currentQuestion.Contains(flavor.ToLower()))
                {
                    Flavors fType = (Flavors)System.Enum.Parse(typeof(Flavors), flavor);
                    messages.Add(new ChatMessage(data.REGULAR_COLOR, flavor + ": \t\t"));

                    if (CharacterManager.Instance.pData.gelato_inventory.ContainsKey(fType))
                    {
                        amount = CharacterManager.Instance.pData.gelato_inventory[fType];
                    }

                    if (amount > 0) color = data.GOOD_NEWS_COLOR;
                    else color = data.WARNING_COLOR;

                    messages.Add(new ChatMessage(data.WARNING_COLOR, amount.ToString()));
                    DisplayMessage(messages, false);
                    return;
                }
            }

            foreach (string flavor in flavorNames)
            {
                amount = 0;
                Flavors fType = (Flavors)System.Enum.Parse(typeof(Flavors), flavor);
                messages.Add(new ChatMessage(data.REGULAR_COLOR, flavor + ": \t\t"));

                if (CharacterManager.Instance.pData.gelato_inventory.ContainsKey(fType))
                {
                    amount = CharacterManager.Instance.pData.gelato_inventory[fType];
                }

                if (amount > 0) color = data.GOOD_NEWS_COLOR;
                else color = data.WARNING_COLOR;

                messages.Add(new ChatMessage(data.WARNING_COLOR, amount.ToString() + "\n"));
                DisplayMessage(messages, false);
            }

            return;
        }

        // Ingredients
        foreach (string ingredient in ingredientNames)
        {
            amount = 0; // should move these outside of foor loop when looking for single elements
            if (currentQuestion.Contains(ingredient.ToLower()))
            {
                messages.Add(new ChatMessage(data.NEWS_COLOR, "Your current Ingredients: \n\n"));
                Ingredient iType = (Ingredient)System.Enum.Parse(typeof(Ingredient), ingredient);

                if (CharacterManager.Instance.pData.ingredientsHeld.ContainsKey(iType))
                {
                    amount = CharacterManager.Instance.pData.ingredientsHeld[iType];
                }

                if (amount <= 0) color = data.WARNING_COLOR;
                else color = data.GOOD_NEWS_COLOR;

                messages.Add(new ChatMessage(data.REGULAR_COLOR, ingredient + ": \t\t"));
                messages.Add(new ChatMessage(color, amount.ToString()));
                DisplayMessage(messages, false);
                return;
            }
        }

        if(currentQuestion.Contains("ingredient") || currentQuestion.Contains("ingredients"))
        {
            messages.Add(new ChatMessage(data.NEWS_COLOR, "Your current Ingredients: \n\n"));

            foreach(Ingredient ing in CharacterManager.Instance.pData.ingredientsHeld.Keys)
            {
                amount = 0;
                if(CharacterManager.Instance.pData.ingredientsHeld.ContainsKey(ing))
                    amount = CharacterManager.Instance.pData.ingredientsHeld[ing];

                if (amount <= 0) color = data.WARNING_COLOR;
                else color = data.GOOD_NEWS_COLOR;

                messages.Add(new ChatMessage(data.REGULAR_COLOR, ing + ": \t\t"));
                messages.Add(new ChatMessage(color, amount.ToString() + "\n"));
            }

            DisplayMessage(messages, false);
            return;
        }

        messages.Add(new ChatMessage(data.REGULAR_COLOR, "Sorry, I don't know what that is."));
        DisplayMessage(messages, false);
    }
}
