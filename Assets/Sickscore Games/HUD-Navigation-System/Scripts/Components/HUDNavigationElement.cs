using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using SickscoreGames;

namespace SickscoreGames.HUDNavigationSystem
{
	[AddComponentMenu (HNS.Name + "/HUD Navigation Element")]
	public class HUDNavigationElement : MonoBehaviour
	{
		#region Variables
		[Tooltip("(optional) Assign to use a settings asset instead.")]
		public HUDNavigationElementSettings Settings;

		[Tooltip("Assign the icon you want to use for this element.")]
		public Sprite Icon;
		public IconSettings RadarIcon = new IconSettings();
		public IconSettings CompassBarIcon = new IconSettings();
		public IconSettings IndicatorIcon = new IconSettings();
		public IconSettings OffscreenIndicatorIcon = new IconSettings();

		[Tooltip("If enabled, this element will be hidden within the radar.")]
		public bool HideInRadar = false;

		[Tooltip("If enabled, this element will be hidden within the compass bar.")]
		public bool HideInCompassBar = false;
		[Tooltip("Enable, if this element should always be visible within the compass bar. Useful for e.g. quest markers.")]
		public bool IgnoreCompassBarRadius = false;

		[Tooltip("Enable, if you want to show an indicator at the elements position.")]
		public bool ShowIndicator = true;
		[Tooltip("Enable, if the indicator of this element should always be visible. Useful for e.g. quest markers.")]
		public bool IgnoreIndicatorRadius = true;
		[Tooltip("Define an offset for the indicator relative to the pivot of the element.")]
		public Vector3 IndicatorOffset = Vector3.zero;


		public NavigatonElementEvent OnAppear = new NavigatonElementEvent ();
		public NavigatonElementEvent OnDisappear = new NavigatonElementEvent ();
		public NavigatonElementEvent OnEnterRadius = new NavigatonElementEvent ();
		public NavigatonElementEvent OnLeaveRadius = new NavigatonElementEvent ();


		[HideInInspector]
		public bool IsActive = true;

		[HideInInspector]
		public Text Distance;

		[HideInInspector]
		public Text IndicatorDistance;

		[HideInInspector]
		public Image RadarMarker;

		[HideInInspector]
		public Image CompassBarMarker;

		[HideInInspector]
		public Image Indicator;

		[HideInInspector]
		public bool IsInRadarRadius;

		[HideInInspector]
		public bool IsInCompassBarRadius;

		[HideInInspector]
		public bool IsInIndicatorRadius;


		protected bool isInitialized = false;
		#endregion


		#region Main Methods
		void Start ()
		{
			// disable, if navigation system is missing
			if (HUDNavigationSystem.Instance == null) {
				Debug.LogError ("HUDNavigationSystem not found in scene!");
				this.enabled = false;
				return;
			}

			// initialize settings
			InitializeSettings ();

			// initialize components
			Initialize ();
		}


		void InitializeSettings ()
		{
			if (Settings == null)
				return;

			// icon settings
			this.Icon = Settings.Icon;
			this.RadarIcon = Settings.RadarIcon;
			this.CompassBarIcon = Settings.CompassBarIcon;
			this.IndicatorIcon = Settings.IndicatorIcon;
			this.OffscreenIndicatorIcon = Settings.OffscreenIndicatorIcon;
			// radar settings
			this.HideInRadar = Settings.HideInRadar;
			// compass bar settings
			this.HideInCompassBar = Settings.HideInCompassBar;
			this.IgnoreCompassBarRadius = Settings.IgnoreCompassBarRadius;
			// indicator settings
			this.ShowIndicator = Settings.ShowIndicator;
			this.IgnoreIndicatorRadius = Settings.IgnoreIndicatorRadius;
			this.IndicatorOffset = Settings.IndicatorOffset;
		}


		void Initialize ()
		{
			// create marker references
			CreateMarkerReferences ();

			// add element to the navigation system
			if (HUDNavigationSystem.Instance != null)
				HUDNavigationSystem.Instance.AddNavigationElement (this);

			isInitialized = true;
		}


		void OnEnable ()
		{
			if (!isInitialized)
				return;
			
			Initialize ();
		}


		void OnDisable ()
		{
			// remove element from the navigation system
			if (HUDNavigationSystem.Instance != null)
				HUDNavigationSystem.Instance.RemoveNavigationElement (this);

			// destroy all marker references
			if (RadarMarker != null)
				Destroy (RadarMarker.gameObject);
			if (CompassBarMarker != null)
				Destroy (CompassBarMarker.gameObject);
			if (Indicator != null)
				Destroy (Indicator.gameObject);
		}


		public void Refresh ()
		{
			this.enabled = false;

			// reset markers
			RadarMarker = null;
			CompassBarMarker = null;
			Indicator = null;

			// create marker references
			CreateMarkerReferences ();

			this.enabled = true;
		}
		#endregion


		#region Utility Methods
		void CreateMarkerReferences ()
		{
			// create marker references
			CreateRadarMarker ();
			CreateCompassBarMarker ();
			CreateIndicatorMarker ();
		}

		void CreateRadarMarker ()
		{
			if (!HUDNavigationSystem.Instance.useRadar || RadarMarker != null)
				return;

			// create marker gameobject
			GameObject markerGO = new GameObject (this.gameObject.name + "_Marker");
			markerGO.transform.SetParent (HUDNavigationCanvas.Instance.Radar.ElementContainer, false);
			markerGO.SetActive (false);

			// setup marker component
			RadarMarker = markerGO.AddComponent<Image> ();
			RadarMarker.rectTransform.sizeDelta = Vector2.one * RadarIcon.IconSize;
			RadarMarker.color = RadarIcon.IconColor;
			RadarMarker.preserveAspect = true;

			// add icon to marker
			if (RadarIcon.OverrideIcon != null)
				RadarMarker.sprite = RadarIcon.OverrideIcon;
			else if (Icon != null)
				RadarMarker.sprite = Icon;
		}


		void CreateCompassBarMarker ()
		{
			if (!HUDNavigationSystem.Instance.useCompassBar || CompassBarMarker != null)
				return;

			// create marker gameobject
			GameObject markerGO = new GameObject (this.gameObject.name + "_Marker");
			markerGO.transform.SetParent (HUDNavigationCanvas.Instance.CompassBar.ElementContainer, false);
			markerGO.SetActive (false);

			// setup marker component
			CompassBarMarker = markerGO.AddComponent<Image> ();
			CompassBarMarker.rectTransform.sizeDelta = Vector2.one * CompassBarIcon.IconSize;
			CompassBarMarker.color = CompassBarIcon.IconColor;
			CompassBarMarker.preserveAspect = true;

			// add icon to marker
			if (CompassBarIcon.OverrideIcon != null)
				CompassBarMarker.sprite = CompassBarIcon.OverrideIcon;
			else if (Icon != null)
				CompassBarMarker.sprite = Icon;

			// setup distance text
			if (HUDNavigationSystem.Instance.useCompassBarDistanceText)
				AddDistanceText (HUDNavigationSystem.Instance.CompassBarDistanceText, markerGO, out Distance);
		}


		void CreateIndicatorMarker ()
		{
			if (!ShowIndicator || Indicator != null)
				return;

			// create indicator gameobject
			GameObject indicatorGO = new GameObject (this.gameObject.name + "_Indicator");
			indicatorGO.transform.SetParent (HUDNavigationCanvas.Instance.Indicator.ElementContainer, false);
			indicatorGO.SetActive (false);

			// setup indicator component
			Indicator = indicatorGO.AddComponent<Image> ();
			Indicator.rectTransform.sizeDelta = Vector2.one * IndicatorIcon.IconSize;
			Indicator.color = IndicatorIcon.IconColor;
			Indicator.preserveAspect = true;

			// add icon to marker
			if (IndicatorIcon.OverrideIcon != null)
				Indicator.sprite = IndicatorIcon.OverrideIcon;
			else if (Icon != null)
				Indicator.sprite = Icon;

			// setup distance text
			if (HUDNavigationSystem.Instance.useIndicatorDistanceText)
				AddDistanceText (HUDNavigationSystem.Instance.IndicatorDistanceText, indicatorGO, out IndicatorDistance);
		}


		void AddDistanceText (HUDNavigationSystem._DistanceText _distanceText, GameObject parentGO, out Text textVar)
		{
			GameObject distanceGO = _distanceText.TextPrefab;
			if (distanceGO != null) {
				// instantiate distance text prefab
				distanceGO = Instantiate (distanceGO, Vector3.zero, Quaternion.identity) as GameObject;
				distanceGO.transform.SetParent (parentGO.transform, false);
				distanceGO.transform.localPosition = _distanceText.TextOffset;

				// get text component
				textVar = distanceGO.GetComponent<Text> ();
				if (textVar != null)
					textVar.text = string.Empty;
				else
					Debug.LogError ("Distance Text Prefab has no Text component!");
			} else {
				textVar = null;
			}
		}
		#endregion
	}


	#region Subclasses
	[System.Serializable]
	public class IconSettings
	{
		[Range(1f, 100f), Tooltip("Select the icon size.")]
		public float IconSize = 16f;
		[Tooltip("Select the icon color. Set to white for default icon appearance.")]
		public Color IconColor = Color.white;
		[Tooltip("(optional) Override the shared icon.")]
		public Sprite OverrideIcon;
	}


	[System.Serializable]
	public enum NavigationElementType { Radar, CompassBar, Indicator };


	[System.Serializable]
	public class NavigatonElementEvent : UnityEvent<NavigationElementType> {}
	#endregion
}
