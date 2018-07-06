using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using SickscoreGames.HUDNavigationSystem;

[CustomEditor(typeof(HUDNavigationSystem))]
public class HUDNavigationSystemEditor : HUDNavigationBaseEditor
{
	#region Variables
	protected HUDNavigationSystem hudTarget;
	#endregion


	#region Main Methods
	void OnEnable()
	{
		editorTitle = "HUD Navigation System";
		splashTexture = (Texture2D)Resources.Load ("splashTexture_System", typeof(Texture2D));

		hudTarget = (HUDNavigationSystem)target;
	}


	protected override void OnBaseInspectorGUI ()
	{
		// update serialized object
		serializedObject.Update ();

		// cache serialized properties
		SerializedProperty _pPlayerCamera = serializedObject.FindProperty ("PlayerCamera");
		SerializedProperty _pPlayerController = serializedObject.FindProperty ("PlayerController");
		SerializedProperty _pRotationReference = serializedObject.FindProperty ("RotationReference");

		SerializedProperty _pUseRadar = serializedObject.FindProperty ("useRadar");
		SerializedProperty _pRadarType = serializedObject.FindProperty ("RadarType");
		SerializedProperty _pRadarZoom = serializedObject.FindProperty ("RadarZoom");
		SerializedProperty _pRadarRadius = serializedObject.FindProperty ("RadarRadius");
		SerializedProperty _pRadarMaxRadius = serializedObject.FindProperty ("RadarMaxRadius");

		SerializedProperty _pUseCompassBar = serializedObject.FindProperty ("useCompassBar");
		SerializedProperty _pCompassBarRadius = serializedObject.FindProperty ("CompassBarRadius");
		SerializedProperty _pCompassBarNorth = serializedObject.FindProperty ("CompassBarNorth");
		SerializedProperty _pUseCompassBarDistanceText = serializedObject.FindProperty ("useCompassBarDistanceText");
		SerializedProperty _pCompassBarDistanceText = serializedObject.FindProperty ("CompassBarDistanceText");
		SerializedProperty _pCompassBarDistanceTextPrefab = _pCompassBarDistanceText.FindPropertyRelative ("TextPrefab");
		SerializedProperty _pCompassBarDistanceTextFormat = _pCompassBarDistanceText.FindPropertyRelative ("TextFormat");
		SerializedProperty _pCompassBarDistanceTextOffset = _pCompassBarDistanceText.FindPropertyRelative ("TextOffset");

		SerializedProperty _pUseIndicators = serializedObject.FindProperty ("useIndicators");
		SerializedProperty _pIndicatorRadius = serializedObject.FindProperty ("IndicatorRadius");
		SerializedProperty _pUseIndicatorOffscreen = serializedObject.FindProperty ("useIndicatorOffscreen");
		SerializedProperty _pIndicatorOffscreenBorder = serializedObject.FindProperty ("IndicatorOffscreenBorder");
		SerializedProperty _pIndicatorOffscreenSprite = serializedObject.FindProperty ("IndicatorOffscreenSprite");
		SerializedProperty _pUseIndicatorDistanceText = serializedObject.FindProperty ("useIndicatorDistanceText");
		SerializedProperty _pHideIndicatorDistanceOffscreen = serializedObject.FindProperty ("hideIndicatorDistanceOffscreen");
		SerializedProperty _pIndicatorDistanceText = serializedObject.FindProperty ("IndicatorDistanceText");
		SerializedProperty _pIndicatorDistanceTextPrefab = _pIndicatorDistanceText.FindPropertyRelative ("TextPrefab");
		SerializedProperty _pIndicatorDistanceTextFormat = _pIndicatorDistanceText.FindPropertyRelative ("TextFormat");
		SerializedProperty _pIndicatorDistanceTextOffset = _pIndicatorDistanceText.FindPropertyRelative ("TextOffset");

		SerializedProperty _pUseIndicaterScaling = serializedObject.FindProperty ("useIndicaterScaling");
		SerializedProperty _pIndicatorScaleRadius = serializedObject.FindProperty ("IndicatorScaleRadius");
		SerializedProperty _pIndicatorMinScale = serializedObject.FindProperty ("IndicatorMinScale");

		// references
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.PropertyField (_pPlayerCamera);
		EditorGUILayout.PropertyField (_pPlayerController);
		EditorGUILayout.PropertyField (_pRotationReference, new GUIContent("Rotation Reference", "The transform you want to use as rotation reference."));
		EditorGUILayout.EndVertical ();

		GUILayout.Space (8); // SPACE

		// radar settings
		EditorGUILayout.BeginVertical (boxStyle);
		_pUseRadar.boolValue = EditorGUILayout.BeginToggleGroup ("Enable Radar", _pUseRadar.boolValue);
		if (_pUseRadar.boolValue) {
			GUILayout.Space (8); // SPACE
			EditorGUILayout.LabelField ("Radar Settings", headerStyle);
			EditorGUILayout.PropertyField (_pRadarType);
			EditorGUILayout.Slider (_pRadarZoom, .1f, 5f, "Radar Zoom");
			EditorGUILayout.Slider (_pRadarRadius, 1f, 500f, "Radar Radius");
			EditorGUILayout.Slider (_pRadarMaxRadius, 1f, 500f, "Radar Radius (Border)");
			if (_pRadarMaxRadius.floatValue < _pRadarRadius.floatValue)
				_pRadarMaxRadius.floatValue = _pRadarRadius.floatValue;
		}
		EditorGUILayout.EndToggleGroup ();
		EditorGUILayout.EndVertical ();

		GUILayout.Space (2); // SPACE

		// compass bar settings
		EditorGUILayout.BeginVertical (boxStyle);
		_pUseCompassBar.boolValue = EditorGUILayout.BeginToggleGroup ("Enable Compass Bar", _pUseCompassBar.boolValue);
		if (_pUseCompassBar.boolValue) {
			GUILayout.Space (8); // SPACE
			EditorGUILayout.LabelField ("Compass Bar Settings", headerStyle);
			EditorGUILayout.Slider (_pCompassBarRadius, 1f, 500f, "Compass Bar Radius");
			EditorGUILayout.PropertyField (_pCompassBarNorth, new GUIContent("Compass Bar North"));

			// distance text settings
			GUILayout.Space (4); // SPACE
			EditorGUILayout.BeginVertical (boxStyle);
			_pUseCompassBarDistanceText.boolValue = EditorGUILayout.ToggleLeft ("Use Distance Text", _pUseCompassBarDistanceText.boolValue, subHeaderStyle);
			if (_pUseCompassBarDistanceText.boolValue) {
				GUILayout.Space (4); // SPACE
				EditorGUILayout.PropertyField (_pCompassBarDistanceTextPrefab, new GUIContent("Distance Text Prefab"));
				if (hudTarget.CompassBarDistanceText.TextPrefab != null) {
					if (hudTarget.CompassBarDistanceText.TextPrefab.GetComponent<Text> () == null)
						EditorGUILayout.HelpBox ("Prefab has no Text component!", MessageType.Error);
				} else {
					EditorGUILayout.HelpBox ("Prefab missing! Distance text will not be rendered.", MessageType.Warning);
				}
				EditorGUILayout.PropertyField (_pCompassBarDistanceTextFormat, new GUIContent("Distance Text Format"));
				EditorGUILayout.PropertyField (_pCompassBarDistanceTextOffset, new GUIContent("Distance Text Offset"));
			}
			EditorGUILayout.EndVertical ();
		}
		EditorGUILayout.EndToggleGroup ();
		EditorGUILayout.EndVertical ();

		GUILayout.Space (2); // SPACE

		// indicator settings
		EditorGUILayout.BeginVertical (boxStyle);
		_pUseIndicators.boolValue = EditorGUILayout.BeginToggleGroup ("Use Indicators", _pUseIndicators.boolValue);
		if (_pUseIndicators.boolValue) {
			GUILayout.Space (8); // SPACE
			EditorGUILayout.LabelField ("Indicators Settings", headerStyle);
			EditorGUILayout.Slider (_pIndicatorRadius, 1f, 500f, "Indicator Radius");

			// off-screen indicator settings
			GUILayout.Space (4); // SPACE
			EditorGUILayout.BeginVertical (boxStyle);
			_pUseIndicatorOffscreen.boolValue = EditorGUILayout.ToggleLeft ("Use Offscreen Indicator", _pUseIndicatorOffscreen.boolValue, subHeaderStyle);
			if (_pUseIndicatorOffscreen.boolValue) {
				GUILayout.Space (4); // SPACE
				EditorGUILayout.Slider (_pIndicatorOffscreenBorder, 0f, 1f, "Screen Border");
				EditorGUILayout.PropertyField (_pIndicatorOffscreenSprite, new GUIContent("Offscreen Sprite"));
			}
			EditorGUILayout.EndVertical ();

			// distance text settings
			GUILayout.Space (4); // SPACE
			EditorGUILayout.BeginVertical (boxStyle);
			_pUseIndicatorDistanceText.boolValue = EditorGUILayout.ToggleLeft ("Use Distance Text", _pUseIndicatorDistanceText.boolValue, subHeaderStyle);
			if (_pUseIndicatorDistanceText.boolValue) {
				GUILayout.Space (4); // SPACE
				_pHideIndicatorDistanceOffscreen.boolValue = EditorGUILayout.Toggle ("Hide Distance Offscreen", _pHideIndicatorDistanceOffscreen.boolValue);
				EditorGUILayout.PropertyField (_pIndicatorDistanceTextPrefab, new GUIContent("Distance Text Prefab"));
				if (hudTarget.IndicatorDistanceText.TextPrefab != null) {
					if (hudTarget.IndicatorDistanceText.TextPrefab.GetComponent<Text> () == null)
						EditorGUILayout.HelpBox ("Prefab has no Text component!", MessageType.Error);
				} else {
					EditorGUILayout.HelpBox ("Prefab missing! Distance text will not be rendered.", MessageType.Warning);
				}
				EditorGUILayout.PropertyField (_pIndicatorDistanceTextFormat, new GUIContent("Distance Text Format"));
				EditorGUILayout.PropertyField (_pIndicatorDistanceTextOffset, new GUIContent("Distance Text Offset"));
			}
			EditorGUILayout.EndVertical ();

			// indicator scaling settings
			GUILayout.Space (4); // SPACE
			EditorGUILayout.BeginVertical (boxStyle);
			_pUseIndicaterScaling.boolValue = EditorGUILayout.ToggleLeft ("Use Distance Scaling", _pUseIndicaterScaling.boolValue, subHeaderStyle);
			if (_pUseIndicaterScaling.boolValue) {
				GUILayout.Space (4); // SPACE
				EditorGUILayout.BeginVertical ();
				EditorGUILayout.Slider (_pIndicatorScaleRadius, 1f, 500f, "Scale Radius");
				if (hudTarget.IndicatorScaleRadius > hudTarget.IndicatorRadius)
					hudTarget.IndicatorScaleRadius = hudTarget.IndicatorRadius;
				EditorGUILayout.Slider (_pIndicatorMinScale, .1f, 1f, "Minimum Scale");
				EditorGUILayout.HelpBox ("Indicator will be scaled by distance within the defined radius.", MessageType.Info);
				EditorGUILayout.EndVertical ();
			}
			EditorGUILayout.EndVertical ();
		}
		EditorGUILayout.EndToggleGroup ();
		EditorGUILayout.EndVertical ();

		// apply modified properties
		serializedObject.ApplyModifiedProperties ();
	}
	#endregion
}
