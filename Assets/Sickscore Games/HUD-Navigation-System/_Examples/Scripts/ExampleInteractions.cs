using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SickscoreGames.HUDNavigationSystem;
using SickscoreGames.ExampleScene;

public class ExampleInteractions : MonoBehaviour
{
	#region Variables
	public LayerMask layerMask = 1 << 0;
	public float interactionDistance = 2f;
	public Sprite defaultIcon;
	public Sprite interactedIcon;
	public Image interactionPoint;
	public Text interactionText;

	protected RaycastHit hit;
	private float pointSize;
	#endregion


	#region Main Methods
	void Update ()
	{
		// update interaction input
		HUDNavigationElement element = null;
		bool raycast = Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, interactionDistance, layerMask);
		if (raycast) {
			// get HUD navigation element component
			element = hit.collider.gameObject.GetComponentInChildren<HUDNavigationElement> ();

			// check for interaction input
			if (Input.GetKeyDown (KeyCode.E)) {
				if (element != null) {
					ExampleRotatePrism prismRotator = hit.collider.gameObject.GetComponent<ExampleRotatePrism> ();
					if (element.Icon != interactedIcon) {
						// change icon
						element.Icon = interactedIcon;

						// stop prism rotation
						if (prismRotator != null)
							prismRotator.enabled = false;
					} else {
						// reset icon
						element.Icon = defaultIcon;

						// start prism rotation
						if (prismRotator != null)
							prismRotator.enabled = true;
					}

					// refresh element
					element.Refresh ();
				}
			}
		}

		// animate interaction point
		if (interactionPoint != null)
			interactionPoint.rectTransform.sizeDelta = Vector2.Lerp (interactionPoint.rectTransform.sizeDelta, Vector2.one * ((raycast && element != null) ? 20f : 5f), Time.deltaTime * 8f);

		// show/hide interaction text
		if (interactionText != null)
			interactionText.enabled = raycast && element != null;

		// update radar zoom / indicator border input
		if (Input.GetKey (KeyCode.C) && HUDNavigationSystem.Instance.RadarZoom < 5f)
			HUDNavigationSystem.Instance.RadarZoom += .0175f;
		if (Input.GetKey (KeyCode.V) && HUDNavigationSystem.Instance.RadarZoom > .1f)
			HUDNavigationSystem.Instance.RadarZoom -= .0175f;
		if (Input.GetKey (KeyCode.B) && HUDNavigationSystem.Instance.IndicatorOffscreenBorder < .9f)
			HUDNavigationSystem.Instance.IndicatorOffscreenBorder += .01f;
		if (Input.GetKey (KeyCode.N) && HUDNavigationSystem.Instance.IndicatorOffscreenBorder > .035f)
			HUDNavigationSystem.Instance.IndicatorOffscreenBorder -= .01f;
	}
	#endregion
}
