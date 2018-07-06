using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SickscoreGames.HUDNavigationSystem; // MANDATORY !!

public class ExampleDynamicElements : MonoBehaviour
{
	public HUDNavigationElementSettings Settings;
	public Sprite[] Icons;
	protected GameObject ElementHolder;


	void Awake ()
	{
		ElementHolder = new GameObject ("Random Element Holder");
	}


	void Update ()
	{
		// add random element when pressing space
		if (Input.GetKeyDown (KeyCode.Tab))
			CreateRandomElement ();
	}


	void CreateRandomElement ()
	{
		// create demo gameobject at random position
		GameObject go = new GameObject ("Random Element");
		go.transform.SetParent (ElementHolder.transform);
		go.transform.position = new Vector3 (Random.Range (-25f, 25f), Random.Range (.5f, 25f), Random.Range (-25f, 25f));

		// add navigation element
		HUDNavigationElement element = go.AddComponent<HUDNavigationElement> ();


		//########################
		// Element Customizations
		//########################

		// use settings asset / manual settings
		if (Settings != null)
			element.Settings = Settings;
		else {
			// set random icon
			if (Icons.Length > 0)
				element.Icon = Icons [Random.Range (0, Icons.Length)];

			// icon sizes
			element.RadarIcon.IconSize = 8;
			element.CompassBarIcon.IconSize = 22;
			element.IndicatorIcon.IconSize = 16;
			element.OffscreenIndicatorIcon.IconSize = 16;

			// icon colors
			Color _color = new Color (Random.value, Random.value, Random.value, 1f);
			element.RadarIcon.IconColor = _color;
			element.CompassBarIcon.IconColor = _color;
			element.IndicatorIcon.IconColor = _color;
			element.OffscreenIndicatorIcon.IconColor = _color;

			// indicator settings
			element.ShowIndicator = true;
			element.IgnoreIndicatorRadius = true;

			//...
		}
	}
}
