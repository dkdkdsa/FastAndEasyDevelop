#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FAED_SettingEditor : EditorWindow
{

    [MenuItem("Test/Testing")]
    public static void CreateSettingWindow()
    {

        var window = GetWindow<FAED_SettingEditor>();
        window.minSize = new Vector2(300, 500);
        window.maxSize = window.minSize;
        window.Show();

    }

    private void OnEnable()
    {

        var image = new Image();

        Texture2D texture = Resources.Load<Texture2D>("FAED/Image/FAED_Logo");
        
        image.image = texture;
        image.style.flexShrink = 100;
        image.style.flexGrow = 0.3f;

        rootVisualElement.Add(image);

    }

}
#endif