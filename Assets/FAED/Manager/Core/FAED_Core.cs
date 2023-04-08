using FD.System.Manager;
using System.Collections;
using System.Collections.Generic;
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
                DontDestroyOnLoad(go);

                if(poolManager == null)
                {



                }

            }

        }

    }

}