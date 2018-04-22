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

[CustomEditor(typeof(HUDNavigationCanvas))]
public class HUDNavigationCanvasEditor : HUDNavigationBaseEditor
{
	#region Variables
	protected HUDNavigationCanvas hudTarget;
	private bool _radar_, _compassBar_, _indicator_;
	private bool _hasMissingReferences;
	#endregion


	#region Main Methods
	void OnEnable()
	{
		editorTitle = "HUD Navigation Canvas";
		splashTexture = (Texture2D)Resources.Load ("splashTexture_Canvas", typeof(Texture2D));

		hudTarget = (HUDNavigationCanvas)target;
	}


	protected override void OnBaseInspectorGUI ()
	{
		// update serialized object
		serializedObject.Update ();

		// cache serialized properties
		SerializedProperty _pRadar = serializedObject.FindProperty ("Radar");
		SerializedProperty _pCompassBar = serializedObject.FindProperty ("CompassBar");
		SerializedProperty _pIndicator = serializedObject.FindProperty ("Indicator");

		// radar references
		EditorGUILayout.BeginVertical (boxStyle);
		_radar_ = EditorGUILayout.BeginToggleGroup ("Radar References", _radar_);
		DrawReferences (_pRadar, _radar_);
		EditorGUILayout.EndToggleGroup ();
		EditorGUILayout.EndVertical ();

		GUILayout.Space (2); // SPACE

		// compass bar references
		EditorGUILayout.BeginVertical (boxStyle);
		_compassBar_ = EditorGUILayout.BeginToggleGroup ("Compass Bar References", _compassBar_);
		DrawReferences (_pCompassBar, _compassBar_);
		EditorGUILayout.EndToggleGroup ();
		EditorGUILayout.EndVertical ();

		GUILayout.Space (2); // SPACE

		// indicator references
		EditorGUILayout.BeginVertical (boxStyle);
		_indicator_ = EditorGUILayout.BeginToggleGroup ("Indicator References", _indicator_);
		DrawReferences (_pIndicator, _indicator_);
		EditorGUILayout.EndToggleGroup ();
		EditorGUILayout.EndVertical ();

		// check for missing references
		if (_hasMissingReferences)
			EditorGUILayout.HelpBox ("References are missing!", MessageType.Error);
		_hasMissingReferences = false;

		// apply modified properties
		serializedObject.ApplyModifiedProperties ();
	}
	#endregion


	#region Utility Methods
	void DrawReferences (SerializedProperty property, bool value)
	{
		if (value) {
			GUILayout.Space (4); // SPACE
			string parentPath = property.propertyPath;
			while (property.NextVisible (true) && property.propertyPath.StartsWith (parentPath)) {
				EditorGUILayout.PropertyField (property);
				if (property.objectReferenceValue == null)
					_hasMissingReferences = true;
			}
		}
	}
	#endregion
}
