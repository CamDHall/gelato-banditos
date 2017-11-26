using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public Text timeLeft;
    public float timeGoal;

    public GameObject finalScore, death;

    public int score;

    public Image i_left, i_right, i_top, i_bottom;

	void Start () {
        Instance = this;
	}
	
	void Update () {
        /* timeLeft.text = "Time: " + System.Math.Round(timeGoal - Time.time, 2);
         if(timeGoal < Time.time)
         {
             End();
         }*/

        if (Input.GetButton("Start"))
        {
            SceneManager.LoadScene("testing");
        }
    }

    void End()
    {
        finalScore.GetComponent<Text>().text = "Final score: " + score;
        finalScore.SetActive(true);
    }

    public void Death()
    {
        death.SetActive(true);
    }

    public void Indicator(string side)
    {
        Debug.Log(side);
        if(side == "Left")
        {
            i_left.color = new Color(i_left.color.r, i_left.color.g, i_left.color.b, 1);
        }

        if(side == "Right")
        {
            i_right.color = new Color(i_right.color.r, i_right.color.g, i_right.color.b, 1);
        }

        if(side == "Top" || side == "Front")
        {
            i_top.color = new Color(i_top.color.r, i_top.color.g, i_top.color.b, 1);
        }

        if(side == "Bottom" || side == "Back")
        {
            i_bottom.color = new Color(i_bottom.color.r, i_bottom.color.g, i_bottom.color.b, 1);
        }
    }
}
