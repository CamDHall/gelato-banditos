using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SickscoreGames;

namespace SickscoreGames.HUDNavigationSystem
{
	[AddComponentMenu (HNS.Name + "/HUD Navigation Canvas"), DisallowMultipleComponent]
	public class HUDNavigationCanvas : MonoBehaviour
	{
		private static HUDNavigationCanvas _Instance;
		public static HUDNavigationCanvas Instance {
			get {
				if (_Instance == null) {
					_Instance = FindObjectOfType<HUDNavigationCanvas> ();
				}
				return _Instance;
			}
		}


		#region Variables
		public _RadarReferences Radar;
		public _CompassBarReferences CompassBar;
		public _IndicatorReferences Indicator;


		private Vector3 _cbInitialPosition;
		private float _cbPixelRotationAngle;
		private float _cbCachedScreenWidth;
		public float _cbCurrentDegrees = 0f;

		protected Vector3 __cbNorthDirection = Vector3.zero;
		private Vector3 _cbNorthDirection {
			get {
				if (__cbNorthDirection == Vector3.zero) {
					HUDNavigationSystem._CompassBarNorth north = _HUDNavigationSystem.CompassBarNorth;
					switch (north) {
						case HUDNavigationSystem._CompassBarNorth.Forward:
							__cbNorthDirection = Vector3.forward;
							break;
						case HUDNavigationSystem._CompassBarNorth.Back:
							__cbNorthDirection = Vector3.back;
							break;
						case HUDNavigationSystem._CompassBarNorth.Left:
							__cbNorthDirection = Vector3.left;
							break;
						case HUDNavigationSystem._CompassBarNorth.Right:
							__cbNorthDirection = Vector3.right;
							break;
						default:
							break;
					}
				}

				return __cbNorthDirection;
			}
			set { __cbNorthDirection = value; }
		}

		protected HUDNavigationSystem __HUDNavigationSystem;
		private HUDNavigationSystem _HUDNavigationSystem {
			get {
				if (__HUDNavigationSystem == null)
					__HUDNavigationSystem = HUDNavigationSystem.Instance;

				return __HUDNavigationSystem;
			}
		}
		#endregion


		#region Main Methods
		void Awake ()
		{
			_Instance = this;
		}
		#endregion


		#region Radar Methods
		public void InitRadar ()
		{
			// check references
			if (Radar.Panel == null || Radar.Radar == null || Radar.PlayerIndicator == null || Radar.ElementContainer == null) {
				Debug.LogError ("Radar references are missing! Please assign them on the HUDNavigationCanvas component.");
				this.enabled = false;
				return;
			}

			// show radar
			ShowRadar (true);

			// max radius needs to be greater/equal to radius
			if (_HUDNavigationSystem.RadarMaxRadius < _HUDNavigationSystem.RadarRadius)
				_HUDNavigationSystem.RadarMaxRadius = _HUDNavigationSystem.RadarRadius;
		}


		public void ShowRadar (bool value)
		{
			if (Radar.Panel != null)
				Radar.Panel.gameObject.SetActive (value);
		}


		public void UpdateRadar ()
		{
			// handle radar rotation
			R_HandleRadarRotation ();
		}


		void R_HandleRadarRotation ()
		{
			// rotate by rotation reference
			Transform rotationReference = _HUDNavigationSystem.GetRotationReference ();
			if (_HUDNavigationSystem.RadarType == RadarTypes.RotateRadar) {
				// set radar rotation
				Radar.Radar.transform.rotation = Quaternion.Euler (Radar.Panel.transform.eulerAngles.x, Radar.Panel.transform.eulerAngles.y, rotationReference.eulerAngles.y);
				Radar.PlayerIndicator.transform.rotation = Radar.Panel.transform.rotation;
			} else {
				// set player indicator rotation
				Radar.Radar.transform.rotation = Radar.Panel.transform.rotation;
				Radar.PlayerIndicator.transform.rotation = Quaternion.Euler (Radar.Panel.transform.eulerAngles.x, Radar.Panel.transform.eulerAngles.y, -rotationReference.eulerAngles.y);
			}
		}
		#endregion


		#region Compass Bar Methods
		public void InitCompassBar ()
		{
			// check references
			if (CompassBar.Panel == null || CompassBar.Compass == null || CompassBar.ElementContainer == null) {
				Debug.LogError ("Compass Bar references are missing! Please assign them on the HUDNavigationCanvas component.");
				this.enabled = false;
				return;
			}

			// show compass bar
			ShowCompassBar (true);

			// assign initial variables
			_cbInitialPosition = CompassBar.Compass.transform.position;
			_cbPixelRotationAngle = ((CompassBar.Compass.rect.width / 2f) / 360f) * this.transform.localScale.x;
		}


		public void ShowCompassBar (bool value)
		{
			if (CompassBar.Panel != null)
				CompassBar.Panel.gameObject.SetActive (value);
		}


		public void UpdateCompassBar ()
		{
			// handle screen resolution
			CB_HandleScreenResolution ();

			// handle compass bar position
			CB_HandleCompassBarPosition ();
		}


		void CB_HandleScreenResolution ()
		{
			// update initial position if screen resolution has changed
			if (Screen.width != _cbCachedScreenWidth) {
				_cbInitialPosition = CompassBar.Compass.transform.position;
				_cbPixelRotationAngle = ((CompassBar.Compass.rect.width / 2f) / 360f) * this.transform.localScale.x;
				_cbCachedScreenWidth = Screen.width;
			}
		}


		void CB_HandleCompassBarPosition ()
		{
			// calculate and set compass bar position
			Transform rotationReference = _HUDNavigationSystem.GetRotationReference ();
			Vector3 perpDirection = Vector3.Cross (_cbNorthDirection, rotationReference.forward);
			float direction = Vector3.Dot (perpDirection, Vector3.up);
			float angle = Vector3.Angle (new Vector3 (rotationReference.forward.x, 0f, rotationReference.forward.z), _cbNorthDirection);
			CompassBar.Compass.transform.position = _cbInitialPosition + -(new Vector3 (angle * Mathf.Sign (direction) * _cbPixelRotationAngle, 0f, 0f));

			// calculate 0-360 degrees value
			_cbCurrentDegrees = (perpDirection.y >= 0f) ? angle : 360f - angle;
		}
		#endregion


		#region Indicator Methods
		public void InitIndicators ()
		{
			// check references
			if (Indicator.Panel == null || Indicator.ElementContainer == null) {
				Debug.LogError ("Indicator references are missing! Please assign them on the HUDNavigationCanvas component.");
				this.enabled = false;
				return;
			}

			// show indicators
			ShowIndicators (true);
		}


		public void ShowIndicators (bool value)
		{
			if (Indicator.Panel != null)
				Indicator.Panel.gameObject.SetActive (value);
		}
		#endregion


		#region Utility Methods
		#endregion


		#region Subclasses
		[System.Serializable]
		public class _RadarReferences
		{
			public RectTransform Panel;
			public RectTransform Radar;
			public RectTransform PlayerIndicator;
			public RectTransform ElementContainer;
		}


		[System.Serializable]
		public class _CompassBarReferences
		{
			public RectTransform Panel;
			public RectTransform Compass;
			public RectTransform ElementContainer;
		}


		[System.Serializable]
		public class _IndicatorReferences
		{
			public RectTransform Panel;
			public RectTransform ElementContainer;
		}
		#endregion
	}
}
