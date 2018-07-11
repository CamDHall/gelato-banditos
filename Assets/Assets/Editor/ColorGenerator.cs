using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ColorGenerator : EditorWindow {

    public Color regularColor;
    public Color warningColor;
    public Color newsColor;
    public Color goodNewsColor;

    [MenuItem("Editor Tools/Color Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ColorGenerator));
    }
    private void OnGUI()
    {
        GUILayout.Label("Color Generator", EditorStyles.boldLabel);
        regularColor = EditorGUILayout.ColorField("Regular Message", regularColor);
        warningColor = EditorGUILayout.ColorField("Warning Message", warningColor);
        newsColor = EditorGUILayout.ColorField("News Message", newsColor);
        goodNewsColor = EditorGUILayout.ColorField("Good News Message", goodNewsColor);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate color strings")) GenerateColorStrings();

        EditorGUILayout.Space();
    }

    void GenerateColorStrings()
    {
        StoredData data = (StoredData)ScriptableObject.CreateInstance("StoredData");

        AssetDatabase.CreateAsset(data, "Assets/storeddata.asset");
        //this basically tells Unity that our scriptable object variable has changed and needs to be saved
        EditorUtility.SetDirty(data);

        data.REGULAR_COLOR = "#" + ColorUtility.ToHtmlStringRGB(regularColor);
        data.NEWS_COLOR = "#" + ColorUtility.ToHtmlStringRGB(newsColor);
        data.GOOD_NEWS_COLOR = "#" + ColorUtility.ToHtmlStringRGB(goodNewsColor);
        data.WARNING_COLOR= "#" + ColorUtility.ToHtmlStringRGB(warningColor);
    }
}
