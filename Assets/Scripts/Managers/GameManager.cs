using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public GameObject finalScore, death;

    public int score;

    public Image i_left, i_right, i_top, i_bottom;

    bool leftActive, rightActive, topActive, bottomActive;
    Color bc;

    public GameObject bulletContainer;
    public bool game_over = false;

    [HideInInspector]
    public List<GameObject> friends = new List<GameObject>();

    [HideInInspector] public SpaceStation nearestStation;
    public Dictionary<Affilation, List<Flavors>> affilation_preferences = new Dictionary<Affilation, List<Flavors>>();

	void Start () {
        Instance = this;

        bc = i_left.color;

        List<Flavors> _chFed = new List<Flavors>();
        List<Flavors> _juarez = new List<Flavors>();

        _chFed.Add(Flavors.Lemon);
        _juarez.Add(Flavors.Mango);
        _juarez.Add(Flavors.Chocolate);

        affilation_preferences.Add(Affilation.ChihuahuaFederation, _chFed);
        affilation_preferences.Add(Affilation.Juarez, _juarez);
	}
	
	void Update () {
        if (Input.GetButton("Quit"))
        {
            Application.Quit();
        }

        if (Input.GetButton("Start"))
        {
            SceneManager.LoadScene("main");
        }

        if (!game_over)
        {
            if (leftActive && i_left.color.a > 0)
            {
                i_left.color = new Color(bc.r, bc.g, bc.b, i_left.color.a - Time.deltaTime);
            }
            else
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

    }

    void End()
    {
        finalScore.GetComponent<Text>().text = "Final score: " + score;
        finalScore.SetActive(true);
        game_over = true;
    }

    public void StartGame()
    {
        PlayerMovement.player.enabled = true;
    }

    public void Death()
    {
        death.SetActive(true);
        game_over = true;
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
