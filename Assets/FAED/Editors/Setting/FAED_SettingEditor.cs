#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FAED_SettingEditor : EditorWindow
{

    public static void CreateSettingWindow()
    {

        var window = GetWindow<FAED_SettingEditor>();

    }

}
#endif