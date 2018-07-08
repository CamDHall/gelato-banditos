using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// I'm using an enum instead of passing an array of colors for two reasons
/// 1. When creating messages I should be thinking about the order of importance, not color
/// 2. MOST IMPORANT: Later different types may have animations, like shake etc...
/// </summary>
public enum ChatMessage { Regular, Warning, News, GoodNews }

public class ChatBot : MonoBehaviour {

    public static ChatBot Instance;
    public GameObject chatBot;
    public GameObject expand;

    public float timerLength;

    public Color warningColor, goodNewsColor, newsColor, regularColor;

    string _warningColor, _goodNewsColor, _newsColor, _regularColor; 
    Text text;
    bool showing, timed;

    // type checks
    bool containsNews, containsWarning, containsGoodNews, containsRegular;

    float timer;
    Dictionary<string, string> keywords;
    string currentQuestion;

    string[] resourceNames, flavorNames, ingredienNames;

    private void Awake()
    {
        Instance = this;
        text = chatBot.GetComponentInChildren<Text>();
        _warningColor = ColorUtility.ToHtmlStringRGB(warningColor);
        _goodNewsColor = ColorUtility.ToHtmlStringRGB(goodNewsColor);
        _newsColor = ColorUtility.ToHtmlStringRGB(newsColor);
        _regularColor = ColorUtility.ToHtmlStringRGB(regularColor);

        string filename = Application.dataPath + "/Data/Game Info/keywords.txt";
        keywords = new Dictionary<string, string>();

        string[] temp = File.ReadAllLines(filename);

        foreach(string line in temp)
        {
            string[] pair = line.Split(',');
            keywords.Add(pair[0].ToLower(), pair[1]);
        }
    }

    void Start () {
        resourceNames = System.Enum.GetNames(typeof(ResourceType));
        flavorNames = System.Enum.GetNames(typeof(Flavors));
        ingredienNames = System.Enum.GetNames(typeof(Ingredient));
        Collapse();
	}
	
	void Update () {
		if(timed && Time.timeSinceLevelLoad > timer)
        {
            Collapse();
            timed = false;
        }
	}

    public void Collapse()
    {
        chatBot.gameObject.SetActive(false);
        expand.SetActive(true);
        timed = false;
    }
    
    public void Expand(bool isTimed)
    {
        expand.SetActive(false);
        chatBot.gameObject.SetActive(true);
        showing = true;

        if(isTimed)
        {
            timed = true;
            timer = Time.timeSinceLevelLoad + timerLength;
        }
    }

    public void DisplayMessage(Dictionary<ChatMessage, string> message)
    {
        
        Expand(true);

        string str = "";

        foreach(ChatMessage messageType in message.Keys)
        {
            string _color;
            if (messageType == ChatMessage.Regular) _color = _regularColor;
            else if (messageType == ChatMessage.News) _color = _newsColor;
            else if (messageType == ChatMessage.GoodNews) _color = _goodNewsColor;
            else _color = _warningColor;

            str += string.Format("<color=#{0}>{1}</color>", _color, message[messageType]);
        }

        text.text = str;
    }

    public void Question(InputField input)
    {
        input.text = input.text.ToLower();

        foreach(string key in keywords.Keys)
        {
            if(input.text.Contains(key))
            {
                currentQuestion = input.text;
                Invoke(keywords[key], 0);
                break;
            }
        }
    }

    void amount()
    {
        Dictionary<ChatMessage, string> result = new Dictionary<ChatMessage, string>();

        int amount = 0;
        string message = "";

        // Resources
        foreach(string resource in resourceNames)
        {
            if(currentQuestion.Contains(resource.ToLower()))
            {
                ResourceType rType =(ResourceType)System.Enum.Parse(typeof(ResourceType), resource);
                result.Add(ChatMessage.Regular, "You currently have: ");

                if (CharacterManager.Instance.pData.resources.ContainsKey(rType))
                {
                    amount = CharacterManager.Instance.pData.resources[rType];
                    message = amount.ToString() + " " + resource;

                    if (amount > 0)
                        result.Add(ChatMessage.GoodNews, message);
                    else
                        result.Add(ChatMessage.Warning, message);

                    DisplayMessage(result);
                    return;
                }

                // Display because they ask for a valid resource, but they have none (not 0)
                message = amount.ToString() + " " + resource;
                result.Add(ChatMessage.Warning, message);
                DisplayMessage(result);
                return;
            }
        }

        // Gelato
        if (currentQuestion.Contains("gelato"))
        {
            foreach (string flavor in flavorNames)
            {
                if (currentQuestion.Contains(flavor.ToLower()))
                {
                    Flavors fType = (Flavors)System.Enum.Parse(typeof(Flavors), flavor);
                    result.Add(ChatMessage.Regular, "You currently have: ");

                    if (CharacterManager.Instance.pData.gelato_inventory.ContainsKey(fType))
                    {
                        amount = CharacterManager.Instance.pData.gelato_inventory[fType];
                        message = amount.ToString() + " " + flavor + " Gelato Cones";

                        if (amount > 0)
                            result.Add(ChatMessage.GoodNews, message);
                        else
                            result.Add(ChatMessage.Warning, message);
                        DisplayMessage(result);
                        return;
                    }

                    // Display because they ask for a valid resource, but they have none (not 0)
                    message = amount.ToString() + " " + flavor;
                    result.Add(ChatMessage.Warning, message);
                    DisplayMessage(result);
                    return;
                }
            }
        }

        // Ingredients
        foreach (string ingredient in ingredienNames)
        {
            if (currentQuestion.Contains(ingredient.ToLower()))
            {
                Ingredient iType = (Ingredient)System.Enum.Parse(typeof(Ingredient), ingredient);
                result.Add(ChatMessage.Regular, "You currently have: ");

                if (CharacterManager.Instance.pData.ingredientsHeld.ContainsKey(iType))
                {
                    amount = CharacterManager.Instance.pData.ingredientsHeld[iType];
                    message = amount.ToString() + " " + ingredient;

                    if (amount > 0)
                        result.Add(ChatMessage.GoodNews, message);
                    else
                        result.Add(ChatMessage.Warning, message);
                    DisplayMessage(result);
                    return;
                }

                // Display because they ask for a valid resource, but they have none (not 0)
                message = amount.ToString() + " " + ingredient;
                result.Add(ChatMessage.Warning, message);
                DisplayMessage(result);
                return;
            }
        }

        result.Add(ChatMessage.News, "Sorry, I don't know what that is.");
        DisplayMessage(result);
    }
}
