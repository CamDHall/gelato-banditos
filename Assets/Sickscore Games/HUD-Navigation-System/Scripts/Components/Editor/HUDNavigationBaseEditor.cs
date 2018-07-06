using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using SickscoreGames.HUDNavigationSystem;

public class HUDNavigationBaseEditor : Editor
{
	#region Variables
	public string editorTitle = string.Empty;
	public GUIStyle titleStyle, subtitleStyle, headerStyle, subHeaderStyle, wrapperStyle, boxStyle, helpboxStyle;
	protected Texture2D splashTexture;
	#endregion


	#region Main Methods
	void Awake ()
	{
		splashTexture = (Texture2D)Resources.Load ("splashTexture_System", typeof(Texture2D));
	}


	public override void OnInspectorGUI ()
	{
		// setup custom styles
		if (titleStyle == null) {
			titleStyle = new GUIStyle (GUI.skin.label);
			titleStyle.fontSize = 20;
			titleStyle.fontStyle = FontStyle.Bold;
			titleStyle.alignment = TextAnchor.MiddleCenter;
		}

		if (subtitleStyle == null) {
			subtitleStyle = new GUIStyle (titleStyle);
			subtitleStyle.fontSize = 12;
			subtitleStyle.fontStyle = FontStyle.Italic;
		}

		if (headerStyle == null) {
			headerStyle = new GUIStyle (GUI.skin.label);
			headerStyle.fontStyle = FontStyle.Bold;
			headerStyle.alignment = TextAnchor.UpperLeft;
		}

		if (subHeaderStyle == null) {
			subHeaderStyle = new GUIStyle (GUI.skin.label);
			subHeaderStyle.fontStyle = FontStyle.Normal;
			subHeaderStyle.alignment = TextAnchor.UpperLeft;
		}

		if (wrapperStyle == null) {
			wrapperStyle = new GUIStyle (GUI.skin.box);
			wrapperStyle.normal.textColor = GUI.skin.label.normal.textColor;
			wrapperStyle.padding = new RectOffset (8, 8, 16, 8);
		}

		if (boxStyle == null) {
			boxStyle = new GUIStyle (GUI.skin.box);
			boxStyle.normal.textColor = GUI.skin.label.normal.textColor;
			boxStyle.fontStyle = FontStyle.Bold;
			boxStyle.alignment = TextAnchor.UpperLeft;
		}

		if (helpboxStyle == null) {
			helpboxStyle = new GUIStyle (GUI.skin.GetStyle ("HelpBox"));
			helpboxStyle.fontSize = 11;
			helpboxStyle.richText = true;
			helpboxStyle.padding = new RectOffset (15, 15, 8, 8);
		}

		// SPLASH
		if (splashTexture != null) {
			GUILayoutUtility.GetRect (1f, 3f, GUILayout.ExpandWidth (false));
			Rect rect = GUILayoutUtility.GetRect (GUIContent.none, GUIStyle.none, GUILayout.Height (80f));
			GUI.DrawTexture (rect, splashTexture, ScaleMode.ScaleAndCrop, true, 0f);
		}

		// -- BEGIN WRAPPER --
		EditorGUILayout.BeginVertical (wrapperStyle);

		// render child content
		OnBaseInspectorGUI ();

		// version info
		GUILayout.Space (8);
		EditorGUILayout.LabelField (string.Format ("{0} {1}", HNS.Name, HNS.Version), subtitleStyle);

		// set editor dirty
		EditorUtility.SetDirty (target);

		// -- END WRAPPER --
		EditorGUILayout.EndVertical ();
	}


	protected virtual void OnBaseInspectorGUI () {}
	#endregion
}
