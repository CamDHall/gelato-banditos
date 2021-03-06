﻿using UnityEngine;
using UnityEngine.UI;

public class ChatBotUI : MonoBehaviour {
    
    public GameObject chatBot;
    public GameObject expand;

    public Color warningColor, goodNewsColor, newsColor, regularColor;

    public float defaultTimerLength;

    bool showing, timed;
    float timer;

    Text text;

    private void Awake()
    {
        text = chatBot.GetComponentInChildren<Text>();
    }

    void Start () {
        Collapse();
	}

    void Update()
    {
        if (timed && Time.timeSinceLevelLoad > timer)
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

        if (isTimed)
        {
            timed = true;
            timer = Time.timeSinceLevelLoad + defaultTimerLength;
        }
    }

    public void Expand(bool isTimed, float timerLength)
    {
        expand.SetActive(false);
        chatBot.gameObject.SetActive(true);
        showing = true;

        if (isTimed)
        {
            timed = true;
            timer = Time.timeSinceLevelLoad + timerLength;
        }
    }

    public void DisplayMessage(string message, bool isTimed, float timerLength)
    {
        text.text = message;
        Expand(isTimed, timerLength);
    }

    public void DisplayMessage(string message, bool isTimed)
    {
        text.text = message;
        Expand(isTimed);
    }

    // Buttons
    public void Question(InputField input)
    {
        input.text = input.text.ToLower();
        ChatBot.Instance.Question(input.text);
    }
}
