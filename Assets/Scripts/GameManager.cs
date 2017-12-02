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

    bool leftActive, rightActive, topActive, bottomActive;
    Color bc;

    public GameObject bulletContainer;

	void Start () {
        Instance = this;

        bc = i_left.color;
	}
	
	void Update () {
        /* timeLeft.text = "Time: " + System.Math.Round(timeGoal - Time.time, 2);
         if(timeGoal < Time.time)
         {
             End();
         }*/

        if(Input.GetKeyDown(KeyCode.R))
        {
            Application.Quit();
        }

        if (Input.GetButton("Start"))
        {
            SceneManager.LoadScene("testing");
        }

        if(leftActive && i_left.color.a > 0)
        {
            i_left.color = new Color(bc.r, bc.g, bc.b, i_left.color.a - Time.deltaTime);
        } else
        {
            leftActive = false;
        }
        if (rightActive && i_right.color.a > 0)
        {
            i_right.color = new Color(bc.r, bc.g, bc.b, i_right.color.a - Time.deltaTime);
        }
        else
        {
            rightActive = false;
        }
        if (topActive && i_top.color.a > 0)
        {
            i_top.color = new Color(bc.r, bc.g, bc.b, i_top.color.a - Time.deltaTime);
        }
        else
        {
            topActive = false;
        }
        if (bottomActive && i_bottom.color.a > 0)
        {
            i_bottom.color = new Color(bc.r, bc.g, bc.b, i_bottom.color.a - Time.deltaTime);
        }
        else
        {
            bottomActive = false;
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
        if (side == "Left" && !leftActive)
        {
            i_left.color = new Color(bc.r, bc.g, bc.b, 1);
            leftActive = true;
        }

        if(side == "Right" && !rightActive)
        {
            i_right.color = new Color(bc.r, bc.g, bc.b, 1);
            rightActive = true;
        }

        if((side == "Top" || side == "Front") && !topActive)
        {
            i_top.color = new Color(bc.r, bc.g, bc.b, 1);
            topActive = true;
        }

        if((side == "Bottom" || side == "Back") && !bottomActive)
        {
            i_bottom.color = new Color(bc.r, bc.g, bc.b, 1);
            bottomActive = true;
        }
    }
}
