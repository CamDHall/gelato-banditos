using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SickscoreGames;

namespace SickscoreGames.HUDNavigationSystem
{
	public static class HUDNavigationExtensions
	{
		#region Extension Methods
		public static float GetDistance (this HUDNavigationElement element, Transform other)
		{
			return Vector2.Distance (new Vector2 (element.transform.position.x, element.transform.position.z), new Vector2 (other.position.x, other.position.z));
		}


		public static Vector3 GetPosition (this HUDNavigationElement element)
		{
			return element.transform.position;
		}


		public static Vector3 GetPositionOffset (this HUDNavigationElement element, Transform other)
		{
			return element.transform.position - other.position;
		}


		public static float GetIconRadius (this HUDNavigationElement element)
		{
			return element.RadarIcon.IconSize / 2;
		}


		public static float GetRadius (this RectTransform rect)
		{
			Vector3[] arr = new Vector3[4];
			rect.GetLocalCorners (arr);
			float _radius = Mathf.Abs (arr [0].y);
			if (Mathf.Abs (arr [0].x) < Mathf.Abs (arr [0].y))
				_radius = Mathf.Abs (arr [0].x);

			return _radius;
		}


		public static bool IsVisibleOnScreen (this HUDNavigationElement element, Vector3 screenPos)
		{
			return screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height;
		}


		public static void SetMarkerPosition (this HUDNavigationElement element, NavigationElementType elementType, Vector3 markerPos, RectTransform parentRect = null)
		{
			if (elementType == NavigationElementType.Radar) {
				// set radar marker position
				if (element.RadarMarker != null)
					element.RadarMarker.rectTransform.localPosition = markerPos;					
			} else if (elementType == NavigationElementType.CompassBar) {
				// set compass bar marker position
				if (element.CompassBarMarker != null)
					element.CompassBarMarker.transform.position = new Vector3 (markerPos.x + parentRect.localPosition.x, parentRect.position.y, 0f);
			}
		}


		public static void SetMarkerActive (this HUDNavigationElement element, NavigationElementType elementType, bool value)
		{
			Image marker = null;
			if (elementType == NavigationElementType.Radar)
				marker = element.RadarMarker;
			else if (elementType == NavigationElementType.CompassBar)
				marker = element.CompassBarMarker;

			// set marker active/inactive
			if (marker != null) {
				// only update whifen value has changed
				if (value != marker.gameObject.activeSelf) {
					if (value) // appeared
						element.OnAppear.Invoke (elementType);
					else // disappeared
						element.OnDisappear.Invoke (elementType);

					// set active state
					marker.gameObject.SetActive (value);
				}
			}
		}


		public static void ShowIndicatorDistance (this HUDNavigationElement element, bool value)
		{
			if (element.IndicatorDistance == null)
				return;

			// only update if value has changed
			if (value != element.IndicatorDistance.gameObject.activeSelf)
				element.IndicatorDistance.gameObject.SetActive (value);
		}


		public static void SetIndicatorSprite (this HUDNavigationElement element, Sprite sharedOffscreenSprite, bool offscreen = false)
		{
			if (element.Indicator == null)
				return;

			// get indicator icon settings
			IconSettings iconSettings = element.IndicatorIcon;
			if (offscreen)
				iconSettings = element.OffscreenIndicatorIcon;

			// get new indicator sprite
			Sprite newSprite = (iconSettings.OverrideIcon != null) ? iconSettings.OverrideIcon : element.Icon;
			if (offscreen && iconSettings.OverrideIcon == null)
				newSprite = sharedOffscreenSprite;

			// set new indicator sprite
			if (element.Indicator.sprite != newSprite)
				element.Indicator.sprite = newSprite;
			
			// set indicator size and color
			element.Indicator.rectTransform.sizeDelta = Vector2.one * iconSettings.IconSize;
			element.Indicator.color = iconSettings.IconColor;
		}


		public static void SetIndicatorActive (this HUDNavigationElement element, bool value)
		{
			if (element.Indicator == null)
				return;

			// only update, if value has changed
			if (value != element.Indicator.gameObject.activeSelf) {
				if (value) // appeared
					element.OnAppear.Invoke (NavigationElementType.Indicator);
				else // disappeared
					element.OnDisappear.Invoke (NavigationElementType.Indicator);
				
				// set indicator active/inactive
				element.Indicator.gameObject.SetActive (value);
			}
		}


		public static void SetIndicatorPosition (this HUDNavigationElement element, Vector3 indicatorPos, RectTransform parentRect)
		{
			if (element.Indicator == null)
				return;

			// set indicator position
			element.Indicator.transform.position = new Vector3 (indicatorPos.x + parentRect.localPosition.x, indicatorPos.y + parentRect.localPosition.y, 0f) + element.IndicatorOffset;
		}


		public static void SetIndicatorRotation (this HUDNavigationElement element, Quaternion rotation)
		{
			if (element.Indicator == null)
				return;

			// set indicator rotation
			element.Indicator.transform.rotation = rotation;
		}


		public static void SetIndicatorScale (this HUDNavigationElement element, float distance, float scaleRadius, float minScale)
		{
			if (element.Indicator == null)
				return;

			// set indicator scale
			float scale = (distance - 1f) / (scaleRadius - 1f);
			scale = Mathf.Clamp01 (scale);
			element.Indicator.rectTransform.localScale = Vector2.Lerp (Vector2.one * minScale, Vector2.one, scale);
		}
		#endregion
	}
}
