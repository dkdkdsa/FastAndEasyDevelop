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

    }

    private void OnEnable()
    {

        var image = new Image();
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "Image/FAED_Logo.png"));

        image.image = texture;
        image.sourceRect = new Rect(0, 0, 100, 100);
        rootVisualElement.Add(image);

    }

}
#endif