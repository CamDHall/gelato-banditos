using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public Text coneCount;
    public Image health;
    public Image shield;

    public Image indicator;

	void Start () {
		
	}
	
	void Update () {

        if (PlayerMovement.player.shield > 0)
        {
            shield.fillAmount = PlayerMovement.player.shield / PlayerMovement.player.startShield;
        }
        else
        {
            shield.fillAmount = 0;
            health.fillAmount = PlayerMovement.player.health / PlayerMovement.player.startHealth;
        }
	}

    public void LoadGalaxy()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        Debug.Log(clicked.name);
        SceneManager.LoadScene(clicked.name);
    }

    public void InvertControls()
    {
        GameManager.Instance.invert = !GameManager.Instance.invert;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Juarez");
    }
}
