#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using FD.System.SO;

namespace FD.System.Import
{

    [InitializeOnLoad]
    public class FAED_ImportSetting
    {

        static FAED_ImportSetting()
        {


            if (Directory.Exists(Application.dataPath + "/Resources/FAED/Setting") == false)
            {

                Directory.CreateDirectory(Application.dataPath + "/Resources/FAED/Setting");

            }

            if (Resources.Load("FAED/Setting/SettingData") == null)
            {

                var ins = ScriptableObject.CreateInstance<FAED_SettingSO>();
                AssetDatabase.CreateAsset(ins, "Assets/Resources/FAED/Setting/SettingData.Asset");
                FAED_SettingEditor.CreateSettingWindow();
                
            }


        }

    }

}

#endif