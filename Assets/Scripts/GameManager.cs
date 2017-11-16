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

	void Start () {
        Instance = this;
	}
	
	void Update () {
        timeLeft.text = "Time: " + System.Math.Round(timeGoal - Time.time, 2);
        if(Input.GetButton("Start"))
        {
            SceneManager.LoadScene("testing");
        }
        if(timeGoal < Time.time)
        {
            End();
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
}
