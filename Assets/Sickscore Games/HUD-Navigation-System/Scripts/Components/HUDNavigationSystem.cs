using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SickscoreGames;

namespace SickscoreGames.HUDNavigationSystem
{
	[AddComponentMenu (HNS.Name + "/HUD Navigation System"), DisallowMultipleComponent]
	public class HUDNavigationSystem : MonoBehaviour
	{
		private static HUDNavigationSystem _Instance;
		public static HUDNavigationSystem Instance {
			get {
				if (_Instance == null) {
					_Instance = FindObjectOfType<HUDNavigationSystem> ();
				}
				return _Instance;
			}
		}


		#region Variables
		// REFERENCES
		public Camera PlayerCamera;
		public Transform PlayerController;
		public _RotationReference RotationReference = _RotationReference.Camera;

		// RADAR
		[Tooltip("Enable, if you want to use the compass bar feature.")]
		public bool useRadar = true;
		[Tooltip("Select the type of radar, you want to use.")]
		public RadarTypes RadarType = RadarTypes.RotateRadar;
		[Tooltip("Define the radar zoom. Change value to zoom radar. Set to 1 for default radar zoom.")]
		public float RadarZoom = 1f;
		[Tooltip("Define the radar radius. Elements outside this radius will be displayed on the border of the radar.")]
		public float RadarRadius = 50f;
		[Tooltip("Define the maximum radar radius. Elements will be hidden outside this radius.")]
		public float RadarMaxRadius = 75f;

		// COMPASS BAR
		[Tooltip("Enable, if you want to use the compass bar feature.")]
		public bool useCompassBar = true;
		[Tooltip("Define the compass radius. Elements that don't ignore the radius will be hidden outside this radius.")]
		public float CompassBarRadius = 150f;
		[Tooltip("Change, if you want to modify the north-direction of the compass bar.")]
		public _CompassBarNorth CompassBarNorth = _CompassBarNorth.Forward;
		[Tooltip("Enable, if you want to show a distance text within the compass bar.")]
		public bool useCompassBarDistanceText = false;
		public _DistanceText CompassBarDistanceText;

		// INDICATOR
		[Tooltip("Enable, if you want to use the indicator feature. Must be separately enabled on each element.")]
		public bool useIndicators = true;
		[Tooltip("Define the indicator radius. Indicators that don't ignore the radius will be hidden outside this radius.")]
		public float IndicatorRadius = 50f;
		[Tooltip("Enable, if you want to use an off-screen indicator, when the element not in sight.")]
		public bool useIndicatorOffscreen = true;
		[Tooltip("(optional) Assign an offscreen sprite for the indicator.")]
		public Sprite IndicatorOffscreenSprite = null;
		[Tooltip("Increase this value to move the indicator further away from the screen borders.")]
		public float IndicatorOffscreenBorder = .075f;
		[Tooltip("Enable, if you want to scale the indicator by distance and within defined radius.")]
		public bool useIndicaterScaling = true;
		[Tooltip("Minimum scale of the indicator. Set value to 1, if you don't want your indicator to scale")]
		public float IndicatorMinScale = .85f;
		[Tooltip("Define the indicator scale radius. Indicator will scale when inside this radius. Must be smaller or equal to indicator radius.")]
		public float IndicatorScaleRadius = 25f;
		[Tooltip("Enable, if you want to show a distance text next to the indicator.")]
		public bool useIndicatorDistanceText = false;
		public _DistanceText IndicatorDistanceText;
		[Tooltip("Enable, if you want to hide the distance text when the indicator is off-screen.")]
		public bool hideIndicatorDistanceOffscreen = true;


		[HideInInspector]
		public List<HUDNavigationElement> NavigationElements;


		private HUDNavigationCanvas _HUDNavigationCanvas;
		#endregion


		#region Main Methods
		void Awake ()
		{
			_Instance = this;

			// assign references
			if (_HUDNavigationCanvas == null) {
				_HUDNavigationCanvas = HUDNavigationCanvas.Instance;

				// check if HUDNavigationCanvas exists
				if (_HUDNavigationCanvas == null) {
					Debug.LogError ("HUDNavigationCanvas not found in scene!");
					this.enabled = false;
					return;
				}
			}

			// assign references
			if (PlayerCamera == null && Camera.main != null)
				PlayerCamera = Camera.main;

			// check references
			if (PlayerCamera == null || PlayerController == null) {
				Debug.LogError ("Player references are missing! Please assign them on the HUDNavigationSystem component.");
				this.enabled = false;
				return;
			}

			// init all components
			InitAllComponents ();
		}


		void LateUpdate ()
		{
			// update navigation elements
			UpdateNavigationElements ();

			// update radar
			if (useRadar)
				_HUDNavigationCanvas.UpdateRadar ();

			// update compass bar
			if (useCompassBar)
				_HUDNavigationCanvas.UpdateCompassBar ();
		}


		public void AddNavigationElement (HUDNavigationElement element)
		{
			if (element == null)
				return;

			// add element, if it doesn't exist yet
			if (!NavigationElements.Contains (element))
				NavigationElements.Add (element);
		}


		public void RemoveNavigationElement (HUDNavigationElement element)
		{
			if (element == null)
				return;

			// remove element from list
			NavigationElements.Remove (element);
		}
		#endregion


		#region Utility Methods
		void InitAllComponents ()
		{
			if (_HUDNavigationCanvas == null)
				return;

			// init radar
			if (useRadar)
				_HUDNavigationCanvas.InitRadar ();
			else
				_HUDNavigationCanvas.ShowRadar (false);

			// init compass bar
			if (useCompassBar)
				_HUDNavigationCanvas.InitCompassBar ();
			else
				_HUDNavigationCanvas.ShowCompassBar (false);

			// init indicators
			if (useIndicators)
				_HUDNavigationCanvas.InitIndicators ();
			else
				_HUDNavigationCanvas.ShowIndicators (false);
		}


		void UpdateNavigationElements ()
		{
			if (_HUDNavigationCanvas == null || NavigationElements.Count <= 0)
				return;

			// update navigation elements
			foreach (HUDNavigationElement element in NavigationElements) {
				if (element == null)
					continue;

				// check if element is active
				if (!element.IsActive) {
					// disable all marker instances
					element.SetMarkerActive (NavigationElementType.Radar, false);
					element.SetMarkerActive (NavigationElementType.CompassBar, false);
					element.SetIndicatorActive (false);

					// skip the element
					continue;
				}

				// cache element values
				Vector3 _worldPos = element.GetPosition ();
				Vector3 _screenPos = PlayerCamera.WorldToScreenPoint (_worldPos);
				float _distance = element.GetDistance (PlayerController.transform);

				// update radar
				if (useRadar)
					UpdateRadarElement (element, _screenPos, _distance);

				// update compass bar
				if (useCompassBar)
					UpdateCompassBarElement (element, _screenPos, _distance);

				// update indicator
				UpdateIndicatorElement (element, _screenPos, _distance);
			}
		}


		void UpdateRadarElement (HUDNavigationElement element, Vector3 screenPos, float distance)
		{
			float _scaledRadius = RadarRadius * RadarZoom;
			float _scaledMaxRadius = RadarMaxRadius * RadarZoom;

			// check for destroyed marker
			if (element.RadarMarker == null)
				return;

			// check if element is hidden within the compass bar
			if (element.HideInRadar) {
				element.SetMarkerActive (NavigationElementType.Radar, false);
				return;
			}

			// check distance
			if (distance > _scaledRadius) {
				// invoke events
				if (element.IsInRadarRadius) {
					element.IsInRadarRadius = false;
					element.OnLeaveRadius.Invoke (NavigationElementType.Radar);
				}

				// check max distance
				if (distance > _scaledMaxRadius) {
					element.SetMarkerActive (NavigationElementType.Radar, false);
					return;
				}

				// set scaled distance when out of range
				distance = _scaledRadius;
			} else {
				// rotate marker
				Transform rotationReference = GetRotationReference ();
				if (RadarType == RadarTypes.RotateRadar) {
					element.RadarMarker.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, -element.transform.eulerAngles.y + rotationReference.eulerAngles.y));
				} else {
					element.RadarMarker.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, -element.transform.eulerAngles.y));
				}

				// invoke events
				if (!element.IsInRadarRadius) {
					element.IsInRadarRadius = true;
					element.OnEnterRadius.Invoke (NavigationElementType.Radar);
				}
			}

			// set marker active
			element.SetMarkerActive (NavigationElementType.Radar, true);

			// calculate marker position
			Vector3 posOffset = element.GetPositionOffset (PlayerController);
			Vector3 markerPos = new Vector3 (posOffset.x, posOffset.z, 0f);
			markerPos.Normalize ();
			markerPos *= (distance / _scaledRadius) * (_HUDNavigationCanvas.Radar.ElementContainer.GetRadius () - element.GetIconRadius ());

			// set marker position
			element.SetMarkerPosition (NavigationElementType.Radar, markerPos);
		}


		void UpdateCompassBarElement (HUDNavigationElement element, Vector3 screenPos, float distance)
		{			
			// check if element is hidden within the compass bar
			if (element.HideInCompassBar) {
				element.SetMarkerActive (NavigationElementType.CompassBar, false);
				return;
			}

			// update distance text
			if (useCompassBarDistanceText && element.Distance != null)
				element.Distance.text = string.Format (CompassBarDistanceText.TextFormat, (int)distance);

			// check distance
			if (distance > CompassBarRadius && !element.IgnoreCompassBarRadius) {
				element.SetMarkerActive (NavigationElementType.CompassBar, false);

				// invoke events
				if (element.IsInCompassBarRadius) {
					element.IsInCompassBarRadius = false;
					element.OnLeaveRadius.Invoke (NavigationElementType.CompassBar);
				}

				return;
			}

			// invoke events
			if (!element.IsInCompassBarRadius) {
				element.IsInCompassBarRadius = true;
				element.OnEnterRadius.Invoke (NavigationElementType.CompassBar);
			}

			// set marker position
			if (screenPos.z <= 0) {
				// hide marker and skip element
				element.SetMarkerActive (NavigationElementType.CompassBar, false);
				return;
			}

			// set marker active
			element.SetMarkerActive (NavigationElementType.CompassBar, true);

			// set marker position
			element.SetMarkerPosition (NavigationElementType.CompassBar, screenPos, _HUDNavigationCanvas.CompassBar.ElementContainer);
		}


		void UpdateIndicatorElement (HUDNavigationElement element, Vector3 screenPos, float distance)
		{
			if (useIndicators && element.ShowIndicator) {
				// update distance text
				if (useIndicatorDistanceText && element.IndicatorDistance != null)
					element.IndicatorDistance.text = string.Format (IndicatorDistanceText.TextFormat, (int)distance);
				
				// check distance and visibility
				if ((distance > IndicatorRadius && !element.IgnoreIndicatorRadius) || (screenPos.z <= 0 && !useIndicatorOffscreen)) {
					element.SetIndicatorActive (false);

					// invoke events
					if (element.IsInIndicatorRadius) {
						element.IsInIndicatorRadius = false;
						element.OnLeaveRadius.Invoke (NavigationElementType.Indicator);
					}
				} else {
					// calculate off-screen position, if indicator is not in sight
					if (useIndicatorOffscreen && !element.IsVisibleOnScreen (screenPos)) {
						// flip if indicator is behind us
						if (screenPos.z < 0f) {
							screenPos.x = Screen.width - screenPos.x;
							screenPos.y = Screen.height - screenPos.y;
						}

						// calculate off-screen position/rotation
						Vector3 screenCenter = new Vector3 (Screen.width, Screen.height, 0f) / 2f;
						screenPos -= screenCenter;
						float angle = Mathf.Atan2 (screenPos.y, screenPos.x);
						angle -= 90f * Mathf.Deg2Rad;
						float cos = Mathf.Cos (angle);
						float sin = -Mathf.Sin (angle);
						float cotangent = cos / sin;
						screenPos = screenCenter + new Vector3 (sin * 50f, cos * 50f, 0f);

						// is indicator inside the defined bounds?
						Vector3 screenBounds = screenCenter * (1f - IndicatorOffscreenBorder);
						float boundsY = (cos > 0f) ? screenBounds.y : -screenBounds.y;
						screenPos = new Vector3 (boundsY / cotangent, boundsY, 0f);

						// when out of bounds, get point on appropriate side
						if (screenPos.x > screenBounds.x) // out => right
							screenPos = new Vector3 (screenBounds.x, screenBounds.x * cotangent, 0f);
						else if (screenPos.x < -screenBounds.x) // out => left
							screenPos = new Vector3 (-screenBounds.x, -screenBounds.x * cotangent, 0f);
						screenPos += screenCenter;

						// update indicator rotation
						element.SetIndicatorRotation (Quaternion.Euler (0f, 0f, angle * Mathf.Rad2Deg));

						// update indicator icon
						element.SetIndicatorSprite (IndicatorOffscreenSprite, true);

						// show indicator distance offscreen?
						element.ShowIndicatorDistance (!hideIndicatorDistanceOffscreen);
					} else {
						// reset indicator rotation
						element.SetIndicatorRotation (Quaternion.identity);

						// reset indicator icon
						element.SetIndicatorSprite (null, false);

						// show indicator distance
						element.ShowIndicatorDistance (useIndicatorDistanceText);
					}

					// update indicator values
					element.SetIndicatorPosition (screenPos, _HUDNavigationCanvas.Indicator.ElementContainer);
					element.SetIndicatorScale (distance, IndicatorScaleRadius, IndicatorMinScale);
					element.SetIndicatorActive (true);

					// invoke events
					if (!element.IsInIndicatorRadius) {
						element.IsInIndicatorRadius = true;
						element.OnEnterRadius.Invoke (NavigationElementType.Indicator);
					}
				}
			} else {
				element.SetIndicatorActive (false);
			}
		}


		public Transform GetRotationReference ()
		{
			return (RotationReference == _RotationReference.Camera) ? PlayerCamera.transform : PlayerController;
		}
		#endregion


		#region Subclasses
		[System.Serializable]
		public class _DistanceText
		{
			[Tooltip("Assign a distance text prefab (make sure, it has a text component).")]
			public GameObject TextPrefab;
			[Tooltip("Assign a custom distance text format. {0} will be replaced with the actual distance.")]
			public string TextFormat = "{0}m";
			[Tooltip("The offset of the distance text relative to it's marker.")]
			public Vector3 TextOffset = Vector3.zero;
		}


		[System.Serializable]
		public enum _CompassBarNorth
		{
			Forward, Back, Left, Right
		}


		[System.Serializable]
		public enum _RotationReference
		{
			Camera, Controller
		}
		#endregion
	}


	#region Subclasses
	[System.Serializable]
	public enum RadarTypes { RotateRadar, RotatePlayer };
	#endregion
}
