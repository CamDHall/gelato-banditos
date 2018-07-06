using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Serialization;
#endif
using SickscoreGames.HUDNavigationSystem;

[CustomEditor(typeof(HUDNavigationElementSettings))]
public class HUDNavigationElementSettingsEditor : HUDNavigationBaseEditor
{
	#region Variables
	protected HUDNavigationElementSettings hudTarget;
	private bool _icon_, _radar_, _compassBar_, _indicator_, _events_;
	#endregion


	#region Main Methods
	void OnEnable ()
	{
		editorTitle = "HUD Settings Asset";
		splashTexture = (Texture2D)Resources.Load ("splashTexture_Settings", typeof(Texture2D));

		hudTarget = (HUDNavigationElementSettings)target;
	}


	protected override void OnBaseInspectorGUI ()
	{
		// update serialized object
		serializedObject.Update ();

		// cache serialized properties
		SerializedProperty _pIcon = serializedObject.FindProperty ("Icon");
		SerializedProperty _pRadar = serializedObject.FindProperty ("RadarIcon");
		SerializedProperty _pCompassBar = serializedObject.FindProperty ("CompassBarIcon");
		SerializedProperty _pIndicator = serializedObject.FindProperty ("IndicatorIcon");
		SerializedProperty _pOffscreenIndicator = serializedObject.FindProperty ("OffscreenIndicatorIcon");

		// icon settings
		EditorGUILayout.BeginVertical (boxStyle);
		_icon_ = EditorGUILayout.BeginToggleGroup ("Icon Settings", _icon_);
		if (_icon_) {
			GUILayout.Space (4); // SPACE
			EditorGUILayout.PropertyField (_pIcon, new GUIContent ("Shared Icon"));
			GUILayout.Space (4); // SPACE
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (_pRadar, new GUIContent ("Radar Icon"), true);
			EditorGUILayout.PropertyField (_pCompassBar, new GUIContent ("CompassBar Icon"), true);
			EditorGUILayout.PropertyField (_pIndicator, new GUIContent ("Indicator Icon"), true);
			EditorGUILayout.PropertyField (_pOffscreenIndicator, new GUIContent ("Offscreen Indicator Icon"), true);
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndToggleGroup ();
		EditorGUILayout.EndVertical ();

		// radar settings
		EditorGUILayout.BeginVertical (boxStyle);
		_radar_ = EditorGUILayout.BeginToggleGroup ("Radar Settings", _radar_);
		if (_radar_) {
			GUILayout.Space (4); // SPACE
			hudTarget.HideInRadar = EditorGUILayout.Toggle ("Hide In Radar", hudTarget.HideInRadar);
			if (hudTarget.HideInRadar)
				EditorGUILayout.HelpBox ("Element will be hidden within the radar.", MessageType.Info);
		}
		EditorGUILayout.EndToggleGroup ();
		EditorGUILayout.EndVertical ();

		// compass bar settings
		EditorGUILayout.BeginVertical (boxStyle);
		_compassBar_ = EditorGUILayout.BeginToggleGroup ("Compass Bar Settings", _compassBar_);
		if (_compassBar_) {
			GUILayout.Space (4); // SPACE
			hudTarget.HideInCompassBar = EditorGUILayout.Toggle ("Hide In Compass Bar", hudTarget.HideInCompassBar);
			if (hudTarget.HideInCompassBar) {
				EditorGUILayout.HelpBox ("Element will be hidden within the compass bar.", MessageType.Info);
			} else {
				hudTarget.IgnoreCompassBarRadius = EditorGUILayout.Toggle ("Ignore Radius", hudTarget.IgnoreCompassBarRadius);
				if (hudTarget.IgnoreCompassBarRadius)
					EditorGUILayout.HelpBox ("The element will always be rendered within the compass bar, independent of the actual distance.", MessageType.Info);
			}
		}
		EditorGUILayout.EndToggleGroup ();
		EditorGUILayout.EndVertical ();

		// indicator settings
		EditorGUILayout.BeginVertical (boxStyle);
		_indicator_ = EditorGUILayout.BeginToggleGroup ("Indicator Settings", _indicator_);
		if (_indicator_) {
			GUILayout.Space (4); // SPACE
			hudTarget.ShowIndicator = EditorGUILayout.Toggle ("Show Indicator", hudTarget.ShowIndicator);
			if (hudTarget.ShowIndicator) {
				HUDNavigationSystem hudNavigationSystem = FindObjectOfType<HUDNavigationSystem> ();
				if (hudNavigationSystem != null && !hudNavigationSystem.useIndicators)
					EditorGUILayout.HelpBox ("Indicators are deactivated!\nEnable this feature on the HUDNavigationSystem component.", MessageType.Warning);
				hudTarget.IgnoreIndicatorRadius = EditorGUILayout.Toggle ("Ignore Radius", hudTarget.IgnoreIndicatorRadius);
				hudTarget.IndicatorOffset = EditorGUILayout.Vector3Field ("Indicator Offset", hudTarget.IndicatorOffset);
				if (hudTarget.IgnoreIndicatorRadius)
					EditorGUILayout.HelpBox ("The indicator will always be rendered, independent of the actual distance.", MessageType.Info);
			}
		}
		EditorGUILayout.EndToggleGroup ();
		EditorGUILayout.EndVertical ();

		// apply modified properties
		serializedObject.ApplyModifiedProperties ();
	}
	#endregion
}
