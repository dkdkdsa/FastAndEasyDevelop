using FD.System.Manager;
using FD.System.SO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FD.System.Core
{

    public class FAED_Core : MonoBehaviour
    {

        private static FAED_PoolManager poolManager;
        private static FAED_Core instance;

        public static FAED_PoolManager PoolManager { get { Init(); return poolManager; } }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {

            if(instance == null)
            {

                GameObject go = new GameObject();
                instance = go.AddComponent<FAED_Core>();
                DontDestroyOnLoad(go);
                var so = Resources.Load<FAED_SettingSO>("FAED/Setting/FAEDSetting");

                if (poolManager == null && so.usePooling == true)
                {

                    poolManager = new FAED_PoolManager(so.poolList, go.transform);

                }

            }

        }

    }

}