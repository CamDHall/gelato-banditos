using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text coneCount;
    public Image health;
    public Image shield;

	void Start () {
		
	}
	
	void Update () {
        coneCount.text = "x" + GameManager.Instance.cones.Count;

        if (PlayerMovement.player.shield > 0)
        {
            shield.fillAmount = PlayerMovement.player.shield / PlayerMovement.player.startShield;
        }
        else
        {
            health.fillAmount = PlayerMovement.player.health / PlayerMovement.player.startHealth;
        }
	}
}
