using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Core
{

    public class FAED_Core
    {

        private static FAED_PoolManager poolManager;
        private static FAED_Core instance;

        public static FAED_PoolManager PoolManager 
        {

            get 
            {

                Init();
                return poolManager; 

            } 

        }
        public static FAED_Core Instance 
        { 
            get 
            {

                Init();
                return instance; 

            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {          

            if(instance == null) 
            {
                
                instance = new FAED_Core();

                GameObject go = new GameObject("_@*FAED_CORE*@_");
                var res = Resources.Load<FAED_SettingSO>("FAED/SettingSO");

                if (poolManager == null && res.usePooling)
                {

                    poolManager = new FAED_PoolManager(res.poolingSO, go.transform);

                }

            }



        }

    }

}