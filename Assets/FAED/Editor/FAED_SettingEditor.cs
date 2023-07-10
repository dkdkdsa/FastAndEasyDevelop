using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FD.Core.Editors
{

    public class FAED_SettingEditor : EditorWindow
    {
        
        [MenuItem("FAED/Setting")]
        public static void CreateSettingWindow()
        {

            var window = GetWindow<FAED_SettingEditor>();
            window.titleContent.text = "FAED_Setting";
            window.maxSize = new Vector2(300, 500);
            window.minSize = new Vector2(300, 500);
            window.Show();       

        }

        private void OnEnable()
        {

        

        }


    }

}