#if UNITY_EDITOR
using FD.System.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FAED_SettingEditor : EditorWindow
{

    private FAED_SettingSO so;
    private static FAED_SettingEditor window;

    [MenuItem("FAED/Setting")]
    public static void CreateSettingWindow()
    {

        window = GetWindow<FAED_SettingEditor>();
        window.minSize = new Vector2(300, 500);
        window.maxSize = window.minSize;
        window.Show();

    }

    private void OnEnable()
    {

        so = Resources.Load<FAED_SettingSO>("FAED/Setting/SettingData");

        #region 이미지

        var image = new Image();

        Texture2D texture = Resources.Load<Texture2D>("FAED/Image/FAED_Logo");
        
        image.image = texture;
        image.style.flexShrink = 100;
        image.style.flexGrow = 0.3f;
        rootVisualElement.Add(image);

        #endregion

        Label label = new Label();
        label.text = "FAED Setting";
        rootVisualElement.Add(label);

        #region 토글
        Toggle toggle = new Toggle();
        toggle.label = "Use pooling";
        rootVisualElement.Add(toggle);
        #endregion

        Button button = new Button(() =>
        {

            so.usePooling = toggle.value;

            if (so.usePooling)
            {

                var ins = ScriptableObject.CreateInstance<FAED_PoolListSO>();
                AssetDatabase.CreateAsset(ins, "Assets/Resources/FAED/Setting/PoolList.Asset");
                so.poolList = Resources.Load<FAED_PoolListSO>("FAED/Setting/PoolList");
                window.Close();

            }

        });
        button.text = "SettingComplete";

        rootVisualElement.Add(button);

       
    }

}
#endif