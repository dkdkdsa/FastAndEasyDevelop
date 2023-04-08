using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.System.SO
{

    public class FAED_PoolListSO : ScriptableObject
    {

        [Serializable]
        public class FAED_PoolList
        {

            public string poolName;
            public int poolCount;
            public GameObject poolObj;

        }

        public List<FAED_PoolList> poolList = new List<FAED_PoolList>();

    }

}