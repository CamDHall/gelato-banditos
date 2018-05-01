using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Switch : MonoBehaviour {

	public void SwitchButton()
    {
        GameObject.Find("EventSystems").GetComponent<EventSystem>().SetSelectedGameObject(gameObject);
    }
}
