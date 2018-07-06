using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    private void Awake()
    {
        Instance = this;
        text = chatBot.GetComponentInChildren<Text>();
        _warningColor = ColorUtility.ToHtmlStringRGB(warningColor);
        _goodNewsColor = ColorUtility.ToHtmlStringRGB(goodNewsColor);
        _newsColor = ColorUtility.ToHtmlStringRGB(newsColor);
        _regularColor = ColorUtility.ToHtmlStringRGB(regularColor);
    }

    void Start () {
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
}
