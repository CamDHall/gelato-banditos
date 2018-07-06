using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SickscoreGames;

namespace SickscoreGames.HUDNavigationSystem
{
	[CreateAssetMenu (fileName="New Element Settings", menuName="HUD Navigation System/New Element Settings")]
	public class HUDNavigationElementSettings : ScriptableObject
	{
		#region Variables
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
		#endregion
	}
}
